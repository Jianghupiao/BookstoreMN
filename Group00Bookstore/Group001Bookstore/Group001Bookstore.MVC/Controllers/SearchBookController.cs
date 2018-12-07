using Group001Bookstore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group001Bookstore.MVC.Controllers
{
    public class SearchBookController : Controller
    {
        // GET: Search


        [HttpPost]
        public ActionResult Search(string searchString)
        {
            Book targetBooks =new Book();
            List<EconomicsBook> targetEconomicsBooks = null;
            List<CSBook> targetCSBooks = null;
            List<NovelBook> targetNovelBooks = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetCSBooks = dbContext.CSBooks.Where(b => b.Name.Contains(searchString) || b.Author.Contains(searchString)).ToList();
                targetNovelBooks = dbContext.NovelBooks.Where(b => b.Name.Contains(searchString) || b.Author.Contains(searchString)).ToList();
                targetEconomicsBooks = dbContext.EconomicsBooks.Where(b => b.Name.Contains(searchString) || b.Author.Contains(searchString)).ToList();

                targetBooks.CSBooks = targetCSBooks;
                targetBooks.NovelBooks = targetNovelBooks;
                targetBooks.EconomicsBooks = targetEconomicsBooks;

            }
            if (targetBooks == null)
            {
                return View("InputSearch");
            }
            else
            {
                return View("ReturnSearchBook", targetBooks);
            }



        }

        [HttpGet]
        public ActionResult AddToCart(int uniqueId)
        {
            SingleBook singlebook = new SingleBook();
            CSBook CStargetBook = null;
            EconomicsBook EconomicstargetBook = null;
            NovelBook NoveltargetBook = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                if (uniqueId < 2000)
                {
                    CStargetBook = dbContext.CSBooks.SingleOrDefault(n => n.UniqueId == uniqueId);
                    singlebook.CSBook = CStargetBook;
                }
                else if (uniqueId > 2000 & uniqueId < 3000)
                {
                    EconomicstargetBook = dbContext.EconomicsBooks.SingleOrDefault(n=>n.UniqueId==uniqueId);
                    singlebook.Economicsbook = EconomicstargetBook;
                }
                else
                {
                    NoveltargetBook = dbContext.NovelBooks.SingleOrDefault(n => n.UniqueId == uniqueId);
                    singlebook.NovelBook = NoveltargetBook;
                }
                
            }

            return View("OrderReview", singlebook);
        }

        [HttpPost]
        public ActionResult AddToCart(Order order)
        {
            User targetUser = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetUser = dbContext.Users.SingleOrDefault(u => u.Id == order.UserId);
            }

            if (targetUser != null)
            {
                using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
                {
                    dbContext.Orders.Add(order);
                    dbContext.SaveChanges();
                    targetUser = dbContext.Users.Include("Orders").SingleOrDefault(u => u.Id == order.UserId);
                    return View("Cart", targetUser);
                }
                
            }
            else
            {
                return View("Error");
            }
        }
    }
}