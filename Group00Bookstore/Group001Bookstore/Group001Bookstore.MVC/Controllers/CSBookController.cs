using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group001Bookstore.MVC.Models;
using System.IO;

namespace Group001Bookstore.MVC.Controllers
{
    public class CSBookController : Controller
    {
        // GET: CSBook
        public ActionResult Show()
        {
            List<CSBook> allCSBook = null;

            Group001BookstoreEntities dbContext = new Group001BookstoreEntities();
            allCSBook = dbContext.CSBooks.ToList();

            return View(allCSBook);
        }

        [HttpGet]
        public ActionResult Detail(int uniqueId)
        {
            CSBook targetBook = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetBook = dbContext.CSBooks.SingleOrDefault(b => b.UniqueId == uniqueId);
            }
            return View(targetBook);
        }

        [HttpPost]
        public ActionResult Detail(CSBook book)
        {
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                if (this.Request.Files != null && this.Request.Files.Count > 0 && this.Request.Files[0].ContentLength > 0 && this.Request.Files[0].ContentLength < 1024 * 1024)
                {
                    string fileName = Path.GetFileName(this.Request.Files[0].FileName);
                    string pathOfWebsite = "~/Images/CSBookCovers/" + fileName;
                    book.CoverImagePath = pathOfWebsite;
                    this.Request.Files[0].SaveAs(this.Server.MapPath(pathOfWebsite));
                }

                dbContext.CSBooks.Attach(book);
                dbContext.Entry(book).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
            }
            return RedirectToAction("Show");
        }

        [HttpGet]
        public ActionResult AddToCart(int uniqueId)
        {
            CSBook targetBook = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetBook = dbContext.CSBooks.SingleOrDefault(b => b.UniqueId== uniqueId);
            }

            return View("OrderReview", targetBook);
        }

        [HttpPost]
        public ActionResult AddToCart(Order order)
        {
            User targetUser = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetUser = dbContext.Users.SingleOrDefault(u => u.Id == order.UserId);
                if (targetUser != null)
                {

                    dbContext.Orders.Add(order);
                    dbContext.SaveChanges();
                    targetUser = dbContext.Users.Include("Orders").SingleOrDefault(u => u.Id == order.UserId);
                    return View("Cart", targetUser);
                }
                else
                {
                    return View("Error");
                }
            }


        }
    }
}