using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Responses;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Services;

public class LoanService
{
    private readonly MysqlDbContext _context;

    public LoanService(MysqlDbContext context)
    {
        _context = context;
    }

    public ServiceResponse<IEnumerable<Loan>> GetAllLoans()
    {
        var loans = _context.loans
            .Include(l => l.User)  
            .Include(l => l.Book)  
            .ToList();

        return new ServiceResponse<IEnumerable<Loan>>()
        {
            Success = true,
            Data = loans
        };
    }

    public ServiceResponse<Loan> GetLoanById(int id)
    {
        var loan = _context.loans
            .Include(l => l.User)
            .Include(l => l.Book)
            .FirstOrDefault(l => l.Id == id);

        if (loan != null)
        {
            return new ServiceResponse<Loan>()
            {
                Success = true,
                Data = loan,
                Message = "Loan found"
            };
        }

        return new ServiceResponse<Loan>()
        {
            Success = false,
            Data = loan,
            Message = "Loan not found"
        };
    }

    public ServiceResponse<Loan> SaveLoan(Loan loan)
    {
        var loanExists = _context.loans
            .FirstOrDefault(l => l.UserId == loan.UserId && l.BookId == loan.BookId);

        if (loanExists != null)
        {
            return new ServiceResponse<Loan>()
            {
                Success = false,
                Data = loan,
                Message = "This user already has this book on loan"
            };
        }

        loan.CreatedAt = DateTime.Now;
        _context.loans.Add(loan);
        _context.SaveChanges();

        return new ServiceResponse<Loan>()
        {
            Success = true,
            Data = loan,
            Message = "Loan created successfully"
        };
    }

    public ServiceResponse<Loan> UpdateLoan(Loan loan)
    {
        var loanExists = _context.loans
            .FirstOrDefault(l => l.UserId == loan.UserId && l.BookId == loan.BookId && l.Id != loan.Id);

        if (loanExists != null)
        {
            return new ServiceResponse<Loan>()
            {
                Success = false,
                Data = loan,
                Message = "This user already has this book on loan"
            };
        }

        var loanDb = _context.loans.Find(loan.Id);

        loanDb.UserId = loan.UserId;
        loanDb.BookId = loan.BookId;
        _context.SaveChanges();

        return new ServiceResponse<Loan>()
        {
            Success = true,
            Data = loan,
            Message = "Loan updated successfully"
        };
    }

    public ServiceResponse<Loan> DeleteLoan(Loan loan)
    {
        var loanDb = _context.loans.Find(loan.Id);

        _context.loans.Remove(loanDb);
        _context.SaveChanges();

        return new ServiceResponse<Loan>()
        {
            Success = true,
            Data = loan,
            Message = "Loan removed successfully"
        };
    }
}