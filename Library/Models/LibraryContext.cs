using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
  public class LibraryContext : IdentityDbContext<LibraryUser>
  {
    // include DbSets as needed
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<AuthorBook> AuthorBooks { get; set; }
    public DbSet<Copy> Copies { get; set; }
    public DbSet<CheckOut> CheckOuts { get; set; }

    public LibraryContext(DbContextOptions options) : base(options) { }
  }
}