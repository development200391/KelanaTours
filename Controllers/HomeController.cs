using System.Diagnostics;
using System.Text;
using KelanaTours.Models;
using KelanaTours.Services;
using Microsoft.AspNetCore.Mvc;

namespace KelanaTours.Controllers;

public class HomeController(ITourRepository repo, SiteOptions site, ILogger<HomeController> logger) : Controller
{
    [Route("")]
    public IActionResult Index()
    {
        var model = new HomeViewModel
        {
            Slides = repo.Slides,
            Features = repo.Features,
            Reviews = repo.Reviews,
            Tours = repo.Tours.Select(repo.ToCard).ToList(),
        };
        return View(model);
    }

    [Route("about-us")]
    public IActionResult About() => View(repo.Features);

    [HttpGet("contact")]
    public IActionResult Contact(string? tour)
    {
        ViewBag.Tours = repo.Tours;
        return View(new ContactFormModel { TourSlug = tour });
    }

    /// <summary>
    /// Form kontak tidak mengirim email: setelah valid, pengunjung diarahkan ke WhatsApp
    /// dengan pesan yang sudah terisi rapi.
    /// </summary>
    [HttpPost("contact")]
    [ValidateAntiForgeryToken]
    public IActionResult Contact(ContactFormModel form)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Tours = repo.Tours;
            return View(form);
        }

        var tour = string.IsNullOrEmpty(form.TourSlug) ? null : repo.FindTour(form.TourSlug);
        var text = new StringBuilder()
            .AppendLine($"Hello {site.BrandName} 👋")
            .AppendLine()
            .AppendLine($"Name : {form.Name}")
            .AppendLine($"Email : {form.Email}");
        if (!string.IsNullOrWhiteSpace(form.Phone)) text.AppendLine($"WhatsApp : {form.Phone}");
        if (tour is not null) text.AppendLine($"Tour : {tour.Title}");
        text.AppendLine().Append(form.Message);

        logger.LogInformation("Contact form from {Name} <{Email}> about {Tour}", form.Name, form.Email, tour?.Title ?? "-");
        return Redirect(site.WhatsAppUrl(text.ToString()));
    }

    [Route("wishlist")]
    public IActionResult Wishlist() => View(repo.Tours.Select(repo.ToCard).ToList());

    [Route("error/{code:int}")]
    public IActionResult StatusCodePage(int code)
    {
        ViewBag.Code = code;
        return View("NotFound");
    }

    [Route("error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
