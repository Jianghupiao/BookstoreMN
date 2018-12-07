using Group001Bookstore.MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group001Bookstore.MVC.Controllers
{
    public class NovelController : Controller
    {
        // GET: Novel
        public ActionResult Show(int pageNumber)
        {
            List<GetNovelBooksofPage_Result> Novels = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                int bookPerPage = 10;
                Novels = dbContext.GetNovelBooksofPage(bookPerPage, pageNumber).ToList();
                ViewBag.MaxPageNumber = dbContext.NovelBooks.Count() / bookPerPage + 1;
            }

            return View("ShowNovel", Novels);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            NovelBook targetNovel = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetNovel = dbContext.NovelBooks.SingleOrDefault(n => n.Id == id);
            }

            return View("EditNovel", targetNovel);
        }

        [HttpPost]
        public ActionResult Edit(NovelBook novel)
        {
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                if (this.Request.Files != null && this.Request.Files.Count > 0 && this.Request.Files[0].ContentLength > 0 && this.Request.Files[0].ContentLength < 1024 * 100)
                {
                    string fileName = Path.GetFileName(this.Request.Files[0].FileName);
                    string filePathOfWebsite = "~/Images/NovelCovers/" + fileName;
                    novel.CoverImagePath = filePathOfWebsite;
                    this.Request.Files[0].SaveAs(this.Server.MapPath(filePathOfWebsite));
                }

                dbContext.NovelBooks.Attach(novel);
                dbContext.Entry(novel).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
            }

            return RedirectToAction("Show", new { pageNumber = 1 });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                NovelBook targetNovel = null;
                targetNovel = dbContext.NovelBooks.SingleOrDefault(n => n.Id == id);
                if (targetNovel == null)
                {
                    return View("Warning");
                }
                else
                {

                    dbContext.NovelBooks.Remove(targetNovel);
                    dbContext.SaveChanges();
                }
            }

            return RedirectToAction("Show", new { pageNumber = 1 });
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("AddNovel");
        }

        [HttpPost]
        public ActionResult Add(NovelBook novel)
        {
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                if (this.Request.Files != null && this.Request.Files.Count > 0 && this.Request.Files[0].ContentLength > 0 && this.Request.Files[0].ContentLength < 1024 * 100)
                {
                    string fileName = Path.GetFileName(this.Request.Files[0].FileName);
                    string filePathOfWebsite = "~/Images/NovelCovers/" + fileName;
                    novel.CoverImagePath = filePathOfWebsite;
                    this.Request.Files[0].SaveAs(this.Server.MapPath(filePathOfWebsite));
                }

                dbContext.NovelBooks.Add(novel);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Show", new { pageNumber = 1 });
        }

        [HttpGet]
        public ActionResult AddToCart(int uniqueId)
        {
            NovelBook targetBook = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetBook = dbContext.NovelBooks.SingleOrDefault(n => n.UniqueId == uniqueId);
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