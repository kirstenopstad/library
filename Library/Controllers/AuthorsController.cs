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
      if(!ModelState.IsValid)
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
      Author thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);
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
        return RedirectToAction("Details", "Authors", new { id = author.AuthorId});
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
  }
}