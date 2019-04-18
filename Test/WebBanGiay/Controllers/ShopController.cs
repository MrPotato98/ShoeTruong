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

        [HttpGet]
        public ActionResult Index(int? page)
        {
            var category = from u in data.LOAIs select u;
            return View(category);
        }

        public ActionResult SPtheoloai(int id)
        {
            var giay = data.GIAYs.Where(m => m.MaLOAI == id).ToList();
            return PartialView("_LayoutCategory", giay);
        }
    }
}