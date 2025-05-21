using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class ProductsController : Controller
    {
        // GET: Admin/Products
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string SearchText, int? page, string sortName)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = db.Products.OrderByDescending(x => x.Id).ToList();

            ViewBag.NameSortParm = string.IsNullOrEmpty(sortName) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortName == "price" ? "price_desc" : "price";
            ViewBag.DateSortParm = sortName == "date" ? "date_desc" : "date";

            switch (sortName)
            {
                case "name_desc":
                    items = items.OrderByDescending(x => x.Title);
                    break;
                case "price":
                    items = items.OrderBy(x => x.Price);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(x => x.Price);
                    break;
                case "date":
                    items = items.OrderBy(x => x.Id);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(x => x.Id);
                    break;
                default:
                    items = items.OrderByDescending(x => x.Id);
                    break;
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                items = items.Where(x => x.Alias.Contains(SearchText) || x.Title.Contains(SearchText));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            ViewBag.Brand = new SelectList(db.Brands.ToList(), "Id", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product product, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            product.Image = Images[i];
                            product.ProductImage.Add(new ProductImage
                            {
                                ProductId = product.Id,
                                Image = Images[i],
                                IsDefault = true
                            });
                        }
                        else
                        {
                            product.ProductImage.Add(new ProductImage
                            {
                                ProductId = product.Id,
                                Image = Images[i],
                                IsDefault = false
                            });
                        }
                    }
                }
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                if (string.IsNullOrEmpty(product.SeoTitle))
                {
                    product.SeoTitle = product.Title;
                }
                if (string.IsNullOrEmpty(product.Alias))
                    product.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(product.Title);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            ViewBag.Brand = new SelectList(db.Brands.ToList(), "Id", "Title");
            return View(product);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            ViewBag.Brand = new SelectList(db.Brands.ToList(), "Id", "Title");
            var item = db.Products.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                product.ModifiedDate = DateTime.Now;
                product.Alias = WebBanHangOnline.Models.Common.Filter.FilterChar(product.Title);
                db.Products.Attach(product);
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = new SelectList(db.ProductCategories.ToList(), "Id", "Title");
            ViewBag.Brand = new SelectList(db.Brands.ToList(), "Id", "Title");
            return View(product);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        //[HttpPost]
        //public ActionResult IsSale(int id)
        //{
        //    var item = db.Products.Find(id);
        //    if (item != null)
        //    {
        //        item.IsSale = !item.IsSale;
        //        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //        return Json(new { success = true, IsSale = item.IsSale });
        //    }
        //    return Json(new { success = false });
        //}
        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsHome = item.IsHome });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsActive = item.IsActive });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = db.Products.Find(Convert.ToInt32(item));
                        db.Products.Remove(obj);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}