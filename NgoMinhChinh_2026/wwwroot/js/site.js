(() => {
    const danhSach = document.getElementById("danh-sach-hang-hoa");
    const trangThai = document.getElementById("filter-status");
    const timKiem = document.getElementById("tim-kiem-hang-hoa");
    let yeuCauHienTai;
    let soLuong = 0;
    let tongTien = 0;

    function capNhatDieuHuong(url) {
        const maLoai = url.searchParams.get("maLoai") || "";

        document.querySelectorAll("a[data-loai-hang]").forEach(link => {
            const duocChon = link.dataset.loaiHang === maLoai;
            link.parentElement.classList.toggle("active", duocChon);
            if (duocChon) {
                link.setAttribute("aria-current", "page");
            } else {
                link.removeAttribute("aria-current");
            }
        });

        if (timKiem) {
            timKiem.elements.maLoai.value = maLoai;
            timKiem.elements.tuKhoa.value = url.searchParams.get("tuKhoa") || "";
        }
    }

    async function locHangHoa(url, luuLichSu) {
        yeuCauHienTai?.abort();
        const yeuCau = new AbortController();
        yeuCauHienTai = yeuCau;
        danhSach.setAttribute("aria-busy", "true");
        trangThai.hidden = true;

        try {
            const diaChi = new URL(danhSach.dataset.filterUrl, window.location.origin);
            diaChi.search = url.search;
            const phanHoi = await fetch(diaChi, {
                headers: { "X-Requested-With": "XMLHttpRequest" },
                signal: yeuCau.signal
            });

            if (!phanHoi.ok) {
                throw new Error(String(phanHoi.status));
            }

            const noiDung = await phanHoi.text();
            if (yeuCau.signal.aborted) {
                return;
            }

            danhSach.innerHTML = noiDung;
            capNhatDieuHuong(url);
            if (luuLichSu) {
                window.history.pushState(null, "", url.pathname + url.search);
            }
        } catch (loi) {
            if (loi.name !== "AbortError") {
                trangThai.textContent = "Không tải được hàng hóa. Vui lòng thử lại.";
                trangThai.hidden = false;
            }
        } finally {
            if (yeuCauHienTai === yeuCau) {
                danhSach.setAttribute("aria-busy", "false");
            }
        }
    }

    document.addEventListener("click", event => {
        const lienKet = event.target.closest("a[data-loai-hang]");
        if (lienKet && danhSach && event.button === 0 && !event.ctrlKey && !event.metaKey && !event.shiftKey && !event.altKey) {
            event.preventDefault();
            locHangHoa(new URL(lienKet.href), true);
        }

        const nutGioHang = event.target.closest("button[data-add-to-cart]");
        if (nutGioHang) {
            soLuong += 1;
            tongTien += Number(nutGioHang.dataset.price);
            document.getElementById("cart-summary").textContent = `${soLuong} item(s) - $${tongTien.toFixed(2)}`;
        }
    });

    timKiem?.addEventListener("submit", event => {
        if (!danhSach) {
            return;
        }

        event.preventDefault();
        const url = new URL(timKiem.action);
        const duLieu = new FormData(timKiem);
        for (const [ten, giaTri] of duLieu) {
            if (String(giaTri).trim()) {
                url.searchParams.set(ten, String(giaTri).trim());
            }
        }
        locHangHoa(url, true);
    });

    window.addEventListener("popstate", () => {
        if (danhSach) {
            locHangHoa(new URL(window.location.href), false);
        }
    });

    document.addEventListener("error", event => {
        const anh = event.target;
        if (anh instanceof HTMLImageElement && anh.hasAttribute("data-product-image") && anh.dataset.fallbackSrc) {
            const anhDuPhong = anh.dataset.fallbackSrc;
            delete anh.dataset.fallbackSrc;
            anh.src = anhDuPhong;
        }
    }, true);
})();
