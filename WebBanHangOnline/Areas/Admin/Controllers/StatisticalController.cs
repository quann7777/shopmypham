using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class StatisticalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Statistical
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails
                        on o.Id equals od.OrderId
                        join pd in db.ProductDetails
                        on od.ProductDetailId equals pd.Id
                        join p in db.Products
                        on pd.ProductId equals p.Id
                        select new
                        {
                            CreatedDate = o.CreatedDate,
                            OrderId = od.OrderId,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            PriceProduct = p.Price
                        };
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate >= startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                query = query.Where(x => x.CreatedDate < endDate);
            }

            var result = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(x => new
            {
                Date = x.Key.Value,
                TotalBuy = x.Sum(y => y.Quantity * y.PriceProduct),
                TotalSell = x.Sum(y => y.Quantity * y.Price),
                TotalProduct = x.Sum(y=>y.Quantity),
                TotalOrder = x.Select(y => y.OrderId).Distinct().Count()
            }).Select(x => new
            {
                Date = x.Date,
                TongsoSp = x.TotalProduct,
                TongOrder = x.TotalOrder,
                DoanhThu = x.TotalSell,
                LoiNhuan = x.TotalSell - x.TotalBuy
            });
            return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        }

    }
}