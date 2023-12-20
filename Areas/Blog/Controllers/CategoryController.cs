using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HKQTravellingAuthenication.Models;
using HKQTravellingAuthenication.Models.Blog;
using Microsoft.AspNetCore.Authorization;
using HKQTravellingAuthenication.Data;

namespace HKQTravellingAuthenication.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("admin/blog/category/[action]/{id?}")]
    [Authorize(Roles = RoleName.Administrator)]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Blog/Category
        public async Task<IActionResult> Index()
        {

            var qr = (from c in _context.Categories select c)
                    .Include(c => c.ParentCategory)                // load parent category
                    .Include(c => c.CategoryChildren);             // load child category

            var Categories = (await qr.ToListAsync())
                         .Where(c => c.ParentCategory == null)
                         .ToList();

            return View(Categories);
        }
        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //PHƯƠNG THỨC CREATE
         private void CreateSelectItems(List<Category> source, List<Category> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("----", level));
             foreach (var category in source)
             {
                //   category.Title = prefix + " " + category.Title;
                    des.Add(new Category(){
                     Id = category.Id,
                     Title = prefix + " " + category.Title
                    });
                   if (category.CategoryChildren?.Count > 0)
                   {
                       CreateSelectItems(category.CategoryChildren.ToList(), des, level +1);
                   }
            }
        }
      
        // GET: Blog/Category/Create
        public async Task<IActionResult> Create()
        {
            var qr = (from c in _context.Categories select c)
                     .Include(c => c.ParentCategory)
                     .Include(c => c.CategoryChildren);

            var Categories = (await qr.ToListAsync())
                             .Where(c => c.ParentCategory == null)
                             .ToList();
            Categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });
            var items = new List<Category>();
            var selectList = new SelectList(items, "Id", "Title");
            ViewData["ParentCategoryId"] = selectList;

            // Categories.Insert(0, new Category(){
            //     Id = -1,
            //     Title = "Không có danh mục cha"
            // });
            // var items = new List<Category>();
             CreateSelectItems(Categories, items, 0); 

            return View();
        }

        // POST: Blog/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,Slug,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentCategoryId == -1) category.ParentCategoryId = null;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var errors = entry.Value.Errors;
                    foreach (var error in errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        // Thực hiện các hành động xử lý lỗi, hoặc ghi log lỗi
                    }       
                }
            }

            var qr = (from c in _context.Categories select c)
                       .Include(c => c.ParentCategory)
                       .Include(c => c.CategoryChildren);

            var Categories = (await qr.ToListAsync())
                             .Where(c => c.ParentCategory == null)
                             .ToList();
            Categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });
            var items = new List<Category>();
            CreateSelectItems(Categories, items, 0);
            var selectList = new SelectList(items, "Id", "Title");
            ViewData["ParentCategoryId"] = selectList;
            return View(category);
        }


        //PHƯƠNG THỨC EDIT
        // GET: Blog/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

             var qr = (from c in _context.Categories select c)
                       .Include(c => c.ParentCategory)
                       .Include(c => c.CategoryChildren);

            var Categories = (await qr.ToListAsync())
                             .Where(c => c.ParentCategory == null)
                             .ToList();
            Categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });
            var items = new List<Category>();
            CreateSelectItems(Categories, items, 0);
            var selectList = new SelectList(items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;
            return View(category);
        }


        // POST: Blog/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Slug,ParentCategoryId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            bool canUpdate = true;

            if (category.ParentCategoryId == category.Id)
            {
                ModelState.AddModelError(string.Empty, "Phải chọn danh mục cha khác");
                canUpdate = false;
            }

            // Kiem tra thiet lap muc cha phu hop
            if (canUpdate && category.ParentCategoryId != null)
            {
                var childCates =
                            (from c in _context.Categories select c)
                            .Include(c => c.CategoryChildren)
                            .ToList()
                            .Where(c => c.ParentCategoryId == category.Id);


                // Func check Id 
                Func<List<Category>, bool> checkCateIds = null; 
                checkCateIds = (cates) =>
                    {
                        foreach (var cate in cates)
                        {
                            Console.WriteLine(cate.Title);
                            if (cate.Id == category.ParentCategoryId)
                            {
                                canUpdate = false;
                                ModelState.AddModelError(string.Empty, "Phải chọn danh mục cha khác (khác với cha của con hiện tại)");
                                return true;
                            }
                            if (cate.CategoryChildren != null)
                                return checkCateIds(cate.CategoryChildren.ToList());

                        }
                        return false;
                    };
                // End Func 
                checkCateIds(childCates.ToList());
            }




            if (ModelState.IsValid && canUpdate)
            {
                try
                {
                    if (category.ParentCategoryId == -1)
                        category.ParentCategoryId = null;

                    var dtc = _context.Categories.FirstOrDefault(c => c.Id == id);
                    _context.Entry(dtc).State = EntityState.Detached;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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

             var qr = (from c in _context.Categories select c)
                       .Include(c => c.ParentCategory)
                       .Include(c => c.CategoryChildren);

            var Categories = (await qr.ToListAsync())
                             .Where(c => c.ParentCategory == null)
                             .ToList();
            Categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });
            var items = new List<Category>();
            CreateSelectItems(Categories, items, 0);
            var selectList = new SelectList(items, "Id", "Title");
            ViewData["ParentCategoryId"] = selectList;


            return View(category);
        }


        //PHƯƠNG THỨC DELETE
        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

    }
}