using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication8.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using WebApplication8.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication8.Controllers
{
    /// <summary>
    /// This is the match controller of the application,for managing add,delete posts,add comments with authorizations.
    /// </summary>
    public class MatchController : Controller
    {
        //Add a reference to the database
        private readonly ApplicationDbContext db;

        public MatchController(ApplicationDbContext _db)
        {
            db = _db;
        }

        /// <summary>
        /// HttpGet method for create match result.
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult MatchPage()
        {

            ViewBag.drolistmenu1 = db.Team.Select(g => new SelectListItem
            {
                Text = g.teamName,
                Value = g.teamName,
                Selected = false
            });

            ViewBag.drolistmenu2 = db.Team.Select(g => new SelectListItem
            {
                Text = g.teamName,
                Value = g.teamName,
                Selected = false
            });

            return View("MatchPage");
        }

        /// <summary>
        /// HttpGet method for create new match.
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddNewMatch()
        {
            return View("AddNewMatch");
        }

        /// <summary>
        /// HttpGet method for viewing new match list.
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public IActionResult NewMatchList()
        {
            NewMatchListViewModel newmatchlistVM = new NewMatchListViewModel();
            //Add all the posts from the database to a list.
            newmatchlistVM.NewMatches = db.NewMatches.ToList<NewMatch>();

            return View(newmatchlistVM);
        }

        /// <summary>
        /// HttpPost method for create new match.
        /// </summary>
        /// <param name="newmatch"></param>
        /// <returns>View</returns>
        [HttpPost, ActionName("CreateNewMatch")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewMatchListAsync(NewMatch newmatch)
        {
            db.NewMatches.Add(newmatch);
            await db.SaveChangesAsync();

            return RedirectToAction("NewMatchList");
        }

        /// <summary>
        /// HttpGet method for viewing match list.
        /// </summary>
        /// <returns>View and viewmodel</returns>
        [HttpGet]
        public IActionResult MatchList()
        {

            MatchListViewModel matchlistVM = new MatchListViewModel();

            matchlistVM.Matches = db.Matches.ToList<Match>();

            matchlistVM.NumberOfMatches = matchlistVM.Matches.Count;

            return View(matchlistVM);
        }

        /// <summary>
        /// HttpPost method for creating match result.
        /// </summary>
        /// <param name="match"></param>
        /// <returns>Redirect to action</returns>
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MatchListAsync(Match match)
        {
            var selectedTeam1 = match.team1Name;
            var selectedTeam2 = match.team2Name;
            Team team1 = await db.Team
                .SingleOrDefaultAsync(m => m.teamName == selectedTeam1);
            match.RelatedTeam1 = team1;

            Team team2 = await db.Team
                .SingleOrDefaultAsync(m => m.teamName == selectedTeam2);
            match.RelatedTeam2 = team2;

            db.Matches.Add(match);
            await db.SaveChangesAsync();

            return RedirectToAction("MatchList");
        }

        /// <summary>
        /// HttpGet method for viewing new match detail.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View and viewmodel</returns>
        public async Task<IActionResult> NewMatchDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Database query.
            NewMatch newmatch = await db.NewMatches
                .SingleOrDefaultAsync(m => m.newMatchId == id);
            if (newmatch == null)
            {
                return NotFound();
            }

            NewMatchDetailViewModel viewModel = await GetNewMatchDetailViewModelFromNewMatch(newmatch);

            return View(viewModel);

        }

        /// <summary>
        /// HttpPost method for viewing new match detail.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>View and view model</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Player,CommonUser")]
        public async Task<IActionResult> NewMatchDetail(NewMatchDetailViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                //Database query.
                NewMatch newmatch = await db.NewMatches
                .SingleOrDefaultAsync(m => m.newMatchId == viewModel.NewMatchID);

                if (newmatch == null)
                {
                    return NotFound();
                }


                viewModel = await GetNewMatchDetailViewModelFromNewMatch(newmatch);

            }
            return View(viewModel);
        }

        /// <summary>
        /// Set new match object for viewmodel.
        /// </summary>
        /// <param name="newmatch"></param>
        /// <returns>viewModel</returns>
        private async Task<NewMatchDetailViewModel> GetNewMatchDetailViewModelFromNewMatch(NewMatch newmatch)
        {
            NewMatchDetailViewModel viewModel = new NewMatchDetailViewModel();

            viewModel.NewMatch = newmatch;

            return viewModel;

        }

        /// <summary>
        /// HttpGet method for viewing match detail and adding comments.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View and viewmodel</returns>
        public async Task<IActionResult> AddComments(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            //Database query.
            Match match = await db.Matches
                .SingleOrDefaultAsync(m => m.matchid == id);
            if (match == null)
            {
                return NotFound();
            }


            MatchDetailViewModel viewModel = await GetMatchDetailViewModelFromMatch(match);

            return View(viewModel);

        }

        /// <summary>
        /// HttpPost method for adding comments.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>View and viewmodel</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Player,CommonUser")]
        public async Task<IActionResult> AddComments([Bind("MatchID,CommentsContent")]MatchDetailViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                Comment comment = new Comment();
                comment.commentscontent = viewModel.CommentsContent;

                comment.commentUsername = HttpContext.Session.GetString("tempUserName");

                string tempuserid = HttpContext.Session.GetString("tempUserId");

                //ViewData["Userid"] = tempuserid;

                ApplicationUser user = await db.Users.SingleOrDefaultAsync(m => m.Id == tempuserid);


                //Database query.
                Match match = await db.Matches
                .SingleOrDefaultAsync(m => m.matchid == viewModel.MatchID);

                if (match == null)
                {
                    return NotFound();
                }

                comment.RelatedMatch = match;
                comment.RelatedUser = user;
                //Add the comments to the database.
                db.Comments.Add(comment);
                await db.SaveChangesAsync();

                viewModel = await GetMatchDetailViewModelFromMatch(match);

            }
            return View(viewModel);
        }

        /// <summary>
        /// Set the comment list for viewmodel.
        /// </summary>
        /// <param name="match"></param>
        /// <returns>viewModel</returns>
        private async Task<MatchDetailViewModel> GetMatchDetailViewModelFromMatch(Match match)
        {
            MatchDetailViewModel viewModel = new MatchDetailViewModel();

            viewModel.Match = match;

            List<Comment> comments = await db.Comments.Include(u => u.RelatedUser)
                .Where(m => m.RelatedMatch == match).ToListAsync();

            viewModel.Comments = comments;

            return viewModel;

        }

        /// <summary>
        /// HttpGet method for editing match result.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View and match</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditMatch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await db.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }
            return View(match);
        }

        /// <summary>
        /// HttpPost method,edit the designated match,update the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns>View and match</returns>
        [HttpPost, ActionName("EditMatch")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMatch(int id, Match match)
        {
            if (id != match.matchid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(match);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.matchid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MatchList));
            }
            return View(match);
        }

        /// <summary>
        /// HttpGet method for deleteing match result.
        /// </summary>
        /// <param name="id"></param> 
        /// <returns>View and match</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await db.Matches
                .FirstOrDefaultAsync(m => m.matchid == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        /// <summary>
        /// HttpPost method for deleting match result.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirect to action</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await db.Matches.FindAsync(id);
            db.Matches.Remove(match);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(MatchList));
        }

        /// <summary>
        /// Check if the match exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>match</returns>
        private bool MatchExists(int id)
        {
            return db.Matches.Any(e => e.matchid == id);
        }

        /// <summary>
        /// HttpGet method for deleting comment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View and comment</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await db.Comments
                .FirstOrDefaultAsync(m => m.commentid == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        /// <summary>
        /// HttpPost method for deleting comment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Redirect to action</returns>
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCommentConfirmed(int id)
        {
            var comment = await db.Comments.FindAsync(id);
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(MatchList));
        }

        /// <summary>
        /// Check if the comment exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Comment</returns>
        private bool CommentExists(int id)
        {
            return db.Comments.Any(e => e.commentid == id);
        }

        /// <summary>
        /// HttpGet method for viewing user profile.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View</returns>
        public async Task<IActionResult> ViewUserProfile(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser User = await db.Users.Include(u => u.RelatedTeam)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (User == null)
            {
                return NotFound();
            }

            Team Team = User.RelatedTeam;

            var allcomments = db.Comments;
            var comments = allcomments.Include(u => u.RelatedMatch).Where(x => x.RelatedUser == User).ToList();

            var allplayerperformance = db.PlayerPerformance;
            var playerperformance = allplayerperformance.Include(u => u.RelatedMatch).Where(x => x.RelatedUser == User).ToList();

            ViewBag.comments = comments;
            ViewBag.playerperformance = playerperformance;

            ViewBag.Username = User.FullName;
            ViewBag.Email = User.Email;
            ViewBag.MobilePhoneNumber = User.MobilePhoneNumber;

            if (User.Position != null)
            { ViewBag.Position = User.Position; }
            else
            {
                ViewBag.Position = "Not a player";
            }

            ViewBag.KitNumber = User.KitNumber;

            if (User.Position != null)
            { ViewBag.Team = Team.teamName; ViewBag.TeamId = Team.teamId; ViewBag.IsRegistered = "Yes"; }

            else
            {
                ViewBag.Team = "Haven't register to a team";
                ViewBag.IsRegistered = "No";
            }

            if (User.AvatarImage != null)
            {
                string temp_inBase64 = Convert.ToBase64String(User.AvatarImage);
                ViewData["MyPic"] = String.Format("data:image/jpeg;base64,{0}", temp_inBase64);
            }

            return View();

        }
    }
}