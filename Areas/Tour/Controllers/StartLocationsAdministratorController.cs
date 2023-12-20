using HKQTravellingAuthenication.Areas.Tour.Extension;
using HKQTravellingAuthenication.Data;
using HKQTravellingAuthenication.Models.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HKQTravellingAuthenication.Areas.Tour.Controllers
{
    [Area("Tour")]
    [Route("start-location-manager")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class StartLocationsAdministratorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StartLocationsAdministratorController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region CRUD Start Location Administrator
        // GET: Admin/StartLocationsAdministrator
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index(string sortedOrder)
        {
            return _context.startLocations != null ?
                        View(await _context.startLocations.ToListAsync()) :
                        Problem("Entity set 'ApplicationDBContext.startLocations'  is null.");
        }

        // GET: Admin/StartLocationsAdministrator/Create
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/StartLocationsAdministrator/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Route("create")]
        public async Task<IActionResult> Create(IFormCollection formColection)
        {
            string startLocation = formColection["StartLocationName"].ToString();
            if (startLocation == null)
            {
                ViewData["validation_message"] = "Điểm khởi hành không được để trống!";
                return View();
            }
            else if (checkingStartLocation.checkStartLocationName(_context, startLocation))
            {
                ViewData["validation_message"] = "Điểm khởi hành đã tồn tại!";
                return View();
            }
            else
            {
                var dbStartLocation = new StartLocations()
                {
                    StartLocationName = startLocation,
                };
                _context.Add(dbStartLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        // GET: Admin/StartLocationsAdministrator/Edit/5
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.startLocations == null)
            {
                return NotFound();
            }

            var startLocations = await _context.startLocations.FindAsync(id);
            if (startLocations == null)
            {
                return NotFound();
            }
            return View(startLocations);
        }

        // POST: Admin/StartLocationsAdministrator/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(long id, IFormCollection formCollection)
        {
            string startLocation = formCollection["StartLocationName"].ToString();
            var dbStartLocation = await _context.startLocations.FindAsync(id);
            try
            {
                if (startLocation == null)
                {
                    ViewData["validation_message"] = "Điểm khởi hành không được để trống!";
                    return View();
                }
                else if (checkingStartLocation.checkStartLocationNameWhenUpdate(_context, startLocation))
                {
                    ViewData["validation_message"] = "Điểm khởi hành đã tồn tại!";
                    return View();
                }
                else
                {
                    dbStartLocation.StartLocationName = startLocation;
                    _context.Update(dbStartLocation);
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

        // GET: Admin/StartLocationsAdministrator/Delete/5
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.startLocations == null)
            {
                return NotFound();
            }

            var startLocations = await _context.startLocations
                .FirstOrDefaultAsync(m => m.StartLocationId == id);
            if (startLocations == null)
            {
                return NotFound();
            }

            return View(startLocations);
        }

        // POST: Admin/StartLocationsAdministrator/Delete/5
        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.startLocations == null)
            {
                return Problem("Entity set 'ApplicationDBContext.startLocations'  is null.");
            }
            var startLocations = await _context.startLocations.FindAsync(id);
            if (startLocations != null)
            {
                var toursToUpdate = await _context.tours.Where(t => t.TourId == id).ToListAsync();
                foreach(var tour in toursToUpdate)
                {
                    tour.StartLocationId = null;
                }
                _context.startLocations.Remove(startLocations);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StartLocationsExists(long id)
        {
            return (_context.startLocations?.Any(e => e.StartLocationId == id)).GetValueOrDefault();
        }
        #endregion
    }
}