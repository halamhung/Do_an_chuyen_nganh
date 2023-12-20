using HKQTravellingAuthenication.Areas.Tour.Extension;
using HKQTravellingAuthenication.Areas.Tour.Models;
using HKQTravellingAuthenication.Data;
using HKQTravellingAuthenication.Models.Tour;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HKQTravellingAuthenication.Areas.Tour.Controllers
{
	[Area("Tour")]
	[Route("discount-tour-manager")]
	[Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
	public class DiscountsAdministratorController : Controller
	{
		private readonly ApplicationDbContext _context;

		public DiscountsAdministratorController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Tour/discount-tour-manager/index
		[Route("index")]
		public async Task<IActionResult> Index()
		{
			return _context.discounts != null ?
						View(await _context.discounts.ToListAsync()) :
						Problem("Entity set 'ApplicationDBContext.discounts'  is null.");
		}

		// GET: Tour/discount-tour-manager/create
		[HttpGet]
		[Route("create")]
		public IActionResult Create()
		{
			return View();
		}

		// POST: Tour/discount-tour-manager/create
		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> Create(IFormCollection collection)
		{
			string discountPercentage = collection["DiscountPercentage"].ToString();
			string discountName = collection["DiscountName"].ToString();
			string discountDateStart = collection["DiscountDateStart"].ToString();
			string discountDateEnd = collection["DiscountDateEnd"].ToString();
			if (discountName == null)
			{
				ViewData["validation_message_discountName"] = "Tên giảm giá không được để trống!";
				return View();
			} 
			else if (discountPercentage == null)
			{
				ViewData["validation_message_discountPercentage"] = "Phần trăm giảm không được để trống!";
				return View();
			}
			else if (discountDateStart == null)
			{
				ViewData["validation_message_discountDateStart"] = "Ngày bắt đầu giảm không được để trống!";
				return View();
			}
			else if (discountDateEnd == null)
			{
				ViewData["validation_message_discountDateEnd"] = "Ngày kết thúc giảm không được để trống!";
				return View();
			}
			else if (checkingDiscounts.checkDiscountsName(_context, discountName))
			{
				ViewData["validation_message_discountName"] = "Tên giảm giá này đã tồn tại!";
				return View();
			}
			else
			{
				var dbDiscounts = new Discounts()
				{
					DiscountName = discountName,
					DiscountPercentage = Double.Parse(discountPercentage),
					DiscountDateStart = Convert.ToDateTime(discountDateStart),
					DiscountDateEnd = Convert.ToDateTime(discountDateEnd),
					Status = 1 //available
				};
				_context.Add(dbDiscounts);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}
		}

		// GET: Tour/discount-tour-manager/edit/5
		[HttpGet]
		[Route("edit/{id}")]
		public async Task<IActionResult> Edit(long? id)
		{
			if (id == null || _context.discounts == null)
			{
				return NotFound();
			}

			var discounts = await _context.discounts.FindAsync(id);
			if (discounts == null)
			{
				return NotFound();
			}

			// Chuyển đổi kiểu DateTime? sang chuỗi theo định dạng mong muốn
			ViewBag.DiscountDateStart = discounts.DiscountDateStart?.ToString("yyyy-MM-ddTHH:mm:ss");
			ViewBag.DiscountDateEnd = discounts.DiscountDateEnd?.ToString("yyyy-MM-ddTHH:mm:ss");
			return View(discounts);
		}

		// POST: Tour/discount-tour-manager/edit/5
		[HttpPost]
		[Route("edit/{id}")]
		public async Task<IActionResult> Edit(long id, IFormCollection collection)
		{
			string discountPercentage = collection["DiscountPercentage"].ToString();
			string discountName = collection["DiscountName"].ToString();
			string discountDateStart = collection["DiscountDateStart"].ToString();
			string discountDateEnd = collection["DiscountDateEnd"].ToString();
			string status = collection["Status"].ToString();
			var dbDiscounts = await _context.discounts.FindAsync(id);
			try
			{
				if (discountName == null)
				{
					ViewData["validation_message_discountName"] = "Tên giảm giá không được để trống!";
					return View();
				}
				else if (discountPercentage == null)
				{
					ViewData["validation_message_discountPercentage"] = "Phần trăm giảm không được để trống!";
					return View();
				}
				else if (discountDateStart == null)
				{
					ViewData["validation_message_discountDateStart"] = "Ngày bắt đầu giảm không được để trống!";
					return View();
				}
				else if (discountDateEnd == null)
				{
					ViewData["validation_message_discountDateEnd"] = "Ngày kết thúc giảm không được để trống!";
					return View();
				}
				else if (checkingDiscounts.checkDiscountsNameWhenUpdate(_context, discountName))
				{
					ViewData["validation_message_discountName"] = "Tên giảm giá này đã tồn tại!";
					return View();
				}
				else
				{
					dbDiscounts.DiscountName = discountName;
					dbDiscounts.DiscountPercentage = Double.Parse(discountPercentage);
					dbDiscounts.DiscountDateStart = Convert.ToDateTime(discountDateStart);
					dbDiscounts.DiscountDateEnd = Convert.ToDateTime(discountDateEnd);
					dbDiscounts.Status = 1;
					_context.Update(dbDiscounts);
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

		// GET: Tour/discount-tour-manager/delete/5
		[HttpGet]
		[Route("delete/{id}")]
		public async Task<IActionResult> Delete(long? id)
		{
			if (id == null || _context.discounts == null)
			{
				return NotFound();
			}

			var discounts = await _context.discounts
				.FirstOrDefaultAsync(m => m.DiscountId == id);
			if (discounts == null)
			{
				return NotFound();
			}

			return View(discounts);
		}

		// POST: Tour/discount-tour-manager/delete/5
		[HttpPost]
		[Route("delete/{id}")]
		public async Task<IActionResult> DeleteConfirmed(long id)
		{
			if (_context.discounts == null)
			{
				return Problem("Entity set 'ApplicationDBContext.discounts'  is null.");
			}
			var discounts = await _context.discounts.FindAsync(id);
			if (discounts != null)
			{
				var toursToUpdate = await _context.tours.Where(t => t.TourId == id).ToListAsync();
				foreach (var tour in toursToUpdate)
				{
					tour.DiscountId = null;
				}
				_context.discounts.Remove(discounts);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}
