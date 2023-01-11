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
      //ViewBag.CopyCount = 0;
                          // JOIN books to copies; filter WHERE matching id; SELECT * ; COUNT instances
                          // SELECT * FROM books JOIN copies ... WHERE bookId = id 
      ViewBag.CopyCount = _db.Books.Include(copy => copy.Copies).Where(book => book.BookId == id).SelectMany(book => book.Copies).Count();
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      Book thisBook = _db.Books.Include(join => join.AuthorBooks)
                               .ThenInclude(authorBook => authorBook.Author)
                               .FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }
    [HttpPost]
    public ActionResult Delete(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      _db.Books.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    
    public ActionResult Edit(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }
    [HttpPost]
    public ActionResult Edit(Book book)
    {
      _db.Books.Update(book);
      _db.SaveChanges();
      return RedirectToAction("Details", "Books",  new { id = book.BookId});
    }
    public ActionResult AddAuthor(int id)
    {
      Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
      return View(thisBook);
    }

    [HttpPost]
    public ActionResult AddAuthor(Book book, int authorId)
    {

      #nullable enable
      AuthorBook? entry = _db.AuthorBooks.FirstOrDefault(entry => (entry.BookId == book.BookId && entry.AuthorId == authorId));
      #nullable disable
      if(entry == null && authorId !=0 )
      {
        _db.AuthorBooks.Add(new AuthorBook {AuthorId = authorId, BookId = book.BookId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", "Books", new { id = book.BookId});
    }
  }
}