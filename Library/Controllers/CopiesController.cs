using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Library.Models;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
  [Authorize]
  public class CopiesController : Controller
  {
    private readonly LibraryContext _db;

    public CopiesController(LibraryContext db)
    {
      _db = db;
    }

    // Read (Index)
    public ActionResult Index()
    {
      List<Copy> copyList = _db.Copies
                               .Include(copy => copy.Book)
                               .ThenInclude(book => book.AuthorBooks)
                               .ThenInclude(authorBook => authorBook.Author)
                               .Where(copy => copy.IsCheckedOut == false)
                               .ToList();
      return View(copyList);
    }
    // Create – accessible via Books
    [HttpPost]
    public ActionResult Create(Book book)
    {
      Copy newCopy = new Copy { BookId = book.BookId, IsCheckedOut = false };
      _db.Copies.Add(newCopy);
      _db.SaveChanges();
      return RedirectToAction("Details", "Books", new {id = book.BookId});
    }
    // Read (Details)
    // public ActionResult Details(int id)
    // {
    //   Copy thisCopy = _db.Copies.FirstOrDefault(copy => copy.CopyId == id);
    //   return View(thisCopy);
    // }
    // Update (Edit)
    [HttpPost]
    public ActionResult Edit(int id)
    {
      // Copy thisCopy = _db.Copies.FirstOrDefault(copy => copy.CopyId == id);
      // if(thisCopy.IsCheckedOut) //true
      // {
      //   thisCopy.IsCheckedOut = false;
      // }
      // else
      // {
      //   thisCopy.IsCheckedOut = true;
      // }
      // _db.SaveChanges();
      return RedirectToAction("CheckOutCopy", "CheckOuts", new {copyId = id});
      //return RedirectToAction("Index");
    }
    [HttpPost]
    public ActionResult Delete(int id)
    {
      Copy thisCopy = _db.Copies.FirstOrDefault(copy => copy.CopyId == id);
      _db.Copies.Remove(thisCopy);
      return RedirectToAction("Index");
    }
  }
}