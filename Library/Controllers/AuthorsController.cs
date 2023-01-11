using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Library.Models;

namespace Library.Controllers
{
  public class AuthorsController : Controller
  {
    // Routes
    private readonly LibraryContext _db;
    public AuthorsController(LibraryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Author> authors = _db.Authors.ToList();
      return View(authors);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Author author)
    {
      if (!ModelState.IsValid)
      {
        return View(author);
      }
      else
      {
        _db.Authors.Add(author);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }

    }

    public ActionResult Details(int id)
    {
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      Author thisAuthor = _db.Authors
        .Include(joinEntry => joinEntry.AuthorBooks)
        .ThenInclude(entity => entity.Book)
        .FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }

    public ActionResult Edit(int id)
    {
      Author thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      return View(thisAuthor);
    }
    [HttpPost]
    public ActionResult Edit(Author author)
    {
      if (!ModelState.IsValid)
      {
        return View(author);
      }
      else
      {
        _db.Authors.Update(author);
        _db.SaveChanges();
        return RedirectToAction("Details", "Authors", new { id = author.AuthorId });
      }
    }

    [HttpPost]
    public ActionResult Delete(int id)
    {
      Author thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
      _db.Authors.Remove(thisAuthor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddBook(int id)
    {
      Author thisAuthor = _db.Authors.FirstOrDefault(entry => entry.AuthorId == id);
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View(thisAuthor);
    }

    [HttpPost]
    public ActionResult AddBook(Author author, int bookId)
    {
      // check if already an AuthorBook association
#nullable enable
      AuthorBook? authorBookEntity = _db.AuthorBooks.FirstOrDefault(authorBook => (authorBook.AuthorId == author.AuthorId && authorBook.BookId == bookId));
#nullable disable
      // if no AuthorBook association, create one
      if (authorBookEntity == null && bookId != 0)
      {
        _db.AuthorBooks.Add(new AuthorBook { AuthorId = author.AuthorId, BookId = bookId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", "Authors", new { id = author.AuthorId });
    }

    [HttpPost]
    public ActionResult RemoveBook(int joinId, int type)
    {
      AuthorBook joinEntry = _db.AuthorBooks.FirstOrDefault(entry => entry.AuthorBookId == joinId);
      _db.AuthorBooks.Remove(joinEntry);
      _db.SaveChanges();
      switch (type)
      {
        case 0:
          return RedirectToAction("Details", new { id = joinEntry.AuthorId });
        case 1:
          return RedirectToAction("Details", "Books", new { id = joinEntry.BookId });
        default:
          return RedirectToAction("Index");
      }
      
    }
  }
}