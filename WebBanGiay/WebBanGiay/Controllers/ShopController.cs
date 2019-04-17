using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class ShopController : Controller
    {
        dbQLBanGiayDataContext data = new dbQLBanGiayDataContext();
        private List<GIAY> LayGiayMoi(int count)
        {
            return data.GIAYs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult Details(int id)
        {
            var giay = from g in data.GIAYs where g.MaGIAY == id select g;
            return View(giay.Single());
        }

        // GET: ShoeStore
        public ActionResult Index(int? page)
        {
            var category = from u in data.LOAIs
                           select u;
            //tao bien quy dinh so sp tren moi trang
            int pageSize = 5;
            //tao bien so trang
            int pageNum = (page ?? 1);

            //lay top 5 giay ban chay nhat

            var giayMoi = LayGiayMoi(16);
            ViewData["loai"] = category;
            return View(giayMoi.ToPagedList(pageNum, pageSize));
        }

        public ActionResult SPtheoloai(int id)
        {
            var giay = data.GIAYs.Where(m => m.MaLOAI == id).ToList();
            return View(giay);
        }

        [WebMethod]
        public static string GetDataSP(int? id)
        {
            dbQLBanGiayDataContext data = new dbQLBanGiayDataContext();
            var giay = data.GIAYs.Where(m => m.MaLOAI == id).FirstOrDefault();
            return giay.ToString();
        }
    }
}