using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using HKQTravellingAuthenication.Data;
using Microsoft.Extensions.Logging;
using HKQTravellingAuthenication.Models.Blog;
using Microsoft.AspNetCore.Mvc;
using HKQTravellingAuthenication.Models;

namespace HKQTravellingAuthenication.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class ViewPostController : Controller
    {
        private readonly ILogger<ViewPostController> logger;
        private readonly ApplicationDbContext _context;

        public ViewPostController(ILogger<ViewPostController> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            _context = context;
        }
        // /Post/
        // /Post/{Ctegoryslug?}
        // Dùng Slug đã được tạo để xác định hiển thị chuyên mục nào 
        [Route("/post/{categoryslug?}")]
        public IActionResult Index(string categoryslug, [FromQuery(Name = "p")] int currentPage, int pagesize) //p=1 là trang 1 ,vv
        {
            var categories = GetCategory();
            ViewBag.categories = categories;
            ViewBag.categoryslug = categoryslug;
            Category category = null;
            if (!string.IsNullOrEmpty(categoryslug))
            {
                category = _context.Categories.Where(c => c.Slug == categoryslug)
                             .Include(c => c.CategoryChildren)
                             .FirstOrDefault();

                if (category == null)
                {
                    return NotFound("Không thấy category");
                }
            }

            var posts = _context.Post
                            .Include(p => p.Author)
                            .Include(p => p.PostCategories)
                            .ThenInclude(p => p.Category)
                            .AsQueryable();
            posts.OrderByDescending(p => p.DateUpdated);
            
            //nếu như category = null nó sẽ lấy ra tất cả các chuyên mục còn nếu có thì nó sẽ lấy ra bài viết thuộc chuyên mục đó 
            if (category != null)
            {
                var ids = new List<int>();
                category.ChildCategoryIDs(null, ids);
                ids.Add(category.Id);
                posts = posts.Where(p => p.PostCategories.Where(pc => ids.Contains(pc.CategoryID)).Any());
            }
            //đoạn code phân trang 
            int totalPosts = posts.Count();
            if (pagesize <= 0) pagesize = 6; //số lượng trong 1 trang
            int countPages = (int)Math.Ceiling((double)totalPosts / pagesize);

            if (currentPage > countPages) currentPage = countPages;
            if (currentPage < 1) currentPage = 1;

            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new
                {
                    p = pageNumber,
                    pagesize = pagesize
                })
            };
            //
            //lấy ra số lượng bài của 1 trang
            var postsInPage = posts.Skip((currentPage - 1) * pagesize)
                             .Take(pagesize);
            //
            
            ViewBag.pagingModel = pagingModel;
            ViewBag.totalPosts = totalPosts;
            ViewBag.category = category;
            return View(postsInPage.ToList());
        }

       [Route("/post/{postslug}.html")]
        public IActionResult Detail(string postslug)
        {
            var categories = GetCategory();
            ViewBag.categories = categories;

            var post = _context.Post.Where(p => p.Slug == postslug)
                               .Include(p => p.Author)
                               .Include(p => p.PostCategories)
                               .ThenInclude(pc => pc.Category)
                               .FirstOrDefault();

            if (post == null)
            {
                return NotFound("Không thấy bài viết");
            }            

            Category category = post.PostCategories.FirstOrDefault()?.Category;
            ViewBag.category = category;

            var otherPosts = _context.Post.Where(p => p.PostCategories.Any(c => c.Category.Id == category.Id))
                                            .Where(p => p.PostId != post.PostId)
                                            .OrderByDescending(p => p.DateUpdated)
                                            .Take(5);
            ViewBag.otherPosts = otherPosts;                                

            return View(post);
        }
        private List<Category> GetCategory()
        {
            var categories = _context.Categories
                            .Include(c => c.CategoryChildren)
                            .AsEnumerable()
                            .Where(c => c.ParentCategoryId == null)
                            .ToList();
            return categories;
        }
    }
}
