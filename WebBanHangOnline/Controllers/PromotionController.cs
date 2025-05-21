using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Controllers
{
    public class PromotionController : Controller
    {
        // GET: Promotion
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var items = db.Promotion_Products.Where(x=>x.Promotion.StartDate <= DateTime.Now && x.Promotion.EndDate >= DateTime.Now ).ToList();
            return View(items);
        }
    }
}