using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models;

public class Expense
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Give this expense a short description.")]
    [StringLength(120)]
    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 100000000, ErrorMessage = "Amount must be greater than zero.")]
    [Display(Name = "Amount")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [Required]
    public string Category { get; set; } = Categories.Default;

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [StringLength(500)]
    public string? Notes { get; set; }
}

public static class Categories
{
    public const string Default = "Other";

    public static readonly string[] All =
    {
        "Food & Dining",
        "Transport",
        "Housing",
        "Utilities",
        "Entertainment",
        "Shopping",
        "Health",
        "Education",
        "Other"
    };

    // Muted brass/rust palette per category dot, kept deliberately restrained.
    public static readonly Dictionary<string, string> Colors = new()
    {
        ["Food & Dining"] = "#C9A227",
        ["Transport"] = "#6E8A9C",
        ["Housing"] = "#8E6B9C",
        ["Utilities"] = "#4E8C7C",
        ["Entertainment"] = "#B5533C",
        ["Shopping"] = "#B08968",
        ["Health"] = "#5C8A5C",
        ["Education"] = "#5C7AA8",
        ["Other"] = "#8A93A0"
    };

    public static string ColorFor(string category) =>
        Colors.TryGetValue(category, out var hex) ? hex : Colors[Default];
}
