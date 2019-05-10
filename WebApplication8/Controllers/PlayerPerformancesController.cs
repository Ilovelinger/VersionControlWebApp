using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class PlayerPerformancesController : Controller
    {
        //Add reference to the database.
        private readonly ApplicationDbContext _context;

        public PlayerPerformancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// HttpGet method for viewing player performance.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View and performance list</returns>
        public async Task<IActionResult> Index(int ?id)
        {
            var allperformance = _context.PlayerPerformance.ToList();

            Match match = _context.Matches.SingleOrDefault(u => u.matchid == id);


            var performances = allperformance.Where(x => x.RelatedMatch == match).ToList();

            return View(performances);
        }


        /// <summary>
        /// HttpGet method for creating player performance.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync(int? id)
        {
            //Match match = await _context.Matches
            //    .SingleOrDefaultAsync(m => m.matchid == id);

            TempData["tempmatchid"] = id;
            TempData.Keep();

            Match match = _context.Matches.Include(u => u.RelatedTeam1).SingleOrDefault(u => u.matchid == id);

            List<Team> teams = new List<Team>();

            teams.Add(new Team() { teamName = match.team1Name });
            teams.Add(new Team() { teamName = match.team2Name });


            var allplayers = _context.TeamRegisteredUsers.ToList();
            var players = allplayers.Where(x => x.RelatedTeamName == match.team1Name).ToList();
            var playerstwo = allplayers.Where(x => x.RelatedTeamName == match.team2Name).ToList();

            players.AddRange(playerstwo);


            ViewBag.drolistmenu1 = teams.Select(g => new SelectListItem()
            {
                Text = g.teamName,
                Value = g.teamName,
                Selected = false
            });

            ViewBag.drolistmenu2 = players.Select(m => new SelectListItem()
            {
                Text = m.FullName,
                Value = m.FullName,
                Selected = false
            });

            return View("Create");
        }

        /// <summary>
        /// HttpPost action for creating player performance.
        /// </summary>
        /// <param name="playerPerformance"></param>
        /// <returns>View and player performance</returns>
        [HttpPost, ActionName("CreatePlayerPerformance")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("linkedTeamname,linkedplayername,startup,substitute,goals,assists,keypasses,keydribbles,clearances,saves,yellowcard,redcard")] PlayerPerformance playerPerformance)
        {
            if (ModelState.IsValid)
            {
                var linkedteamname = playerPerformance.linkedplayername;
                var linkedplayername = playerPerformance.linkedplayername;

                Team linkedteam = await _context.Team
                    .SingleOrDefaultAsync(m => m.teamName == linkedteamname);

                ApplicationUser linkeduser = await _context.Users.Include(u => u.RelatedTeam).SingleOrDefaultAsync(m => m.FullName == linkedplayername);

                var tempteam = linkeduser.RelatedTeam;

                if (tempteam.teamName != playerPerformance.linkedTeamname)
                {
                    ViewData["error"] = true;
                    return RedirectToAction("MatchList", "Match",ViewData["error"]);
                }

                ViewData["error"] = false;

                int tempmatchid = (int)TempData["tempmatchid"];
                Match match = await _context.Matches
                .SingleOrDefaultAsync(m => m.matchid == tempmatchid);

                playerPerformance.RelatedMatch = match;
                playerPerformance.RelatedUser = linkeduser;

                _context.Add(playerPerformance);
                await _context.SaveChangesAsync();
                return RedirectToAction("MatchList", "Match",ViewData["error"]); 
            }
            return View(playerPerformance);
        }

        /// <summary>
        /// HttpGet method for editing player performance.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View and player performance</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerPerformance = await _context.PlayerPerformance.FindAsync(id);
            if (playerPerformance == null)
            {
                return NotFound();
            }
            return View(playerPerformance);
        }

        /// <summary>
        /// HttpPost method for editing player performance
        /// </summary>
        /// <param name="id"></param>
        /// <param name="playerPerformance"></param>
        /// <returns>View and player performance</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("startup,substitute,goals,assists,keypasses,keydribbles,clearances,saves,yellowcard,redcard")] PlayerPerformance playerPerformance)
        {

            if (id != playerPerformance.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playerPerformance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerPerformanceExists(playerPerformance.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MatchList", "Match");
            }
            return View(playerPerformance);
        }

        /// <summary>
        /// HttpGet method for deleting player performance.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View and playerperformance</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerPerformance = await _context.PlayerPerformance
                .FirstOrDefaultAsync(m => m.id == id);
            if (playerPerformance == null)
            {
                return NotFound();
            }

            return View(playerPerformance);
        }

       /// <summary>
       /// HttpPost method for deleting player performance
       /// </summary>
       /// <param name="id"></param>
       /// <returns>Redirect to action</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var playerPerformance = await _context.PlayerPerformance.FindAsync(id);
            _context.PlayerPerformance.Remove(playerPerformance);
            await _context.SaveChangesAsync();
            return RedirectToAction("MatchList","Match");
        }

        private bool PlayerPerformanceExists(int id)
        {
            return _context.PlayerPerformance.Any(e => e.id == id);
        }
    }
}
