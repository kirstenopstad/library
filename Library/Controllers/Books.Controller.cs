using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models
{
  public class BooksController : Controller
  {
    private readonly LibraryContext _db;
    public BooksController(LibraryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Books.ToList());
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Book book)
    {
      if(!ModelState.IsValid)
      {
        return View(book);
      }
      else 
      {     
        _db.Books.Add(book);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }
  }
}