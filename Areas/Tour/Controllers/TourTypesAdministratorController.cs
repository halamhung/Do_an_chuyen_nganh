using HKQTravellingAuthenication.Areas.Tour.Extension;
using HKQTravellingAuthenication.Data;
using HKQTravellingAuthenication.Models.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HKQTravellingAuthenication.Areas.Tour.Controllers
{
    [Area("Tour")]
    [Route("tour-type-manager")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class TourTypesAdministratorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TourTypesAdministratorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/TourTypesAdministrator
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            return _context.tourTypes != null ?
                        View(await _context.tourTypes.ToListAsync()) :
                        Problem("Entity set 'ApplicationDBContext.tourTypes'  is null.");
        }

        // GET: Admin/TourTypesAdministrator/Create
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TourTypesAdministrator/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create(IFormCollection formColection)
        {
            string tourTypeName = formColection["TourTypeName"].ToString();
            if (tourTypeName == null)
            {
                ViewData["validation_message"] = "Tên loại tour không được để trống!";
                return View();
            }
            else if (checkingTourTypes.checkTourTypeName(_context, tourTypeName))
            {
                ViewData["validation_message"] = "Tên loại tour này đã tồn tại!";
                return View();
            }
            else
            {
                var dbTourTypes = new TourTypes()
                {
                    TourTypeName = tourTypeName,
                };
                _context.Add(dbTourTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/TourTypesAdministrator/Edit/5
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.tourTypes == null)
            {
                return NotFound();
            }

            var TourTypes = await _context.tourTypes.FindAsync(id);
            if (TourTypes == null)
            {
                return NotFound();
            }
            return View(TourTypes);
        }

        // POST: Admin/TourTypesAdministrator/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(long id, IFormCollection formCollection)
        {
            string tourTypeName = formCollection["TourTypeName"].ToString();
            var dbTourTypes = await _context.tourTypes.FindAsync(id);
            if(dbTourTypes == null)
            {
                return NotFound();
            }
            try
            {
                if (tourTypeName == null)
                {
                    ViewData["validation_message"] = "Điểm khởi hành không được để trống!";
                    return View();
                }
                else if (checkingTourTypes.checkTourTypeNameWhenUpdate(_context, tourTypeName))
                {
                    ViewData["validation_message"] = "Điểm khởi hành đã tồn tại!";
                    return View();
                }
                else
                {
                    dbTourTypes.TourTypeName = tourTypeName;
                    _context.Update(dbTourTypes);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                //Điều hướng đến trang lỗi
                return View();
            }
        }

        // GET: Admin/TourTypesAdministrator/Delete/5
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.tourTypes == null)
            {
                return NotFound();
            }

            var TourTypes = await _context.tourTypes
                .FirstOrDefaultAsync(m => m.TourTypeId == id);
            if (TourTypes == null)
            {
                return NotFound();
            }

            return View(TourTypes);
        }

        // POST: Admin/TourTypesAdministrator/Delete/5
        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.tourTypes == null)
            {
                return Problem("Entity set 'ApplicationDBContext.tourTypes'  is null.");
            }
            var TourTypes = await _context.tourTypes.FindAsync(id);
            if (TourTypes != null)
            {
                var toursToUpdate = await _context.tours.Where(t => t.TourId == id).ToListAsync();
                foreach (var tour in toursToUpdate)
                {
                    tour.TourTypeId = null;
                }
                _context.tourTypes.Remove(TourTypes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TourTypesExists(long id)
        {
            return (_context.tourTypes?.Any(e => e.TourTypeId == id)).GetValueOrDefault();
        }
    }

}
