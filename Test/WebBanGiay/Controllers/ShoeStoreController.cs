using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanGiay.Models;
using PagedList.Mvc;
using PagedList;

namespace WebBanGiay.Controllers
{
    public class ShoeStoreController : Controller
    {
        dbQLBanGiayDataContext data = new dbQLBanGiayDataContext();

        private List<GIAY> LayGiayMoi (int count)
        {
            return data.GIAYs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }

        public ActionResult SPtheoloai(int id)
        {
            var giay = from s in data.GIAYs where s.MaLOAI==id select s;
            return View(giay);
        }

        public ActionResult Details(int id)
        {
            var giay= from g in data.GIAYs where g.MaGIAY==id select g;
            return View(giay.Single());
        }

        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var giayMoi = LayGiayMoi(16);
            return View(giayMoi.ToPagedList(pageNum,pageSize));
        }
    }
}