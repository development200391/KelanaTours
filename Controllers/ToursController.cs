using KelanaTours.Models;
using KelanaTours.Services;
using Microsoft.AspNetCore.Mvc;

namespace KelanaTours.Controllers;

// URL dibuat sama dengan epicescapes.id:
//   /our-tours                        semua tur
//   /our-tours/category/{slug}        tur per kategori
//   /tours/{slug}                     detail tur
//   /search?keyword=ijen              pencarian
public class ToursController(ITourRepository repo) : Controller
{
    private const int PageSize = 12;
    private const string DefaultHero = "page/title--image-0412202516504354.png";

    [HttpGet("our-tours")]
    public IActionResult Index(int page = 1) =>
        View("List", BuildList(repo.Tours, page, "Our Tours", "Signature Adventures", DefaultHero, "/our-tours", null, null));

    [HttpGet("our-tours/category/{slug}")]
    public IActionResult Category(string slug, int page = 1)
    {
        var category = repo.FindCategory(slug);
        if (category is null) return NotFound();

        var tours = repo.Search(null, categorySlug: category.Slug);
        return View("List", BuildList(tours, page, category.Name, "Our Services", category.Image,
            $"/our-tours/category/{category.Slug}", category, null));
    }

    [HttpGet("search")]
    public IActionResult Search(string? keyword, int page = 1)
    {
        var tours = repo.Search(keyword);
        var title = string.IsNullOrWhiteSpace(keyword) ? "Find Your Perfect Tour" : $"Search results for \"{keyword}\"";
        return View("List", BuildList(tours, page, title, "Search", DefaultHero, "/search", null, keyword));
    }

    [HttpGet("tours/{slug}")]
    public IActionResult Detail(string slug)
    {
        var tour = repo.FindTour(slug);
        if (tour is null) return NotFound();

        var related = repo.Tours
            .Where(t => t.Slug != tour.Slug)
            .OrderByDescending(t => t.DestinationSlug == tour.DestinationSlug)
            .ThenByDescending(t => t.CategorySlug == tour.CategorySlug)
            .Take(8)
            .Select(repo.ToCard)
            .ToList();

        var card = repo.ToCard(tour);
        return View(new TourDetailViewModel
        {
            Tour = tour,
            Category = card.Category,
            Destination = card.Destination,
            RelatedTours = related,
        });
    }

    private TourListViewModel BuildList(IReadOnlyList<Tour> tours, int page, string title, string? supTitle,
        string heroImage, string basePath, TourCategory? category, string? query)
    {
        var totalPages = Math.Max(1, (int)Math.Ceiling(tours.Count / (double)PageSize));
        page = Math.Clamp(page, 1, totalPages);

        return new TourListViewModel
        {
            Title = title,
            SupTitle = supTitle,
            HeroImage = heroImage,
            BasePath = basePath,
            Category = category,
            Query = query,
            Page = page,
            PageSize = PageSize,
            TotalCount = tours.Count,
            Tours = tours.Skip((page - 1) * PageSize).Take(PageSize).Select(repo.ToCard).ToList(),
        };
    }
}
