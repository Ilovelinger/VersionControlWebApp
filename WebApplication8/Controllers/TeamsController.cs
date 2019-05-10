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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebApplication8.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// HttpGet method for viewing team list.
        /// </summary>
        /// <returns>View and list</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Team.ToListAsync());
        }

        /// <summary>
        /// HttpGet method for viewing team detail.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View and viewmodel</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .FirstOrDefaultAsync(m => m.teamId == id);
            if (team == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("isRegistered") == "No")
            {
                ViewData["ShowButton"] = true;
            }

            if(HttpContext.Session.GetString("isRegistered") == "Yes")
            {
                ViewData["ShowButton"] = false;
            }


            TeamDetailViewModel viewModel = await GetTeamDetailViewModelFromTeam(team);

            return View(viewModel);
        }

        /// <summary>
        /// HttpPost method for viewing team detail and register in a team
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>View and viewmodel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Player,CommonUser")]
        public async Task<IActionResult> Details(TeamDetailViewModel viewModel)
        { 


            if (ModelState.IsValid)
            {

                Team team = await _context.Team
                    .SingleOrDefaultAsync(m => m.teamId == viewModel.TeamID);

                if (team == null)
                    {
                        return NotFound();
                    }

                string userid = HttpContext.Session.GetString("tempUserId");

                if (userid == null)
                    return NotFound();

                RegisteredUser user = new RegisteredUser();

                ApplicationUser tempuser = await _context.Users.SingleOrDefaultAsync(m => m.Id == userid);

                tempuser.RelatedTeam = team;

                user.RelatedTeam = tempuser.RelatedTeam;
                user.RelatedUser = tempuser;
                user.Firstname = tempuser.Firstname;
                user.Surname = tempuser.Surname;
                user.FullName = tempuser.FullName;
                user.RelatedTeamName = team.teamName;

                user.isRegistered = "Yes";
                tempuser.isRegistered = "Yes";
                //Add the comments to the database.
                _context.TeamRegisteredUsers.Add(user);
                await _context.SaveChangesAsync();

                viewModel = await GetTeamDetailViewModelFromTeam(team);
      

            }
            return View(viewModel);
        }


        /// <summary>
        /// Set the team and user list for viewmodel
        /// </summary>
        /// <param name="team"></param>
        /// <returns>viewmodel</returns>
        private async Task<TeamDetailViewModel> GetTeamDetailViewModelFromTeam(Team team)
        {
            TeamDetailViewModel viewModel = new TeamDetailViewModel();

            viewModel.Team = team;

            List<RegisteredUser> users = await _context.TeamRegisteredUsers.Include(u => u.RelatedUser)
                .Where(m => m.RelatedTeam == team).ToListAsync();

            viewModel.RegisteredUsers = users;

            return viewModel;

        }

        /// <summary>
        /// HttpGet method for creating team
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// HttpPost method for creating team
        /// </summary>
        /// <param name="team"></param>
        /// <returns>View and team</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("teamName")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }


        /// <summary>
        /// HttpGet method for editing team
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View and team</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

       /// <summary>
       /// HttpPost method for editing team
       /// </summary>
       /// <param name="id"></param>
       /// <param name="team"></param>
       /// <returns>View and team</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("teamName")] Team team)
        {
            if (id != team.teamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.teamId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        /// <summary>
        /// HttpGet method for deleting team
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View and team</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Team
                .FirstOrDefaultAsync(m => m.teamId == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        /// <summary>
        /// HttpPost method for deleting team
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirect to action</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Team.FindAsync(id);
            _context.Team.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// check if the team exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>team</returns>
        private bool TeamExists(int id)
        {
            return _context.Team.Any(e => e.teamId == id);
        }
    }
}
