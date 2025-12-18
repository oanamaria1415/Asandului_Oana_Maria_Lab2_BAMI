using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asandului_Oana_Maria_Lab2.Data;
using Asandului_Oana_Maria_Lab2.Models;

namespace Asandului_Oana_Maria_Lab2.Controllers
{
    public class BooksController : Controller
    {
        private readonly Asandului_Oana_Maria_Lab2Context _context;

        public BooksController(Asandului_Oana_Maria_Lab2Context context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["CurrentFilter"] = searchString;

            var books = from b in _context.Book
                        join a in _context.Author on b.AuthorID equals a.ID
                        join g in _context.Genre on b.GenreID equals g.ID
                        select new BookViewModel
                        {
                            ID = b.ID,
                            Title = b.Title,
                            Price = b.Price,
                            LastName = a.LastName,
                            GenreName = g.Name
                        };


            // 🔍 FILTRARE DUPĂ TITLU
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString));
            }

            // 🔃 SORTARE
            switch (sortOrder)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    books = books.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.Price);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            return View(await books.AsNoTracking().ToListAsync());
        }



        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
  .Include(s => s.Orders)
  .ThenInclude(e => e.Customer)
  .AsNoTracking()
  .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "ID", "Name");
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "LastName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,Price")] Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }


            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }

            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "ID", "Name", book.GenreID);
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "LastName", book.AuthorID);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "ID", "Name", book.GenreID);
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "LastName", book.AuthorID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Edit")]
    
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
                return NotFound();

            var bookToUpdate = await _context.Book.FirstOrDefaultAsync(b => b.ID == id);
            if (bookToUpdate == null)
                return NotFound();

            if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "",
                b => b.Title, b => b.Price, b => b.AuthorID, b => b.GenreID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists");
                }
            }

            // EXACT ca în a doua: refaci dropdown-urile
            ViewData["GenreID"] = new SelectList(_context.Set<Genre>(), "ID", "Name", bookToUpdate.GenreID);
            ViewData["AuthorID"] = new SelectList(_context.Set<Author>(), "ID", "LastName", bookToUpdate.AuthorID);

            return View(bookToUpdate);
        }


        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.ID == id);
        }
    }
}
