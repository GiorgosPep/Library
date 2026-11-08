using LibraryAppModelFirst.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAppModelFirst.Controllers;

public class AuthorsController : Controller
{
    public IActionResult Index()
    {
        var authors = new List<AuthorViewModel>
        {
            new() { Id = 1, Name = "Νίκος Καζαντζάκης", Country = "Ελλάδα" },
            new() { Id = 2, Name = "Gabriel García Márquez", Country = "Κολομβία" }
        };

        return View(authors);
    }
}