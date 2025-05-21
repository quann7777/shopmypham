using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductDetailController : Controller
    {
        // GET: Admin/ProductQuaniti
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            ViewBag.Sizes = db.Sizes.ToList();
            var items = db.ProductDetails.Where(x => x.ProductId == id).ToList();
            return View(items);
        }

        [HttpPost]
        public ActionResult Add(int productId, int sizeId, int quantity)
        {
            var item = db.ProductDetails.FirstOrDefault(x => x.ProductId == productId && x.SizeId == sizeId);
            if(item == null)
            {
                db.ProductDetails.Add(new ProductDetail
                {
                    ProductId = productId,
                    SizeId = sizeId,
                    Quantity = quantity,
                });
                db.SaveChanges();
                
            }
            return Json(new { Success = true });

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductDetails.Find(id);
            db.ProductDetails.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }
    }
}