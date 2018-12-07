using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group001Bookstore.MVC.Models;

namespace Group001Bookstore.MVC.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult UserRegister()
        {
            return View("InputUserInfo");
        }

        [HttpPost]
        public ActionResult UserRegister(User user)
        {
            User newUser = null;
            if (user.FirstName != null && user.LastName!=null && user.DateofBirth !=0 && this.ModelState.IsValid)
            {
                using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
                {
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();

                    newUser = dbContext.Users.SingleOrDefault(u => u.FirstName == user.FirstName && u.LastName == user.LastName && u.DateofBirth == user.DateofBirth);
                }

                return View("UserInfo", newUser);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult UserInfoUpdate()
        {
            return View("UserReviseUpdate");
        }

        [HttpPost]
        public ActionResult UserInfoRevise(int id)
        {
            User targetUser = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetUser = dbContext.Users.Include("Orders").SingleOrDefault(u => u.Id == id);
            }

            if (targetUser != null)
            {
                return View("UserInfoVerify", targetUser);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult UserRevise(User user)
        {
            if (user.FirstName != null && user.LastName != null && user.DateofBirth != 0 && this.ModelState.IsValid)
            {
                using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
                {
                    dbContext.Users.Attach(user);
                    dbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();
                }

                return View("UserInfo", user);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult ViewCart()
        {
            return View("UserVerification");
        }

        [HttpPost]
        public ActionResult ViewCart(int Id)
        {
            User targetUser = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetUser = dbContext.Users.Include("Orders").SingleOrDefault(u => u.Id == Id);
            }

            if (targetUser != null)
            {
                return View("Cart", targetUser);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult EditCart(int id, int userId)
        {
            Order targetOrder = null;
            User targetUser = null;
            using (Group001BookstoreEntities dbContext = new Group001BookstoreEntities())
            {
                targetUser = dbContext.Users.Include("Orders").SingleOrDefault(u => u.Id == userId);
                targetOrder = dbContext.Orders.SingleOrDefault(o => o.Id == id & o.UserId==userId);
                if (targetOrder == null)
                {
                    return View("Warning");
                }
                else
                {
                    dbContext.Orders.Remove(targetOrder);
                    dbContext.SaveChanges();
                    return View("Cart", targetUser);
                }
            }

        }
    }
}