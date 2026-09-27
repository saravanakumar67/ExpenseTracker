# Ledger — an ASP.NET Core expense tracker

A small, self-contained expense tracker built with ASP.NET Core Razor Pages
(.NET 10) and EF Core + SQLite. No external services, no accounts — it runs
entirely on your machine and keeps its data in a local SQLite file.

## Features

- Add, edit, and delete expenses (description, amount, category, date, notes)
- Month-by-month view with previous/next navigation
- Filter the ledger by category
- Running total for the month and a per-category breakdown
- SQLite database created automatically on first run — nothing to configure

## Requirements

- .NET 10 SDK (you already have this)

## Run it

```bash
cd ExpenseTracker
dotnet restore
dotnet run
```

Then open the URL printed in the console (by default
`http://localhost:5080`). The `dotnet run` profile also launches your
browser automatically.

The first time it runs, it creates `expenses.db` in the project folder and
sets up the schema — no migrations to run by hand.

## Project layout

```
ExpenseTracker/
├── Program.cs               # app startup, DB setup
├── Data/AppDbContext.cs      # EF Core DbContext
├── Models/Expense.cs         # Expense entity + category list/colors
├── Pages/
│   ├── Index.cshtml(.cs)     # ledger view, filters, monthly summary
│   ├── Create.cshtml(.cs)    # add expense form
│   ├── Edit.cshtml(.cs)      # edit expense form
│   ├── Delete.cshtml(.cs)    # delete confirmation
│   └── Shared/_Layout.cshtml # page chrome
└── wwwroot/css/site.css      # styling
```

## Notes

- Categories are a fixed list in `Models/Expense.cs` (`Categories.All`) —
  add or rename entries there if you want different categories; give new
  ones a color in `Categories.Colors` too.
- The connection string lives in `appsettings.json`
  (`ConnectionStrings:DefaultConnection`) if you'd rather point it at a
  different file or a different provider.
