using Group001Bookstore.MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group001Bookstore.MVC.Controllers
{
    public class ChildrenBookController : Controller
    {
        // GET: ChildrenBook
        public ActionResult Show()
        {
            List<ChildrenBook> allChildrenBooks = null;
            Group001BookstoreEntities dbContext = new Group001BookstoreEntities();
            allChildrenBooks = dbContext.ChildrenBooks.ToList();

            return View(allChildrenBooks);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ChildrenBook targetBook = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetBook = dbContext.ChildrenBooks.SingleOrDefault(b => b.Id == id);
            }

            return View("EditChildrenBook", targetBook);
        }
        [HttpPost]
        public ActionResult Edit(ChildrenBook ChildrenBook)
        {
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                if (this.Request.Files != null && this.Request.Files.Count > 0 && this.Request.Files[0].ContentLength > 0 && this.Request.Files[0].ContentLength < 1024 * 100)
                {
                    string fileName = Path.GetFileName(this.Request.Files[0].FileName);
                    string filePathOfWebsite = "~/Images/ChildrenBookCover/" + fileName;
                    ChildrenBook.CoverImagePath = filePathOfWebsite;
                    this.Request.Files[0].SaveAs(this.Server.MapPath(filePathOfWebsite));//无法添加图片，路径名不对。

                }
                dbContext.ChildrenBooks.Attach(ChildrenBook);
                dbContext.Entry(ChildrenBook).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
            }
            return RedirectToAction("Show");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                ChildrenBook targetBook = null;
                targetBook = dbContext.ChildrenBooks.SingleOrDefault(b => b.Id == id);
                if (targetBook == null)
                {
                    return View("Warning");
                }
                else
                {
                    dbContext.ChildrenBooks.Remove(targetBook);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("Show");
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View("AddChildrenBook");
        }
        [HttpPost]
        public ActionResult Add(ChildrenBook childrenBook)
        {
            using (Group001BookstoreEntities dbContext=new Group001BookstoreEntities())
            {
                if (this.Request.Files !=null&&this.Request.Files.Count>0&&this.Request.Files[0].ContentLength>0&&this.Request.Files[0].ContentLength<1024*100)
                {
                    string fileName = Path.GetFileName(this.Request.Files[0].FileName);
                    string filePathOfWebsite = "~/Images/ChildrenBookCover" + fileName;
                    childrenBook.CoverImagePath = filePathOfWebsite;
                    this.Request.Files[0].SaveAs(this.Server.MapPath(filePathOfWebsite));
                }
                dbContext.ChildrenBooks.Add(childrenBook);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Show");
        }
        [HttpGet]
        public ActionResult AddToCart(int uniqueId)
        {
            ChildrenBook targetBook = null;
            using (Group001BookstoreEntities dbContext=new Group001BookstoreEntities())
            {
                targetBook = dbContext.ChildrenBooks.SingleOrDefault(c => c.UniqueId == uniqueId);
            }
            return View("OrderReview",targetBook);
        }
        [HttpPost]
        public ActionResult AddToCart( Order order)
        {
            User targetUser = null;
            using (Group001BookstoreEntities dbContext=new Group001BookstoreEntities())
            {
                targetUser = dbContext.Users.SingleOrDefault(u => u.Id == order.UserId);
                if (targetUser!=null)
                {
                    dbContext.Orders.Add(order);
                    dbContext.SaveChanges();
                    targetUser = dbContext.Users.Include("Orders").SingleOrDefault(u => u.Id == order.UserId);
                    return View("Cart",targetUser);
                }
                else
                {
                    return View("Error");
                }
            }
                    }
    }
}

    