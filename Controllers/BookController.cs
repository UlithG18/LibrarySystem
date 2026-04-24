using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Models;
using LibrarySystem.Services;

namespace LibrarySystem.Controllers;

public class BookController : Controller
{
    
    private readonly BookService _bookService;

    public BookController(BookService bookService)
    {
        _bookService = bookService;
    }

    public IActionResult Index()
    {
        var response = _bookService.GetAllBooks();
        return View(response.Data);
    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Store(Book book)
    {
        var newBook = _bookService.SaveBook(book);
        if (newBook.Success == true)
        {
            TempData["message"] = newBook.Message;
            return RedirectToAction("Index");
        }
        else
        {
            TempData["message"] = newBook.Message;
            return RedirectToAction("Create");
        }
    }
    
    public IActionResult Show(int id)
    {
        var result = _bookService.GetBookById(id);
        return View(result.Data);
    }
    
    
    public IActionResult Edit(int id)
    {
        var result = _bookService.GetBookById(id);
        return View(result.Data);
    }

    [HttpPost]
    public IActionResult Update(Book book)
    {
        var result = _bookService.UpdateBook(book);
        return RedirectToAction("Index");
    }
    
    [HttpPost]
    public IActionResult Destroy(Book book)
    {
        var result = _bookService.DeleteBook(book);
        TempData["message"] = result.Message;
        return RedirectToAction("Index");
    }
}