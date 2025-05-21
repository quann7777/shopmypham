using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models.EF;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class PromotionDetailController : Controller
    {
        // GET: Admin/PromotionDetail
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int id)
        {
            ViewBag.PromotionId = id;
            ViewBag.Products = db.Products.ToList();
            var items = db.Promotion_Products.Where(x => x.PromotionId == id).ToList();
            return View(items);
        }

        [HttpPost]
        public ActionResult Add(int promotionId, int productId, int type)
        {
            var item = db.Promotion_Products.FirstOrDefault(x => x.PromotionId == promotionId && x.ProductId==productId);
            if (item == null)
            {
                db.Promotion_Products.Add(new Promotion_Product
                {
                    PromotionId = promotionId,
                    ProductId = productId,
                    Type = type
                });
                db.SaveChanges();

            }
            return Json(new { Success = true });

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Promotion_Products.Find(id);
            db.Promotion_Products.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}