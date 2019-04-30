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

namespace WebApplication8.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Team.ToListAsync());
        }

        // GET: Teams/Details/5
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

            //string userid = HttpContext.Session.GetString("tempUserId");

            //if (userid == null)
            //    return NotFound();

            //ApplicationUser user = await _context.Users.SingleOrDefaultAsync(m => m.Id == userid);

            //if (user.Nickname != null)
            //    return NotFound();

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


        private async Task<TeamDetailViewModel> GetTeamDetailViewModelFromTeam(Team team)
        {
            TeamDetailViewModel viewModel = new TeamDetailViewModel();

            viewModel.Team = team;

            List<RegisteredUser> users = await _context.TeamRegisteredUsers
                .Where(m => m.RelatedTeam == team).ToListAsync();

            viewModel.RegisteredUsers = users;
            return viewModel;

        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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


        // GET: Teams/Edit/5
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

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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

        // GET: Teams/Delete/5
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

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Team.FindAsync(id);
            _context.Team.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Team.Any(e => e.teamId == id);
        }
    }
}
