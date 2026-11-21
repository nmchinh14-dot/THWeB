using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NgoMinhChinh_2026.Models;

public partial class HangHoa
{
    public int MaHang { get; set; }

    [Display(Name = "Loại hàng")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn loại hàng.")]
    public int MaLoai { get; set; }

    [Display(Name = "Tên hàng")]
    [Required(ErrorMessage = "Vui lòng nhập tên hàng.")]
    [StringLength(50, ErrorMessage = "Tên hàng không được vượt quá 50 ký tự.")]
    public string TenHang { get; set; } = null!;

    [Display(Name = "Giá")]
    [Required(ErrorMessage = "Vui lòng nhập giá.")]
    [Range(typeof(decimal), "100", "5000", ErrorMessage = "Giá phải nằm trong khoảng từ 100 đến 5000.")]
    public decimal? Gia { get; set; }

    [Display(Name = "Ảnh")]
    [Required(ErrorMessage = "Vui lòng nhập tên file ảnh.")]
    [StringLength(100, ErrorMessage = "Tên file ảnh không được vượt quá 100 ký tự.")]
    [RegularExpression(@"^[^\\/:*?""<>|\r\n]+\.([jJ][pP][gG]|[pP][nN][gG]|[gG][iI][fF]|[tT][iI][fF][fF])$",
        ErrorMessage = "Tên file ảnh phải có đuôi .jpg, .png, .gif hoặc .tiff.")]
    public string? Anh { get; set; }

    [ValidateNever]
    public virtual LoaiHang MaLoaiNavigation { get; set; } = null!;
}
