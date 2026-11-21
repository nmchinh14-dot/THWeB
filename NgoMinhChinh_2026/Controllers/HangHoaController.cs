using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NgoMinhChinh_2026.Data;
using NgoMinhChinh_2026.Models;

namespace NgoMinhChinh_2026.Controllers;

public class HangHoaController(QLHangHoaContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await NapLoaiHang();
        return View(new HangHoa());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaLoai,TenHang,Gia,Anh")] HangHoa hangHoa)
    {
        if (ModelState.IsValid && !await context.LoaiHang.AnyAsync(loai => loai.MaLoai == hangHoa.MaLoai))
        {
            ModelState.AddModelError(nameof(HangHoa.MaLoai), "Loại hàng không tồn tại.");
        }

        if (!ModelState.IsValid)
        {
            await NapLoaiHang(hangHoa.MaLoai);
            return View(hangHoa);
        }

        hangHoa.TenHang = hangHoa.TenHang.Trim();
        hangHoa.Anh = hangHoa.Anh?.Trim();
        context.HangHoa.Add(hangHoa);
        await context.SaveChangesAsync();

        TempData["ThongBao"] = "Thêm hàng hóa thành công.";
        return RedirectToAction("Index", "Home");
    }

    private async Task NapLoaiHang(int? maLoai = null)
    {
        var loaiHang = await context.LoaiHang.AsNoTracking().OrderBy(loai => loai.MaLoai).ToListAsync();
        ViewBag.LoaiHang = new SelectList(loaiHang, nameof(LoaiHang.MaLoai), nameof(LoaiHang.TenLoai), maLoai);
    }
}
