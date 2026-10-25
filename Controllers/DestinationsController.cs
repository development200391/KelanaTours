using KelanaTours.Models;
using KelanaTours.Services;
using Microsoft.AspNetCore.Mvc;

namespace KelanaTours.Controllers;

[Route("destinations")]
public class DestinationsController(ITourRepository repo) : Controller
{
    [HttpGet("")]
    public IActionResult Index() =>
        View(repo.Destinations
            .Select(d => (d, repo.Tours.Count(t => t.DestinationSlug == d.Slug)))
            .ToList());

    [HttpGet("{slug}")]
    public IActionResult Detail(string slug)
    {
        var destination = repo.FindDestination(slug);
        if (destination is null) return NotFound();

        return View(new DestinationDetailViewModel
        {
            Destination = destination,
            Tours = repo.Search(null, destinationSlug: destination.Slug).Select(repo.ToCard).ToList(),
        });
    }
}
