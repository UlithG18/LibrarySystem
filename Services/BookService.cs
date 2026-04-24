using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Responses;

namespace LibrarySystem.Services;

public class BookService
{
    private readonly MysqlDbContext _context;
    
    public BookService(MysqlDbContext context)
    {
        _context = context;
    }

    public ServiceResponse<IEnumerable<Book>> GetAllBooks()
    {
        var books = _context.books.ToList();
        return new ServiceResponse<IEnumerable<Book>>()
        {
            Success = true,
            Data = books
        };
    }

    public ServiceResponse<Book> SaveBook(Book book)
    {
        var BookExists = _context.books.FirstOrDefault(u => u.Title == book.Title);

        if (BookExists != null)
        {
            return new ServiceResponse<Book>()
            {
                Success = false,
                Data = book,
                Message = "Book already exists"
            };
        }
        
        _context.books.Add(book);
        _context.SaveChanges();
        return new ServiceResponse<Book>()
        {
            Success = true,
            Data = book,
            Message = "Book created successfully"
        };
    }

    public ServiceResponse<Book> GetBookById(int id)
    {
        var book = _context.books.FirstOrDefault(u => u.Id == id);
        if (book != null)
        {
            return new ServiceResponse<Book>()
            {
                Success = true,
                Data = book,
                Message = "Book found"
            };
        }

        return new ServiceResponse<Book>()
        {
            Success = false,
            Data = book,
            Message = "Book not found"
        };
    }

    public ServiceResponse<Book> UpdateBook(Book book)
    {
        var bookDb = _context.books.Find(book.Id);

        bookDb.Title = book.Title;
        bookDb.Author = book.Author;
        bookDb.Available = book.Available;
        _context.SaveChanges();

        return new ServiceResponse<Book>()
        {
            Success = true,
            Data = book,
            Message = "Book updated successfully"
        };
    }
    
    public ServiceResponse<Book> DeleteBook(Book book)
    {
        var bookDb = _context.books.Find(book.Id);

        _context.books.Remove(bookDb);
        _context.SaveChanges();

        return new ServiceResponse<Book>()
        {
            Success = true,
            Data = book,
            Message = "Book removed successfully"
        };
    }
    
}