using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class GiohangController : Controller
    {
        dbQLBanGiayDataContext data = new dbQLBanGiayDataContext();
        // GET: Giohang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang==null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang; 
        }
        public ActionResult ThemGioHang (int iMaGiay, string strURL)
        {
            // Lay session trong gio hang
            List<Giohang> lstGiohang = Laygiohang();
            // 
            Giohang sanpham = lstGiohang.Find(n => n.iMagiay == iMaGiay);
            if (sanpham==null)
            {
                sanpham = new Giohang(iMaGiay);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        //Tong so luong

        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang!=null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }

        //tong tien

        private decimal TongTien()
        {
            decimal iTongtien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang!=null)
            {
                iTongtien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongtien;
        }
        public ActionResult GioHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count==0)
            {
                return RedirectToAction("Index", "ShoeStore");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);

        }
        public ActionResult Index()
        {
            return View();
        }

        //Hiển thị View DatHang để cập nhật các thông tin cho đơn hàng
        [HttpGet]
        public ActionResult DatHang()
        {
            if(Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if(Session["Giohang"]==null)
            {
                return RedirectToAction("Index", "WebBanGiay");
            }

            //Lay gio hang tu Sesstion
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lstGiohang);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sach da co trong Session[giohang]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMagiay == iMaSP);
            //neu co ton tai thi cho sua soluong
            if(sanpham !=null)
            {
                lstGiohang.RemoveAll(n => n.iMagiay == iMaSP);
                return RedirectToAction("GioHang");
            }
            if(lstGiohang.Count ==0)
            {
                return RedirectToAction("Index", "ShoeStore");
            }
            return RedirectToAction("GioHang");
        }
        //cap nhat gio hang
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            //lay gio hang
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMagiay == iMaSP);
            if(sanpham !=null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        //Xoa tat ca
        public ActionResult XoaTatcaGiohang()
        {
            //lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            //kt
            lstGiohang.Clear();
            return RedirectToAction("Index", "ShoeStore");
        }

        public ActionResult DatHang(FormCollection collection)
        {
            //Them Don hang
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = string.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.TongTien = TongTien();
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //Them chi tiet don hang
            foreach(var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.Magiay = item.iMagiay;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}