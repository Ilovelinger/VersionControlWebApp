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

namespace WebApplication8.Controllers
{
    /// <summary>
    /// This is the post controller of the application,for managing add,delete posts,add comments with authorizations.
    /// </summary>
    public class PostController : Controller
    {
        //Add a reference to the database I created
        private readonly ApplicationDbContext db;

        public PostController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View("PostPage");
        }


        [HttpGet]
        public IActionResult PostList()
        {
            PostListViewModel postlistVM = new PostListViewModel();
            //Add all the posts from the database to a list.
            postlistVM.Posts = db.Posts.ToList<Post>();
            //Count the number of posts.
            postlistVM.NumberOfPosts = postlistVM.Posts.Count;

            return View(postlistVM);
        }

        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostListAsync(Post post)
        {
            //Add posts to the database.
            db.Posts.Add(post);
            await db.SaveChangesAsync();

            return RedirectToAction("PostList");
        }

        /// <summary>
        /// Takes an id as input, check if the id and related post exist.
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
            Post post = await db.Posts
                .SingleOrDefaultAsync(m => m.postid == id);
            if (post == null)
            {
                return NotFound();
            }

            PostDetailViewModel viewModel = await GetPostDetailViewModelFromPost(post);

            return View(viewModel);

        }

        /// <summary>
        /// Add comments to their related posts,only customer can do this.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddComments([Bind("PostID,CommentsContent")]PostDetailViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Comment comment = new Comment();

                comment.commentscontent = viewModel.CommentsContent;

                //Database query.
                Post post = await db.Posts
                .SingleOrDefaultAsync(m => m.postid == viewModel.PostID);

                if (post == null)
                {
                    return NotFound();
                }

                comment.RelatedPost = post;
                //Add the comments to the database.
                db.Comments.Add(comment);
                await db.SaveChangesAsync();

                viewModel = await GetPostDetailViewModelFromPost(post);

            }
            return View(viewModel);
        }

        /// <summary>
        /// Take comments from the databse and add them to a list.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private async Task<PostDetailViewModel> GetPostDetailViewModelFromPost(Post post)
        {
            PostDetailViewModel viewModel = new PostDetailViewModel();

            viewModel.Post = post;

            List<Comment> comments = await db.Comments
                .Where(m => m.RelatedPost == post).ToListAsync();

            viewModel.Comments = comments;
            return viewModel;

        }

        /// <summary>
        /// Take an id as input,check if the id and its related post exist.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await db.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        /// <summary>
        /// Edit the designated post,update the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost,ActionName("EditPost")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, [Bind("postid,title,author,content")] Post post)
        {
            if (id != post.postid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(post);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.postid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(PostList));
            }
            return View(post);
        }

        /// <summary>
        /// Takes an id of the post that need to be deleted as input, check if the id and related post exist。
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

            var post = await db.Posts
                .FirstOrDefaultAsync(m => m.postid == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        /// <summary>
        /// Delete the designated post from database,only admin can do this.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await db.Posts.FindAsync(id);
            db.Posts.Remove(post);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(PostList));
        }

        private bool PostExists(int id)
        {
            return db.Posts.Any(e => e.postid == id);
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
            return RedirectToAction(nameof(PostList));
        }

        private bool CommentExists(int id)
        {
            return db.Comments.Any(e => e.commentid == id);
        }

    }
}