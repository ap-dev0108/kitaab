using kitaab.Database;
using Microsoft.AspNetCore.Mvc;
using kitaab.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace kitaab.Controller;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public BookController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("/getBooks")]
    public async Task<ActionResult<List<Book>>> GetBooks()
    {
        var checkBooks = await _db.Books.ToListAsync();

        if (checkBooks == null)
            return NoContent();

        return Ok(checkBooks);
    }

    [HttpPost("/addBook")]
    public async Task<IActionResult> AddBook([FromBody] Book books)
    {
        var checkBooks = await _db.Books.FirstOrDefaultAsync(b => b.title == books.title);

        if (checkBooks != null)
            return Conflict("Book with this title already exists");
        
        if (books.score > 10 && books.score < 0)
            return BadRequest("Please rate the book between 0-10");

        var newBook = new Book
        {
            bookId = Guid.NewGuid(),
            title = books.title,
            description = books.description,
            score =  books.score
        };

        await _db.Books.AddAsync(newBook);
        await _db.SaveChangesAsync();

        return Ok(new {Message = "Book added successfully", values = newBook});
    }

    [HttpPut("/updateBook/{id}")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromBody] Book books)
    {
        var checkBooks = await _db.Books.FirstOrDefaultAsync(b => b.bookId == id);

        if (checkBooks == null)
            return NotFound("The book with this ID cannot be found");

        checkBooks.title = books.title;
        checkBooks.description = books.description;
        checkBooks.score = books.score;

        _db.Books.Update(checkBooks);
        await _db.SaveChangesAsync();
        return Ok(checkBooks);
    }

    [HttpDelete("/deleteBook/{id}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var checkBooks = await _db.Books.FirstOrDefaultAsync(b => b.bookId == id);

        if (checkBooks == null)
            return NotFound("Book with this ID cannot be found");
        
        _db.Books.Remove(checkBooks);
        await _db.SaveChangesAsync();
        return Ok(new {Message = "Book deleted successfully"});
    }
}