using HKQTravellingAuthenication.Areas.DashBoard.Models;
using HKQTravellingAuthenication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HKQTravellingAuthenication.Areas.DashBoard.Controllers
{
    [Area("DashBoard")]
    [Route("/DashBoard")]
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class DashBoardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashBoardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard-info")]
        public async Task<IActionResult> DashBoardInfo()
        {
            // Lấy thông tin thống kê từ cơ sở dữ liệu
            long totalPosts = await _context.Post.CountAsync();
            long totalUsers = await _context.Users.CountAsync();
            long totalBookings = await _context.bookings.CountAsync();
            long totalPayments = await _context.payments.CountAsync();
            double totalPrices = await _context.payments.SumAsync(p => p.TotalPrices ?? 0);
            long totalTours = await _context.tours.CountAsync();

            // Đưa thông tin đã thống kê vào một DTO hoặc ViewModel
            var dashboardInfo = new DashboardInfo
            {
                TotalPosts = totalPosts,
                TotalUsers = totalUsers,
                TotalBookings = totalBookings,
                TotalPayments = totalPayments,
                TotalPrices = totalPrices,
                TotalTours = totalTours,
            };

            // Trả về View với thông tin đã thống kê
            return View(dashboardInfo);
        }

        [HttpGet]
        [Route("dashboard-revenue-chart")]
        public async Task<IActionResult> RevenueChart()
        {
            // Lấy dữ liệu từ bảng Payments (ví dụ: tổng số tiền từng tháng)
            var revenueData = await _context.payments
                .Where(p => p.PaymentDate != null) // Lọc các bản ghi có ngày thanh toán không null
                .GroupBy(p => new { Month = p.PaymentDate.Value.Month, Year = p.PaymentDate.Value.Year })
                .Select(g => new { Month = g.Key.Month, Year = g.Key.Year, TotalRevenue = g.Sum(p => p.TotalPrices) })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            // Chuyển dữ liệu sang dạng dễ sử dụng trong JavaScript
            var chartData = revenueData.Select(d => new { Label = $"{d.Month}/{d.Year}", Revenue = d.TotalRevenue });

            return View(chartData);
        }

    }
}
