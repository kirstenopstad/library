# Library

VIEWS
## Authors
  * C 
  * R
  * U
  * D 
## Books
  * C
  * R
  * U
  * D
  ### Copies
  * Create - Books Details <form>AddCopies SpecifyAmount<form>
    --> let user specify qty of copies
  * Read - Copy -> Book.Title, IsCheckedOut
  * Update
  * Delete 
## Checkouts 
  * C - Joins AddCopy Copy
  * R - Copy.Book.Title, CheckOutDate, DueDate = CheckoutDate + 7, IsOverDue
  * U - Extend DueDate
  * Delete 

## Users [AccountController]

## Description

An app to catalog a library's books and let patrons check them out. Below are some user stories that define MVP.

### Objectives (MVP)

#### User Stories
* As a librarian, I want to __create, read, update, delete__, and list books in the catalog, so that we can keep track of our inventory.
* As a librarian, I should only be able to create, update and delete if I am __logged in__. All users should be able to have read functionality. (Hint: authorize CUD routes for books.)
* As a librarian, I want to __search__ for a book by author or title, so that I can find a book when there are a lot of books in the library.
* As a librarian, I want to __enter__ multiple authors for a book, so that I can include accurate information in my catalog. (Hint: make an authors table and a books table with a many-to-many relationship.)
* As a patron, I want to __check a book out__, so that I can take it home with me. I should only be able to do this if I am logged in.
* As a patron, I want to know __how many copies__ of a book are on the shelf, so that I can see if any are available. (Hint: make a copies table; a book should have many copies.)
* As a patron, I want to see a __history of all the books__ I checked out, so that I can look up the name of that awesome sci-fi novel I read three years ago. (Hint: make a checkouts table that is a join table between patrons and copies.)
* As a patron, I want to know when a book I checked out is __due__, so that I know when to return it.
* As a librarian, I want to see a __list of overdue books__, so that I can call up the patron who checked them out and tell them to bring them back — OR ELSE!