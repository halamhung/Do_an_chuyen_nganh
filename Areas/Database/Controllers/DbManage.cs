using Bogus;
using HKQTravellingAuthenication.Data;
using HKQTravellingAuthenication.Models;
using HKQTravellingAuthenication.Models.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace HKQTravellingAuthenication.Areas.Database.Controllers
{
    [Area("Database")]
    //[Authorize(Roles = RoleName.Administrator)]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbManageController(ApplicationDbContext dbcontext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeleteDb()
        {
            return View();
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        public async Task<IActionResult> DeleteDbAsync()
        {
            var success = await _dbcontext.Database.EnsureDeletedAsync();
            StatusMessage = success ? "Xóa Database thành công" : "Không xóa được Db";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Migrate()
        {
            await _dbcontext.Database.MigrateAsync();
            StatusMessage = "Cập nhật Database thành công";
            return RedirectToAction(nameof(Index));
        }

        //https://localhost:5126/database-manage/SeedData
        public async Task<IActionResult> SeedDataAsync()
        {
            //Create Roles
            var rolenames = typeof(RoleName).GetFields().ToList();
            foreach (var r in rolenames)
            {
                var rolename = (string)r.GetRawConstantValue();
                var rfound = await _roleManager.FindByNameAsync(rolename);
                if (rfound == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(rolename));
                }
            }

            //admin, pass=admin123, admin@example.com
            var useradmin = await _userManager.FindByEmailAsync("hkqtravelling@gmail.com");
            if (useradmin == null)
            {
                useradmin = new AppUser()
                {
                    UserName = "admin",
                    Email = "hkqtravelling@gmail.com",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(useradmin, "admin123");
                await _userManager.AddToRoleAsync(useradmin, RoleName.Administrator);
            }
            StatusMessage = "Vừa Seed DB";
            return RedirectToAction("Index");
        }
        //tao ra 1 số bài viết để test 
        //https://localhost:5126/database-manage/SeedPostCategory
        public async Task<IActionResult> SeedPostCategoryAsync()
        {
             _dbcontext.Categories.RemoveRange(_dbcontext.Categories.Where(c => c.Content.Contains("[fakeData]")));
            // _dbcontext.Posts.RemoveRange(_dbcontext.Posts.Where(p => p.Content.Contains("[fakeData]")));

            //  _dbcontext.SaveChanges();
            Randomizer.Seed = new Random(8675309);
             
             var fakerCategory = new Faker<Category>();
             int cm = 1;
             fakerCategory.RuleFor( c => c.Title,  fk => $"CM{cm++} " + fk.Lorem.Sentence(1,2).Trim('.'));
             fakerCategory.RuleFor( c => c.Content,  fk => fk.Lorem.Sentences(5) + "[fakeData]");
             fakerCategory.RuleFor( c => c.Slug,  fk => fk.Lorem.Slug());



            var cate1 = fakerCategory.Generate();
            var cate11 = fakerCategory.Generate();
            var cate12 = fakerCategory.Generate();
            var cate2 = fakerCategory.Generate();
            var cate21 = fakerCategory.Generate();
            var cate211 = fakerCategory.Generate();


             cate11.ParentCategory  = cate1;
             cate12.ParentCategory  = cate1;
             cate21.ParentCategory = cate2;
             cate211.ParentCategory = cate21;

             var categories = new Category[] { cate1, cate2, cate12, cate11, cate21, cate211};
            _dbcontext.Categories.AddRange(categories);
            
            // POST
            var rCateIndex = new Random();
            int bv = 1;

            var user = _userManager.GetUserAsync(this.User).Result;
            var fakerPost = new Faker<Post>();
            fakerPost.RuleFor(p => p.AuthorId, f => user.Id);
            fakerPost.RuleFor(p => p.Content, f => f.Lorem.Paragraphs(7)+"[fakeData]");
            fakerPost.RuleFor(p => p.DateCreated, f => f.Date.Between(new DateTime(2021,1,1), new DateTime(2021,7,1)));
            fakerPost.RuleFor(p => p.Description, f => f.Lorem.Sentences(3));
            fakerPost.RuleFor(p => p.Published, f => true);
            fakerPost.RuleFor(p => p.Slug, f => f.Lorem.Slug());
            fakerPost.RuleFor(p => p.Title, f => $"Bài {bv++} " + f.Lorem.Sentence(3,4).Trim('.')); 

            List<Post> posts = new List<Post>();
            List<PostCategory> post_categories = new List<PostCategory>();


            for (int i = 0; i < 40; i++)
            {
                var post = fakerPost.Generate();
                post.DateUpdated = post.DateCreated;
                posts.Add(post);
                post_categories.Add(new PostCategory() { 
                    Post = post,
                    Category = categories[rCateIndex.Next(5)]
                });
            } 

            _dbcontext.AddRange(posts);
            _dbcontext.AddRange(post_categories); 
            // END POST
            _dbcontext.SaveChanges();
            StatusMessage = "Vừa tạo ra fake data";
            return RedirectToAction("Index");
        }
    }

}
