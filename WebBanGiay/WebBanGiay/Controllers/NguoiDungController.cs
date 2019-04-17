using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class NguoiDungController : Controller
    {
        dbQLBanGiayDataContext data = new dbQLBanGiayDataContext();

        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {

            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhatrlai"];


            var diachi = collection["Diachi"];
            var email = collection["Email"];

            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}",collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Ban phai nhap ho ten";
            }
            else if (String.IsNullOrEmpty(tendn)) {
                ViewData["Loi2"] = "Ban phai nhap ten dang nhap";

            }
            else if (String.IsNullOrEmpty(matkhau)) {
                ViewData["Loi3"] = "Ban phai nhap mat khau";

            }
            else if (String.IsNullOrEmpty(matkhaunhaplai)) {
                ViewData["Loi4"] = "Ban phai nhap lai mat khau";

            }
            if (String.IsNullOrEmpty(email)) {
                ViewData["Loi7"] = "Ban phai nhap email";

            }
            if (String.IsNullOrEmpty(dienthoai)) {
                ViewData["Loi8"] = "Ban phai nhap dien thoai";

            }
            if (String.IsNullOrEmpty(diachi)) {
                ViewData["Loi6"] = "Ban phai nhap dia chi";
            }
            else
            {
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse( ngaysinh);
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Dangnhap");
                
            }
            return this.Dangky();

        }
        
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["Tendn"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Vui long nhap ten dang nhap";
            }
            else if (String.IsNullOrEmpty(matkhau)) {
                ViewData["Loi2"] = "Vui long nhap mat khau";
            }
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    //ViewBag.Thongbao = "Chúng mừng bạn đã đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("GioHang", "Giohang");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập  hoặc mật khẩu không đúng";
                }
            

            }
            return View();
        }


    }
}