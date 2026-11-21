using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NgoMinhChinh_2026.Data;

namespace NgoMinhChinh_2026.ViewComponents;

public class LoaiHangViewComponent(QLHangHoaContext context) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var loaiHang = await context.LoaiHang.AsNoTracking().OrderBy(loai => loai.MaLoai).ToListAsync();
        return View(loaiHang);
    }
}
