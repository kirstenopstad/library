using Microsoft.AspNetCore.Identity; // added to assoc users 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // added to assoc users 
using System.Security.Claims; // added to assoc users 
using Library.Models;

namespace Library.Controllers
{
  // for patrons
  public class CheckOutsController : Controller 
  {
    [Authorize]
    private readonly LibraryContext _db;
    private readonly UserManager<LibraryUser> _userManager;

    public CheckOutsController(UserManager<ApplicationUser> userManager, LibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    // Index | Async to look up in DB
    public async Task<ActionResult> Index()
      // table of all PATRON checkouts
    {
      // get userId from User (a property of CheckOutsController)
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      // get user object
      LibraryUser currentUser = await _userManager.FindByIdAsync(userId);
      // lookup checkouts associated with user
      List<CheckOut> checkOuts = _db.CheckOuts
                                    .Where(entry => entry.User.Id == currentUser.Id)
                                    .Include(copy => copy.Copy)
                                    .ThenInclude(book => book.Book)
                                    .ThenInclude(authorBooks => authorBooks.AuthorBooks)
                                    .ThenInclude(author => author.Author)
                                    .ToList();
      return View(checkOuts);
    }
    
    // Create 
    public ActionResult Create()
    {
      // List of books available
      List<Copy> availableTitles = _db.Copies
                                      // TODO: Show only distinct available copies
                                      .Where(copy => copy.IsCheckedOut == false)
                                      .Include(copy => copy.Book)
                                      .ThenInclude(book => book.AuthorBooks)
                                      .ThenInclude(authorBook => authorBook.Author)
                                      .ToList();
      ViewBag.CopyId = new SelectList(availableTitles, "CopyId", "Book.Title");
      return View();
    }
    
    // TODO: Add association with user in Create POST
    [HttpPost]
    // takes in copyId
    public ActionResult Create(int id)
    {
      // TODO: Add ModelState validation
      Copy thisCopy = _db.Copies.FirstOrDefault(copy => copy.CopyId == id);
      // TODO: add Date now to new instance of checkout { CheckOutDate = ___ }
      // TODO: add logic to add DueDate based on two weeks from now
      CheckOut newCheckout = new CheckOut { CopyId = thisCopy.CopyId, IsOverdue = false};
      _db.CheckOuts.Add(newCheckout);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    // Edit
    public ActionResult Edit(int id)
    {
      return View();
    }

    [HttpPost]
    public ActionResult Edit(CheckOut checkout)
    {
      return RedirectToAction("Index");
    }

    // Delete
    [HttpPost]
    public ActionResult Delete(int id)
    {
      return RedirectToAction("Index");
    }

  }
}