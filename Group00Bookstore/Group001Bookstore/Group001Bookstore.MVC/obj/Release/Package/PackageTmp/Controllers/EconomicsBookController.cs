using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group001Bookstore.MVC.Models;
namespace Group001Bookstore.MVC.Controllers
{
    public class EconomicsBookController : Controller
    {
        // GET: Book
        public ActionResult BookList()
        {
            Group001BookstoreEntities dbContext = new Group001BookstoreEntities();
            List<EconomicsBook> allBooks = dbContext.EconomicsBooks.ToList();
            return View(allBooks);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EconomicsBook targetBook = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetBook = dbContext.EconomicsBooks.SingleOrDefault(b => b.Id == id);
            }
            return View(targetBook);
        }

        [HttpPost]
        public ActionResult Edit(EconomicsBook book)
        {
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                if (this.Request.Files != null && this.Request.Files.Count > 0 && this.Request.Files[0].ContentLength > 0 && this.Request.Files[0].ContentLength < 1024 * 1024)
                {
                    book.CoverImagePath = this.Request.Files[0].FileName;
                }

                dbContext.EconomicsBooks.Attach(book);
                dbContext.Entry(book).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
            }
            return RedirectToAction("BookList");
        }

        [HttpGet]
        public ActionResult AddToCart(int uniqueId)
        {
            EconomicsBook targetBook = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetBook = dbContext.EconomicsBooks.SingleOrDefault(b => b.UniqueId == uniqueId);
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