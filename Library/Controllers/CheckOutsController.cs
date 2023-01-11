using Microsoft.AspNetCore.Identity; // added to assoc users 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; // added to assoc users 
using System.Security.Claims; // added to assoc users 
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using System;
namespace Library.Controllers
{
  // for patrons
  [Authorize]
  public class CheckOutsController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

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
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
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

    [HttpPost]
    // takes in copyId
    public async Task<ActionResult> Create(int copyId)
    {
      // TODO: Add ModelState validation
      if (!ModelState.IsValid)
      {
        return RedirectToAction("Create");
      }
      else
      {
        Copy thisCopy = _db.Copies.FirstOrDefault(copy => copy.CopyId == copyId);
        // TODO: add Date now to new instance of checkout { CheckOutDate = ___ }
        // TODO: add logic to add DueDate based on two weeks from now
        if (!thisCopy.IsCheckedOut)
        {
          // build newCheckout
          CheckOut newCheckout = new CheckOut { CopyId = copyId, IsOverdue = false };
          // lookup user id from db
          string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          // get user obj
          ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
          // change copy bool to checked out
          thisCopy.IsCheckedOut = true;
          _db.Copies.Update(thisCopy);
          // assign user to checkout
          newCheckout.User = currentUser;
          // add newcheckout to db
          _db.CheckOuts.Add(newCheckout);
          // save db
          _db.SaveChanges();

        }
        // go back to checkouts index
        return RedirectToAction("Index");
      }
    }

    public async Task<ActionResult> CheckOutCopy(int copyId)
    {
      // TODO: Add ModelState validation
      if (!ModelState.IsValid)
      {
        return RedirectToAction("Create");
      }
      else
      {
        Copy thisCopy = _db.Copies.FirstOrDefault(copy => copy.CopyId == copyId);
        // TODO: add Date now to new instance of checkout { CheckOutDate = ___ }
        // TODO: add logic to add DueDate based on two weeks from now
        if (!thisCopy.IsCheckedOut)
        {
          // build newCheckout
          CheckOut newCheckout = new CheckOut { CopyId = copyId, IsOverdue = false };
          // lookup user id from db
          string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          // get user obj
          ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
          // change copy bool to checked out
          thisCopy.IsCheckedOut = true;
          _db.Copies.Update(thisCopy);
          // assign user to checkout
          newCheckout.User = currentUser;
          // add newcheckout to db
          _db.CheckOuts.Add(newCheckout);
          // save db
          _db.SaveChanges();

        }
        // go back to checkouts index
        return RedirectToAction("Index");
      }
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