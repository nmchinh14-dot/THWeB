using System;
using System.Collections.Generic;

namespace NgoMinhChinh_2026.Models;

public partial class LoaiHang
{
    public int MaLoai { get; set; }

    public string TenLoai { get; set; } = null!;

    public virtual ICollection<HangHoa> HangHoa { get; set; } = new List<HangHoa>();
}
