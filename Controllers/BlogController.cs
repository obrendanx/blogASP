using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BlogController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult ShowPosts(string filter, string forumPageFilter = "default")
        {
            IEnumerable<BlogPost> objBlogPosts = _db.BlogPost;

            /*if (!string.IsNullOrEmpty(filter))
            {
                DateTime cutoffDate;

                switch (filter)
                {
                    case "last24hours":
                        cutoffDate = DateTime.Now.AddDays(-1);
                        objBlogPosts = objBlogPosts.Where(p => p.CreatedDateTime >= cutoffDate);
                        break;

                    case "last7days":
                        cutoffDate = DateTime.Now.AddDays(-7);
                        objBlogPosts = objBlogPosts.Where(p => p.CreatedDateTime >= cutoffDate);
                        break;

                    default:
                        break;
                }
            }*/

            using (_db)
            {
                var filterParam = new SqlParameter("@FilterType", SqlDbType.NVarChar)
                {
                    Value = filter ?? "all"
                };

                var forumPageParam = new SqlParameter("@ForumPage", SqlDbType.NVarChar)
                {
                    Value = forumPageFilter 
                };

                objBlogPosts = _db.BlogPost
                    .FromSqlRaw("EXEC GetFilteredBlogPosts @FilterType, @ForumPage", filterParam, forumPageParam)
                    .ToList();
            }

            return View(objBlogPosts);
        }

        public IActionResult GamingPosts(string filter, string forumPageFilter = "gaming")
        {
            IEnumerable<BlogPost> objBlogPosts = _db.BlogPost;

            using (_db)
            {
                var filterParam = new SqlParameter("@FilterType", SqlDbType.NVarChar)
                {
                    Value = filter ?? "all"
                };

                var forumPageParam = new SqlParameter("@Page", SqlDbType.NVarChar)
                {
                    Value = forumPageFilter
                };

                objBlogPosts = _db.BlogPost
                    .FromSqlRaw("EXEC GetFilteredBlogPosts @FilterType, @Page", filterParam, forumPageParam)
                    .ToList();
            }

            return View(objBlogPosts);
        }

        //GET
        public IActionResult CreatePost()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(BlogPost obj)
        {
            if(ModelState.IsValid)
            {
                _db.BlogPost.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("ShowPosts");
            }
            return View(obj);
        }
    }
}
