﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication8.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using WebApplication8.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication8.Controllers
{
    /// <summary>
    /// This is the match controller of the application,for managing add,delete posts,add comments with authorizations.
    /// </summary>
    public class MatchController : Controller
    {
        //Add a reference to the database I created
        private readonly ApplicationDbContext db;

        public MatchController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View("MatchPage");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddNewMatch()
        {
            return View("AddNewMatch");
        }

        [HttpGet]
        public IActionResult NewMatchList()
        {
            NewMatchListViewModel newmatchlistVM = new NewMatchListViewModel();
            //Add all the posts from the database to a list.
            newmatchlistVM.NewMatches = db.NewMatches.ToList<NewMatch>();

            return View(newmatchlistVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewMatchListAsync(NewMatch newmatch)
        {
            //Add posts to the database.
            db.NewMatches.Add(newmatch);
            await db.SaveChangesAsync();

            return RedirectToAction("NewMatchList");
        }

        [HttpGet]
        public IActionResult MatchList()
        {
            MatchListViewModel matchlistVM = new MatchListViewModel();
            //Add all the posts from the database to a list.
            matchlistVM.Matches = db.Matches.ToList<Match>();
            //Count the number of posts.
            matchlistVM.NumberOfMatches = matchlistVM.Matches.Count;

            return View(matchlistVM);
        }

        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MatchListAsync(Match match)
        {
            //Add posts to the database.
            db.Matches.Add(match);
            await db.SaveChangesAsync();

            return RedirectToAction("MatchList");
        }

        /// <summary>
        /// Takes an id as input, check if the id and related match exist.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddComments(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //Database query.
            Match post = await db.Matches
                .SingleOrDefaultAsync(m => m.matchid == id);
            if (post == null)
            {
                return NotFound();
            }

            MatchDetailViewModel viewModel = await GetMatchDetailViewModelFromMatch(post);

            return View(viewModel);

        }

        /// <summary>
        /// Add comments to their related posts,only customer can do this.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Player,CommonUser")]
        public async Task<IActionResult> AddComments([Bind("MatchID,CommentsContent")]MatchDetailViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Comment comment = new Comment();

                comment.commentscontent = viewModel.CommentsContent;

                //Database query.
                Match match = await db.Matches
                .SingleOrDefaultAsync(m => m.matchid == viewModel.MatchID);

                if (match == null)
                {
                    return NotFound();
                }

                comment.RelatedMatch = match;
                //Add the comments to the database.
                db.Comments.Add(comment);
                await db.SaveChangesAsync();

                viewModel = await GetMatchDetailViewModelFromMatch(match);

            }
            return View(viewModel);
        }

        /// <summary>
        /// Take comments from the databse and add them to a list.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private async Task<MatchDetailViewModel> GetMatchDetailViewModelFromMatch(Match match)
        {
            MatchDetailViewModel viewModel = new MatchDetailViewModel();

            viewModel.Match = match;

            List<Comment> comments = await db.Comments
                .Where(m => m.RelatedMatch == match).ToListAsync();

            viewModel.Comments = comments;
            return viewModel;

        }

        /// <summary>
        /// Take an id as input,check if the id and its related match exist.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// Edit the designated match,update the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost,ActionName("EditMatch")]
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
        /// Takes an id of the match that need to be deleted as input, check if the id and related match exist。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// Delete the designated match from database,only admin can do this.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        private bool MatchExists(int id)
        {
            return db.Matches.Any(e => e.matchid == id);
        }

        /// <summary>
        /// Take the id of the comment as input,check if the id and its realated comment exist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// Delete the designated comment from the database,only admin can do this.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        private bool CommentExists(int id)
        {
            return db.Comments.Any(e => e.commentid == id);
        }

    }
}