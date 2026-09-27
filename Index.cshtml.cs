using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty(SupportsGet = true)]
    public string? CategoryFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? Month { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? Year { get; set; }

    public List<Expense> Expenses { get; set; } = new();
    public decimal Total { get; set; }
    public string PeriodLabel { get; set; } = string.Empty;
    public List<CategorySlice> Breakdown { get; set; } = new();

    public async Task OnGetAsync()
    {
        var year = Year ?? DateTime.Today.Year;
        var month = Month ?? DateTime.Today.Month;

        var periodStart = new DateTime(year, month, 1);
        var periodEnd = periodStart.AddMonths(1);
        Year = year;
        Month = month;
        PeriodLabel = periodStart.ToString("MMMM yyyy");

        var query = _db.Expenses
            .Where(e => e.Date >= periodStart && e.Date < periodEnd);

        if (!string.IsNullOrWhiteSpace(CategoryFilter))
        {
            query = query.Where(e => e.Category == CategoryFilter);
        }

        Expenses = await query.OrderByDescending(e => e.Date).ThenByDescending(e => e.Id).ToListAsync();
        Total = Expenses.Sum(e => e.Amount);

        // Breakdown always reflects the full month, regardless of the category filter,
        // so the rail stays a stable map of where the month's spending sits.
        var monthExpenses = await _db.Expenses
            .Where(e => e.Date >= periodStart && e.Date < periodEnd)
            .ToListAsync();

        var grouped = monthExpenses
            .GroupBy(e => e.Category)
            .Select(g => new CategorySlice
            {
                Category = g.Key,
                Amount = g.Sum(e => e.Amount)
            })
            .OrderByDescending(c => c.Amount)
            .ToList();

        var max = grouped.Count > 0 ? grouped.Max(g => g.Amount) : 0m;
        foreach (var slice in grouped)
        {
            slice.RelativeWidth = max > 0 ? (double)(slice.Amount / max) * 100 : 0;
        }

        Breakdown = grouped;
    }

    public class CategorySlice
    {
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public double RelativeWidth { get; set; }
    }
}
