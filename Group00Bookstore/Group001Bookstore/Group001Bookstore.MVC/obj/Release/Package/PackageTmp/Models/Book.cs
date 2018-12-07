using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Group001Bookstore.MVC.Models
{
    public class Book
    {
        public List<EconomicsBook> EconomicsBooks;
        public List<CSBook> CSBooks;
        public List<NovelBook> NovelBooks;
        public List<ChildrenBook> ChildrenBooks;
    }
}