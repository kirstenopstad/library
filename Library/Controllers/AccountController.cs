using Library.Models;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Library.Controllers
{
  public class AccountController : Controller
  {
    private readonly LibraryContext _db;
    private readonly UserManager<LibraryUser> _userMgr;
    private readonly SignInManager<LibraryUser> _signInMgr;

    public AccountController (UserManager<LibraryUser> userMgr, SignInManager<LibraryUser> signInMgr, LibraryContext db)
    {
      _userMgr = userMgr;
      _signInMgr =signInMgr;
      _db = db;
    }

    public ActionResult Index()
    {
      return View();
    }
    
    public IActionResult Register()
    {
      return View();
    }
    
    [HttpPost]
    public async Task<ActionResult> Register (RegisterViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      else
      {
        // make a new user
        LibraryUser user = new LibraryUser { UserName = model.Email };
        // validate password match
        IdentityResult result = await _userMgr.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          foreach (IdentityError error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
          return View(model);
        }
      }
    }

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      else
      {
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInMgr.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          ModelState.AddModelError("", "There is something wrong with your email or username. Please try again.");
          return View(model);
        }
      }
    }
  }
}