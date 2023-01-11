using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Library.Models;

namespace Library.Models
{
  public class Copy
  {
    public int CopyId {get;set;}
    public int BookId {get;set;}
    public Book Book {get;set;}
    // TODO: set default to false
    public bool IsCheckedOut {get;set;}
  }
}
// book
  // copies
// author
// authorBooks
// checkout