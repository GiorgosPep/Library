using LibraryAppModelFirst.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAppModelFirst.Controllers;

public class LoansController : Controller
{
    public IActionResult Index()
    {
        var loans = new List<LoanViewModel>
        {
            new()
            {
                Id = 1,
                BookTitle = "Ο Καπετάν Μιχάλης",
                BorrowerName = "Γιώργος Πέππας",
                LoanDate = new DateTime(2026, 11, 1),
                ReturnDate = null
            },
            new()
            {
                Id = 2,
                BookTitle = "Εκατό χρόνια μοναξιά",
                BorrowerName = "Μαρία Παπαδοπούλου",
                LoanDate = new DateTime(2026, 10, 20),
                ReturnDate = new DateTime(2026, 10, 30)
            }
        };

        return View(loans);
    }
}