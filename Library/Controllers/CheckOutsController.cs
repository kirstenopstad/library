using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Library.Models;

namespace Library.Controllers
{
  // for patrons
  public class CheckOutsController : Controller 
  {
    private readonly LibraryContext _db;
    public CheckOutsController(LibraryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      
      List<CheckOut> checkOuts = _db.CheckOuts.Include(copy => copy.Copy).ThenInclude(book => book.Book).ThenInclude(authorBooks => authorBooks.AuthorBooks).ThenInclude(author => author.Author).ToList();
      return View(checkOuts);
    }
    // Index
      // table of all PATRON checkouts
    // Create 
      // List of copies if available
      // Checkout Date
    // Edit
    // Delete
  }
}