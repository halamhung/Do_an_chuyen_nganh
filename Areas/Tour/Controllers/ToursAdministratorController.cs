using Bogus.DataSets;
using elFinder.NetCore.Helpers;
using HKQTravellingAuthenication.Areas.Tour.Extension;
using HKQTravellingAuthenication.Areas.Tour.Models;
using HKQTravellingAuthenication.Data;
using HKQTravellingAuthenication.Models.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities;
using System.Data;
namespace HKQTravellingAuthenication.Areas.Tour.Controllers
{
	[Area("Tour")]
	[Route("tour-manager")]
	[Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
	public class ToursAdministratorController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ToursAdministratorController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		[HttpGet]
		[Route("index")]
		public async Task<IActionResult> Index()
		{
			var dbTours = await _context.tours.Include(t => t.discounts)
				.Include(t => t.endLocations)
				.Include(t => t.startLocations)
				.Include(t => t.tourTypes).ToListAsync();
			return View(dbTours);
		}

		[HttpGet]
		[Route("details/{id}")]
		public async Task<IActionResult> Details(long? id)
		{
			if (id == null || _context.tours == null)
			{
				return NotFound();
			}

			var tours = await _context.tours.Include(t => t.discounts)
				.Include(t => t.startLocations)
				.Include(t => t.endLocations)
				.Include(t => t.tourTypes)
				.FirstOrDefaultAsync(t => t.TourId == id);
			var tourImages = await _context.tourImages.Where(t => t.TourId == id).ToListAsync();
			if (tours == null)
			{
				return NotFound();
			}
			else
			{
				var tourExtraViewModel = new TourExtraViewModel()
				{
					Tours = tours,
					TourImagesList = tourImages
				};
				return View(tourExtraViewModel);
			}
		}

		[HttpGet]
		[Route("create")]
		public IActionResult Create()
		{
			ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountName");
			ViewData["EndLocationId"] = new SelectList(_context.endLocations.OrderBy(c => c.EndLocationName), "EndLocationId", "EndLocationName");
			ViewData["StartLocationId"] = new SelectList(_context.startLocations.OrderBy(c => c.StartLocationName), "StartLocationId", "StartLocationName");
			ViewData["TourTypeId"] = new SelectList(_context.tourTypes.OrderBy(c => c.TourTypeName), "TourTypeId", "TourTypeName");
			return View();
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> Create(IFormCollection collection)
		{
			string tourName = collection["TourName"].ToString();
			string price = collection["Price"].ToString().Trim();
			string userInputStartDate = collection["StartDate"].ToString();
			string userInputEndDate = collection["EndDate"].ToString();
			string discountId = collection["DiscountId"].ToString();
			string startLocationId = collection["StartLocationId"].ToString();
			string endLocationId = collection["EndLocationId"].ToString();
			string tourTypeId = collection["TourTypeId"].ToString();
			string remaining = collection["Remaining"].ToString().Trim();
			string priceInclude = collection["PriceInclude"].ToString();
			string priceNotInclude = collection["PriceNotInclude"].ToString();
			string surcharge = collection["Surcharge"].ToString();
			string cancelChange = collection["CancelChange"].ToString();
			string note = collection["Note"].ToString();
			string description = collection["Description"].ToString();

			//Chuyển đổi thành DateTime hoàn toàn
			DateTime? startDate = !string.IsNullOrWhiteSpace(userInputStartDate) ?
					  Convert.ToDateTime(userInputStartDate) : (DateTime?)null;
			DateTime? endDate = !string.IsNullOrWhiteSpace(userInputEndDate) ?
					Convert.ToDateTime(userInputEndDate) : (DateTime?)null;

			var tours = new Tours();
			#region kiểm tra đầu vào
			if (tourName.IsNullOrEmpty())
			{
				ViewData["validation_message_tourName"] = "Tên tour không được để trống";
				ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountName");
				ViewData["EndLocationId"] = new SelectList(_context.endLocations.OrderBy(c => c.EndLocationName), "EndLocationId", "EndLocationName", tours.EndLocationId);
				ViewData["StartLocationId"] = new SelectList(_context.startLocations.OrderBy(c => c.StartLocationName), "StartLocationId", "StartLocationName", tours.StartLocationId);
				ViewData["TourTypeId"] = new SelectList(_context.tourTypes.OrderBy(c => c.TourTypeName), "TourTypeId", "TourTypeName", tours.TourTypeId);
				return View();
			}
			else if (CheckingTours.checkTourName(_context, tourName))
			{
				ViewData["validation_message_tourName"] = "Tên tour đã tồn tại";
				ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountName");
				ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName", tours.EndLocationId);
				ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName", tours.StartLocationId);
				ViewData["TourTypeId"] = new SelectList(_context.tourTypes, "TourTypeId", "TourTypeName", tours.TourTypeId);
				return View();
			}
			else if (price.IsNullOrEmpty())
			{
				ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountName");
				ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName", tours.EndLocationId);
				ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName", tours.StartLocationId);
				ViewData["TourTypeId"] = new SelectList(_context.tourTypes, "TourTypeId", "TourTypeName", tours.TourTypeId);
				ModelState.AddModelError("Price", "Giá không được để trống");
				return View();
			}
			else if (int.Parse(price) < 0)
			{
				ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountName");
				ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName", tours.EndLocationId);
				ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName", tours.StartLocationId);
				ViewData["TourTypeId"] = new SelectList(_context.tourTypes, "TourTypeId", "TourTypeName", tours.TourTypeId);
				ModelState.AddModelError("Price", "Giá phải là số không âm");
				return View();
			}
			else if (remaining.IsNullOrEmpty())
			{
				ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountName");
				ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName", tours.EndLocationId);
				ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName", tours.StartLocationId);
				ViewData["TourTypeId"] = new SelectList(_context.tourTypes, "TourTypeId", "TourTypeName", tours.TourTypeId);
				ModelState.AddModelError("Remaining", "Số lượng tồn không được để trống");
				return View();
			}
			else if(int.Parse(remaining) < 0)
			{
				ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountName");
				ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName", tours.EndLocationId);
				ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName", tours.StartLocationId);
				ViewData["TourTypeId"] = new SelectList(_context.tourTypes, "TourTypeId", "TourTypeName", tours.TourTypeId);
				ModelState.AddModelError("Remaining", "Số lượng tồn phải là số không âm");
				return View(tours);
			}
			else if (startDate == null || startDate == default(DateTime))
			{
				ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountName");
				ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName", tours.EndLocationId);
				ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName", tours.StartLocationId);
				ViewData["TourTypeId"] = new SelectList(_context.tourTypes, "TourTypeId", "TourTypeName", tours.TourTypeId);
				ModelState.AddModelError("EndDate", "Ngày kết thúc không được bỏ trống");
				return View();
			}
			else if(endDate == null || startDate == default(DateTime))
			{
				ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountName");
				ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName", tours.EndLocationId);
				ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName", tours.StartLocationId);
				ViewData["TourTypeId"] = new SelectList(_context.tourTypes, "TourTypeId", "TourTypeName", tours.TourTypeId);
				ModelState.AddModelError("EndDate", "Ngày kết thúc không được bỏ trống");
				return View();
			}
			#endregion
			else
			{
				tours = new Tours()
				{
					TourName = tourName,
					Price = int.Parse(price),
					Status = 1,
					StartDate = startDate,
					EndDate = endDate,
					StartLocationId = int.Parse(startLocationId),
					EndLocationId = int.Parse(endLocationId),
					DiscountId = int.Parse(discountId),
					TourTypeId = int.Parse(tourTypeId),
					Remaining = int.Parse(remaining),
					PriceInclude = priceInclude,
					PriceNotInclude = priceNotInclude,
					Surcharge = surcharge,
					CancelChange = cancelChange,
					Note = note,
					Description = description,
				};
				_context.Add(tours);
				await _context.SaveChangesAsync();
				var newTourId = tours.TourId;
				string destinationFolder = Path.Combine(_webHostEnvironment.WebRootPath, "User", "img", "Tour");
				await ImageHandler.DownloadAndSaveImage(description, destinationFolder, newTourId, _context);
				
				return RedirectToAction(nameof(Index));
			}
		}

		// GET: Tour/ToursAdministrator/edit/5
		[HttpGet]
		[Route("edit/{id}")]
		public async Task<IActionResult> Edit(long? id)
		{
			if (id == null || _context.tours == null)
			{
				return NotFound();
			}

			var tours = await _context.tours.Include(t => t.discounts)
				.Include(t => t.startLocations)
				.Include(t => t.endLocations)
				.Include(t => t.tourTypes)
				.FirstOrDefaultAsync(t => t.TourId == id);
			var tourImages = await _context.tourImages.Where(t => t.TourId == id).ToListAsync();
			if (tours == null)
			{
				return NotFound();
			}
			else
			{
				ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountName");
				ViewData["EndLocationId"] = new SelectList(_context.endLocations, "EndLocationId", "EndLocationName", tours.EndLocationId);
				ViewData["StartLocationId"] = new SelectList(_context.startLocations, "StartLocationId", "StartLocationName", tours.StartLocationId);
				ViewData["TourTypeId"] = new SelectList(_context.tourTypes, "TourTypeId", "TourTypeName", tours.TourTypeId);
				return View(tours);
			}
		}

		// POST: Tour/ToursAdministrator/edit/5
		[HttpPost]
		[Route("edit/{id}")]
		public async Task<IActionResult> Edit(long? id, IFormCollection collection)
		{
			string tourName = collection["TourName"].ToString();
			string price = collection["Price"].ToString().Trim();
			string userInputStartDate = collection["StartDate"].ToString();
			string userInputEndDate = collection["EndDate"].ToString();
			string discountId = collection["DiscountId"].ToString();
			string startLocationId = collection["StartLocationId"].ToString();
			string endLocationId = collection["EndLocationId"].ToString();
			string tourTypeId = collection["TourTypeId"].ToString();
			string remaining = collection["Remaining"].ToString().Trim();
			string priceInclude = collection["PriceInclude"].ToString();
			string priceNotInclude = collection["PriceNotInclude"].ToString();
			string surcharge = collection["Surcharge"].ToString();
			string cancelChange = collection["CancelChange"].ToString();
			string note = collection["Note"].ToString();
			string description = collection["Description"].ToString();

			//Chuyển đổi thành DateTime hoàn toàn
			DateTime? startDate = !string.IsNullOrWhiteSpace(userInputStartDate) ?
					  Convert.ToDateTime(userInputStartDate) : (DateTime?)null;
			DateTime? endDate = !string.IsNullOrWhiteSpace(userInputEndDate) ?
					Convert.ToDateTime(userInputEndDate) : (DateTime?)null;

			var dbTours = await _context.tours.FindAsync(id);
			if(dbTours == null)
			{
				return NotFound();
			}
			try
			{
				#region kiểm tra đầu vào
				if (tourName.IsNullOrEmpty())
				{
					ViewData["validation_message_tourName"] = "Tên tour không được để trống";
					ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountId", dbTours.DiscountId);
					ViewData["EndLocationId"] = new SelectList(_context.endLocations.OrderBy(c => c.EndLocationName), "EndLocationId", "EndLocationName", dbTours.EndLocationId);
					ViewData["StartLocationId"] = new SelectList(_context.startLocations.OrderBy(c => c.StartLocationName), "StartLocationId", "StartLocationName", dbTours.StartLocationId);
					ViewData["TourTypeId"] = new SelectList(_context.tourTypes.OrderBy(c => c.TourTypeName), "TourTypeId", "TourTypeName", dbTours.TourTypeId);
					return View();
				}
				else if (CheckingTours.checkTourNameWhenUpdate(_context, tourName))
				{
					ViewData["validation_message_tourName"] = "Tên tour đã tồn tại";
					ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountId", dbTours.DiscountId);
					ViewData["EndLocationId"] = new SelectList(_context.endLocations.OrderBy(c => c.EndLocationName), "EndLocationId", "EndLocationName", dbTours.EndLocationId);
					ViewData["StartLocationId"] = new SelectList(_context.startLocations.OrderBy(c => c.StartLocationName), "StartLocationId", "StartLocationName", dbTours.StartLocationId);
					ViewData["TourTypeId"] = new SelectList(_context.tourTypes.OrderBy(c => c.TourTypeName), "TourTypeId", "TourTypeName", dbTours.TourTypeId);
					return View();
				}
				else if (price.IsNullOrEmpty())
				{
					ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountId", dbTours.DiscountId);
					ViewData["EndLocationId"] = new SelectList(_context.endLocations.OrderBy(c => c.EndLocationName), "EndLocationId", "EndLocationName", dbTours.EndLocationId);
					ViewData["StartLocationId"] = new SelectList(_context.startLocations.OrderBy(c => c.StartLocationName), "StartLocationId", "StartLocationName", dbTours.StartLocationId);
					ViewData["TourTypeId"] = new SelectList(_context.tourTypes.OrderBy(c => c.TourTypeName), "TourTypeId", "TourTypeName", dbTours.TourTypeId);
					ModelState.AddModelError("Price", "Giá không được để trống");
					return View();
				}
				else if (int.Parse(price) < 0)
				{
					ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountId", dbTours.DiscountId);
					ViewData["EndLocationId"] = new SelectList(_context.endLocations.OrderBy(c => c.EndLocationName), "EndLocationId", "EndLocationName", dbTours.EndLocationId);
					ViewData["StartLocationId"] = new SelectList(_context.startLocations.OrderBy(c => c.StartLocationName), "StartLocationId", "StartLocationName", dbTours.StartLocationId);
					ViewData["TourTypeId"] = new SelectList(_context.tourTypes.OrderBy(c => c.TourTypeName), "TourTypeId", "TourTypeName", dbTours.TourTypeId);
					ModelState.AddModelError("Price", "Giá phải là số không âm");
					return View();
				}
				else if (remaining.IsNullOrEmpty())
				{
					ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountId", dbTours.DiscountId);
					ViewData["EndLocationId"] = new SelectList(_context.endLocations.OrderBy(c => c.EndLocationName), "EndLocationId", "EndLocationName", dbTours.EndLocationId);
					ViewData["StartLocationId"] = new SelectList(_context.startLocations.OrderBy(c => c.StartLocationName), "StartLocationId", "StartLocationName", dbTours.StartLocationId);
					ViewData["TourTypeId"] = new SelectList(_context.tourTypes.OrderBy(c => c.TourTypeName), "TourTypeId", "TourTypeName", dbTours.TourTypeId);
					ModelState.AddModelError("Remaining", "Số lượng tồn không được để trống");
					return View();
				}
				else if (int.Parse(remaining) < 0)
				{
					ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountId", dbTours.DiscountId);
					ViewData["EndLocationId"] = new SelectList(_context.endLocations.OrderBy(c => c.EndLocationName), "EndLocationId", "EndLocationName", dbTours.EndLocationId);
					ViewData["StartLocationId"] = new SelectList(_context.startLocations.OrderBy(c => c.StartLocationName), "StartLocationId", "StartLocationName", dbTours.StartLocationId);
					ViewData["TourTypeId"] = new SelectList(_context.tourTypes.OrderBy(c => c.TourTypeName), "TourTypeId", "TourTypeName", dbTours.TourTypeId);
					ModelState.AddModelError("Remaining", "Số lượng tồn phải là số không âm");
					return View();
				}
				else if (startDate == null || startDate == default(DateTime))
				{
					ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountId", dbTours.DiscountId);
					ViewData["EndLocationId"] = new SelectList(_context.endLocations.OrderBy(c => c.EndLocationName), "EndLocationId", "EndLocationName", dbTours.EndLocationId);
					ViewData["StartLocationId"] = new SelectList(_context.startLocations.OrderBy(c => c.StartLocationName), "StartLocationId", "StartLocationName", dbTours.StartLocationId);
					ViewData["TourTypeId"] = new SelectList(_context.tourTypes.OrderBy(c => c.TourTypeName), "TourTypeId", "TourTypeName", dbTours.TourTypeId);
					ModelState.AddModelError("EndDate", "Ngày kết thúc không được bỏ trống");
					return View();
				}
				else if (endDate == null || startDate == default(DateTime))
				{
					ViewData["DiscountId"] = new SelectList(_context.discounts.OrderBy(c => c.DiscountName), "DiscountId", "DiscountId", dbTours.DiscountId);
					ViewData["EndLocationId"] = new SelectList(_context.endLocations.OrderBy(c => c.EndLocationName), "EndLocationId", "EndLocationName", dbTours.EndLocationId);
					ViewData["StartLocationId"] = new SelectList(_context.startLocations.OrderBy(c => c.StartLocationName), "StartLocationId", "StartLocationName", dbTours.StartLocationId);
					ViewData["TourTypeId"] = new SelectList(_context.tourTypes.OrderBy(c => c.TourTypeName), "TourTypeId", "TourTypeName", dbTours.TourTypeId);
					ModelState.AddModelError("EndDate", "Ngày kết thúc không được bỏ trống");
					return View();
				}
				#endregion
				else
				{
					dbTours.TourName = tourName;
					dbTours.Price = int.Parse(price);
					dbTours.Status = 1;
					dbTours.StartDate = Convert.ToDateTime(startDate);
					dbTours.EndDate = Convert.ToDateTime(endDate);
					dbTours.StartLocationId = int.Parse(startLocationId);
					dbTours.EndLocationId = int.Parse(endLocationId);
					dbTours.DiscountId = int.Parse(discountId);
					dbTours.TourTypeId = int.Parse(tourTypeId);
					dbTours.Remaining = int.Parse(remaining);
					dbTours.Description = description;
					dbTours.PriceInclude = priceInclude;
					dbTours.PriceNotInclude = priceNotInclude;
					dbTours.Surcharge = surcharge;
					dbTours.CancelChange = cancelChange;
					dbTours.Note = note;
					dbTours.UpdateDate = DateTime.Now;
					_context.Update(dbTours);
					await _context.SaveChangesAsync();

					var newTourId = dbTours.TourId; //get tour_id
					string destinationFolder = Path.Combine(_webHostEnvironment.WebRootPath, "User", "img", "Tour");
					await ImageHandler.DownloadAndSaveImage(description, destinationFolder, newTourId, _context);
					return RedirectToAction(nameof(Index));
				}
			} catch (DBConcurrencyException)
			{
				return View();
			}
		}

		// GET: Admin/ToursAdministrator/delete/5
		[HttpGet]
		[Route("delete/{id}")]
		public async Task<IActionResult> Delete(long? id)
		{
			if (id == null || _context.tours == null)
			{
				return NotFound();
			}

			var tours = await _context.tours
				.Include(t => t.discounts)
				.Include(t => t.endLocations)
				.Include(t => t.startLocations)
				.Include(t => t.tourTypes)
				.FirstOrDefaultAsync(m => m.TourId == id);
			if (tours == null)
			{
				return NotFound();
			}

			return View(tours);
		}

		// POST: Admin/ToursAdministrator/delete/5
		[HttpPost]
		[Route("delete/{id}")]
		public async Task<IActionResult> DeleteConfirmed(long id)
		{
			if (_context.tours == null)
			{
				return Problem("Entity set 'ApplicationDBContext.tours'  is null.");
			}
			var tours = await _context.tours.FindAsync(id);
			if (tours != null)
			{
				// Tìm tất cả các dòng trong tourImages có TOUR_ID trùng với id của tour đang xóa
				var tourImagesRelated = await _context.tourImages.Where(ti => ti.TourId == id).ToListAsync();
				if (tourImagesRelated.Any())
				{
					// Xóa tất cả các dòng trong tourImages có TOUR_ID trùng với id của tour đang xóa
					_context.tourImages.RemoveRange(tourImagesRelated);
				}
				// Set bookings' TOUR_ID to null where TOUR_ID is referencing the tour being deleted
				var bookingsToUpdate = await _context.bookings
					.Where(b => b.TourId == id)
					.ToListAsync();

				foreach (var booking in bookingsToUpdate)
				{
					booking.TourId = null;
				}
				_context.tours.Remove(tours);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ToursExists(long id)
		{
			return (_context.tours?.Any(e => e.TourId == id)).GetValueOrDefault();
		}
	}
}
