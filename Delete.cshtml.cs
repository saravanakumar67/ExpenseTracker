using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseTracker.Pages;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _db;

    public DeleteModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public Expense Expense { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var expense = await _db.Expenses.FindAsync(id);
        if (expense is null)
        {
            return RedirectToPage("/Index");
        }

        Expense = expense;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var expense = await _db.Expenses.FindAsync(id);
        if (expense is not null)
        {
            _db.Expenses.Remove(expense);
            await _db.SaveChangesAsync();
        }

        return RedirectToPage("/Index");
    }
}
