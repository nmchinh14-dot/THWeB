using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NgoMinhChinh_2026.Data;
using NgoMinhChinh_2026.Models;
using System.Diagnostics;

namespace NgoMinhChinh_2026.Controllers
{
    public class HomeController(QLHangHoaContext context) : Controller
    {
        public async Task<IActionResult> Index(int? maLoai, string? tuKhoa)
        {
            ViewData["MaLoai"] = maLoai;
            ViewData["TuKhoa"] = tuKhoa;
            return View(await LayHangHoa(maLoai, tuKhoa));
        }

        [HttpGet]
        public async Task<IActionResult> LocHangHoa(int? maLoai, string? tuKhoa)
        {
            return PartialView("NgoMinhChinh_DanhSachHangHoa", await LayHangHoa(maLoai, tuKhoa));
        }

        private Task<List<HangHoa>> LayHangHoa(int? maLoai, string? tuKhoa)
        {
            var query = context.HangHoa.AsNoTracking().Where(hang => hang.Gia >= 100);

            if (maLoai.HasValue)
            {
                query = query.Where(hang => hang.MaLoai == maLoai.Value);
            }

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                var tenHang = tuKhoa.Trim();
                query = query.Where(hang => hang.TenHang.Contains(tenHang));
            }

            return query.OrderBy(hang => hang.MaHang).ToListAsync();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
