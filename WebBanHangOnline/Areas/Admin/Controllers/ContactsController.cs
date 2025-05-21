using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models.EF;
using WebBanHangOnline.Models;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ContactsController : Controller
    {
        // GET: Admin/Contacts
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page)
        {
            var pageSize = 7;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Contact> item = db.Contacts.OrderByDescending(x => x.Id);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            item = item.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(item);
        }
        public ActionResult Detail(int id)
        {
            var item = db.Contacts.FirstOrDefault(x => x.Id == id);
            return View(item);
        }
    }
}