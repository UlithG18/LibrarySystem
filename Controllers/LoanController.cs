using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Models;
using LibrarySystem.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibrarySystem.Controllers;

public class LoanController : Controller
{
    private readonly LoanService _loanService;
    private readonly UserService _userService;
    private readonly BookService _bookService;

    public LoanController(LoanService loanService, UserService userService, BookService bookService)
    {
        _loanService = loanService;
        _userService = userService;
        _bookService = bookService;
    }

    public IActionResult Index()
    {
        var response = _loanService.GetAllLoans();
        return View(response.Data);
    }

    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    [HttpPost]
    public IActionResult Store(Loan loan)
    {
        var result = _loanService.SaveLoan(loan);
        if (result.Success)
        {
            TempData["message"] = result.Message;
            return RedirectToAction("Index");
        }
        else
        {
            TempData["message"] = result.Message;
            return RedirectToAction("Create");
        }
    }

    public IActionResult Show(int id)
    {
        var result = _loanService.GetLoanById(id);
        return View(result.Data);
    }

    public IActionResult Edit(int id)
    {
        var result = _loanService.GetLoanById(id);
        PopulateDropdowns();
        return View(result.Data);
    }

    [HttpPost]
    public IActionResult Update(Loan loan)
    {
        var result = _loanService.UpdateLoan(loan);
        TempData["message"] = result.Message;
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Destroy(Loan loan)
    {
        var result = _loanService.DeleteLoan(loan);
        TempData["message"] = result.Message;
        return RedirectToAction("Index");
    }

    private void PopulateDropdowns()
    {
        ViewBag.Users = new SelectList(_userService.GetAllUsers().Data, "Id", "Name");
        ViewBag.Books = new SelectList(_bookService.GetAllBooks().Data, "Id", "Title");
    }
}