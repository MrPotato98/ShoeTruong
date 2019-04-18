using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Linq;

namespace WebBanGiay.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbQLBanGiayDataContext data = new dbQLBanGiayDataContext();

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var session = Session[CommonConstants.]
        //    base.OnActionExecuting(filterContext);
        //}
        public ActionResult Index(DateTime? NgayA, DateTime? NgayB)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            //Tính tổng doanh thu
            TempData["TongDoanhThu"] = data.DONDATHANGs.Where(n => n.Tinhtranggiaohang == true && n.Ngaygiao.ToString() != "" && n.Dathanhtoan == true).Sum(n => n.TongTien);

            //Đếm đơn hàng chưa duyệt
            TempData["DonHangChuaDuyet"] = data.DONDATHANGs.Where(n => n.Tinhtranggiaohang == false).Count();

            //Đếm đơn hàng chờ giao
            TempData["DonHangChoGiao"] = data.DONDATHANGs.Where(n => n.Tinhtranggiaohang == true && n.Dathanhtoan == false).Count();

            //Đếm số khách hàng
            TempData["TongKhachHang"] = data.KHACHHANGs.Count();
            return View(data.DONDATHANGs.Where(n => n.Ngaydat >= NgayA && n.Ngaydat < NgayB && n.Tinhtranggiaohang == true).ToList());
        }
        public Object TKDoanhThuTheoNgay(DateTime? NgayA, DateTime? NgayB)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            TempData["NgayA"] = NgayA;
            TempData["NgayB"] = NgayB;
            if (NgayA == null || NgayB == null)
            {
                TempData["DoanhThuTheoNgay"] = "0";
                return RedirectToAction("Index", "Admin");
            }
            TempData["DoanhThuTheoNgay"] = data.DONDATHANGs.Where(n => (n.Ngaygiao) > NgayA && n.Ngaygiao <= NgayB && n.Tinhtranggiaohang == true && n.Dathanhtoan == true).Sum(n => n.TongTien);
            return RedirectToAction("Index", "Admin");
        }
       
        public ActionResult DanhSachChuaDuyet(string timkiem, int? page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (timkiem != null)
            {
                List<DONDATHANG> listKQ = data.DONDATHANGs.Where(n => n.MaDonHang.ToString().Contains(timkiem) || n.KHACHHANG.MaKH.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(data.DONDATHANGs.Where(n => n.Ngaygiao.ToString() != null && n.Tinhtranggiaohang == false).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.Where(n => n.Ngaygiao.ToString() != null && n.Tinhtranggiaohang == false).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
            }
            //return View(data.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
            return View(data.DONDATHANGs.Where(n => n.Ngaygiao.ToString() != null && n.Tinhtranggiaohang == false).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DuyetDonHang(int id)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            DONDATHANG dh = data.DONDATHANGs.Where(n => n.MaDonHang == id).SingleOrDefault();
            dh.Tinhtranggiaohang = true;
            dh.Dathanhtoan = false;
            data.SubmitChanges();
            return RedirectToAction("DanhSachChuaDuyet", "Admin");

        }
        public ActionResult DanhSachChuaGiao(string timkiem, int? page)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (timkiem != null)
            {
                List<DONDATHANG> listKQ = data.DONDATHANGs.Where(n => n.MaDonHang.ToString().Contains(timkiem) || n.MaKH.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(data.DONDATHANGs.Where(n => n.Ngaygiao.ToString() != null && n.Tinhtranggiaohang == true && n.Dathanhtoan == false).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.Where(n => n.Ngaygiao.ToString() != null && n.Tinhtranggiaohang == true && n.Dathanhtoan == false).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
            }
            return View(data.DONDATHANGs.Where(n => n.Ngaygiao.ToString() != null && n.Tinhtranggiaohang == true && n.Dathanhtoan == false).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DuyetGiaoHang(int id)
        {

            DONDATHANG dh = data.DONDATHANGs.Where(n => n.MaDonHang == id).SingleOrDefault();
            dh.Dathanhtoan = true;
            data.SubmitChanges();
            return RedirectToAction("DanhSachChuaGiao", "Admin");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(FormCollection collection)
        {
            //gan cac gia tri ng dung len cac bien
            var tendn = collection["username"];
            var matkhau = collection["password"];

            //gan gia tri cho doi tuong tao moi (ad)
            Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);

            if (ad != null)
            {

                Session["Taikhoanadmin"] = ad;
                Session["Admin"] = ad.UserAdmin;

                //Session.Add(CommonConstants.)
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            else
                ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";

            return Redirect("Index");
        }
        [HttpGet]
        public ActionResult AdminLogin()
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }


        [HttpPost]
        public ActionResult AdminLogin(FormCollection collection, Admin ad)
        {


            var TenTK = collection["username"];
            var Pass = collection["password"];
            var hoten = collection["Hoten"];

            if (String.IsNullOrEmpty(TenTK))
            {
                ViewData["LoiTenTK"] = "TenTK Khong Duoc Trong";
            }
            else if (String.IsNullOrEmpty(Pass))
            {
                ViewData["LoiPass"] = "Pass Khong Duoc Trong";
            }
            else
            {
                try
                {
                    ad.UserAdmin = TenTK;
                    ad.PassAdmin = Pass;
                    ad.Hoten = hoten;
                    UpdateModel(ad);
                    data.Admins.InsertOnSubmit(ad);
                    data.SubmitChanges();
                }
                catch (Exception) { ViewData["LoiTenTK2"] = "Email Da TOn Tai"; }
                return RedirectToAction("Index");

            }


            return this.AdminLogin();
        }
        //logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult TaiKhoanKH(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            //return View(db.tb_HangHoas.ToList());
            return View(data.KHACHHANGs.ToList().OrderBy(n => n.HoTen).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult XoaKH(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = kh.MaKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpPost, ActionName("XoaKH")]
        public ActionResult XacNhanXoaKH(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = kh.MaKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KHACHHANGs.DeleteOnSubmit(kh);
            data.SubmitChanges();
            return RedirectToAction("TaiKhoanKH");
        }

        public ActionResult Giay(int? page)
        {

            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            //return View(data.GIAYs.ToList());
            return View(data.GIAYs.ToList().OrderBy(n => n.MaGIAY).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            //lay du lieu vao dropdownlist
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.MaLoai = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLOAI), "MaLoai", "TenLoai");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(GIAY giay, HttpPostedFileBase fileupload)
        {
            //Dua du lieu vao dropdown
            ViewBag.MaLoai = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLOAI), "MaLoai", "TenLoai", giay.MaLOAI);
            //kiem tra duong dan file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file, luu y bo sung thu vien using system.io;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    //Kiem tra hinh anh ton tai chua?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    giay.Anhbia = fileName;
                    //Luu vào csdl
                    data.GIAYs.InsertOnSubmit(giay);
                    data.SubmitChanges();
                }
                return RedirectToAction("Giay");
            }
        }

        //hien thi san pham
        public ActionResult Details(int id)
        {
            // lay ra doi tuong giay theo ma
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            GIAY giay = data.GIAYs.SingleOrDefault(n => n.MaGIAY == id);
            ViewBag.MaGIAY = giay.MaGIAY;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }

        //xoa san pham
        [HttpGet]
        public ActionResult Delete(int id)
        {
            //lay doi tuong giay ra
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            GIAY giay = data.GIAYs.SingleOrDefault(n => n.MaGIAY == id);
            ViewBag.MaGIAY = giay.MaGIAY;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Xacnhanxoa(int id)
        {
            //lay doi tuong giay
            GIAY giay = data.GIAYs.SingleOrDefault(n => n.MaGIAY == id);
            ViewBag.MaGIAY = giay.MaGIAY;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.GIAYs.DeleteOnSubmit(giay);
            data.SubmitChanges();
            return RedirectToAction("Giay");
        }

        //chinh sua san pham
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Taikhoanadmin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            //Lay ra doi tuong sach theo ma
            GIAY giay = data.GIAYs.SingleOrDefault(n => n.MaGIAY == id);
            ViewBag.MaGIAY = giay.MaGIAY;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, s?p xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaLoai = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLOAI), "MaLoai", "TenLoai", giay.MaGIAY);
            return View(giay);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(GIAY giay, HttpPostedFileBase fileupload)
        {
            int a = giay.MaGIAY;
            //Dua du lieu vao dropdownload
            ViewBag.MaLoai = new SelectList(data.LOAIs.ToList().OrderBy(n => n.TenLOAI), "MaLoai", "TenLoai", giay.MaGIAY);
            //Kiem tra duong dan file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View(giay);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileupload.SaveAs(path);
                    }
                    GIAY giay2 = data.GIAYs.Where(n => n.MaGIAY == giay.MaGIAY).SingleOrDefault();
                    string img = fileName;
                    giay2.TenGIAY = Request.Form["TenGIAY"];
                    giay2.Soluongton = int.Parse(Request.Form["Soluongton"]);
                    giay2.Anhbia = img;
                    giay2.Giaban = decimal.Parse(Request.Form["Giaban"]);
                    giay2.Mota = Request.Form["Mota"];
                    giay2.Ngaycapnhat = DateTime.Parse(Request.Form["Ngaycapnhat"]);
                    giay2.MaLOAI = int.Parse(Request.Form["MaLOAI"]);


                    //Luu vao CSDL   
                    UpdateModel(giay);
                    try
                    {
                        data.SubmitChanges();
                    }
                    catch (ChangeConflictException e)
                    {
                        Console.WriteLine(e.Message);
                        // Make some adjustments.
                        // ...
                        // Try again.
                        data.SubmitChanges();
                    }

                }
                return RedirectToAction("Giay");
            }
        }
        public ActionResult TongDonHang(string timkiem, int? page)
        {
            //if (Session["Taikhoanadmin"] == null)
            //{
            //    return RedirectToAction("Login", "Admin");
            //}
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            if (timkiem != null)
            {
                List<DONDATHANG> listKQ = data.DONDATHANGs.Where(n => n.MaDonHang.ToString().Contains(timkiem) || n.MaKH.ToString().Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy đơn hàng nào phù hợp.";
                    return View(data.DONDATHANGs.Where(n => n.Tinhtranggiaohang == true && n.Dathanhtoan == true).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.Where(n =>  n.Tinhtranggiaohang == true && n.Dathanhtoan == true).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
            }
            return View(data.DONDATHANGs.Where(n =>  n.Tinhtranggiaohang == true && n.Dathanhtoan == true).OrderByDescending(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }
    }
}