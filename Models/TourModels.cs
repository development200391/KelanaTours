using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace KelanaTours.Models;

// =====================================================================
// Data website Epic Escapes Indonesia.
// Gambar ditulis sebagai path relatif dari folder "images" website,
// misalnya "tour/ijen.png". URL lengkapnya dibuat oleh SiteOptions.Img / .Webp.
// =====================================================================

/// <summary>Kategori tur (menu "Our Services"). Slug sama dengan epicescapes.id.</summary>
public record TourCategory(string Slug, string Name, string Image);

/// <summary>Destinasi tur (menu "Tour Destinations").</summary>
public record Destination(string Slug, string Name, string Image);

/// <summary>Satu slide di hero beranda.</summary>
public record HeroSlide(string SupTitle, string Title, string Description, string Image);

/// <summary>Kartu "Why Travelers Choose". Icon = path gambar ikon.</summary>
public record Feature(string Icon, string Title, string Text);

/// <summary>Ulasan tamu (diambil dari TripAdvisor).</summary>
public record Review(string Name, string Text, string Url, string? Avatar = null);

/// <summary>Logo partner di footer.</summary>
public record Partner(string Name, string Image, string? Url);

/// <summary>Header halaman dalam: foto latar + judul + breadcrumb (partial _PageHead).</summary>
public record PageHeadModel(string Title, string? SupTitle, string Image, IReadOnlyList<(string Text, string? Url)> Crumbs);

/// <summary>Satu tur. Data dibaca dari Data/tours.json.</summary>
public class Tour
{
    [JsonPropertyName("slug")] public string Slug { get; set; } = "";
    [JsonPropertyName("title")] public string Title { get; set; } = "";
    [JsonPropertyName("category")] public string CategorySlug { get; set; } = "";
    [JsonPropertyName("destination")] public string DestinationSlug { get; set; } = "";
    [JsonPropertyName("duration")] public string Duration { get; set; } = "";
    [JsonPropertyName("price")] public decimal PricePerPerson { get; set; }
    [JsonPropertyName("guideLanguages")] public string GuideLanguages { get; set; } = "";
    [JsonPropertyName("cancellation")] public string Cancellation { get; set; } = "";
    [JsonPropertyName("reserve")] public string Reserve { get; set; } = "";
    [JsonPropertyName("pickup")] public string Pickup { get; set; } = "";

    /// <summary>Foto utama, contoh "tour/ijen.png".</summary>
    [JsonPropertyName("image")] public string Image { get; set; } = "";
    [JsonPropertyName("gallery")] public List<string> Gallery { get; set; } = [];

    // Bagian berikut berisi HTML sederhana (p, ul, li, strong, br).
    [JsonPropertyName("summary")] public string SummaryHtml { get; set; } = "";
    [JsonPropertyName("itinerary")] public string ItineraryHtml { get; set; } = "";
    [JsonPropertyName("highlights")] public string HighlightsHtml { get; set; } = "";
    [JsonPropertyName("description")] public string DescriptionHtml { get; set; } = "";
    [JsonPropertyName("important")] public string ImportantHtml { get; set; } = "";

    [JsonPropertyName("included")] public List<string> Included { get; set; } = [];
    [JsonPropertyName("notIncluded")] public List<string> NotIncluded { get; set; } = [];
}

/// <summary>Data yang dibutuhkan partial _TourCard.</summary>
public record TourCardViewModel(Tour Tour, TourCategory Category, Destination Destination);

public class TourListViewModel
{
    public required string Title { get; init; }
    public string? SupTitle { get; init; }
    public required string HeroImage { get; init; }
    public TourCategory? Category { get; init; }
    public string? Query { get; init; }
    public required string BasePath { get; init; }
    public required IReadOnlyList<TourCardViewModel> Tours { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 12;
    public int TotalCount { get; init; }
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
}

public class TourDetailViewModel
{
    public required Tour Tour { get; init; }
    public required TourCategory Category { get; init; }
    public required Destination Destination { get; init; }
    public required IReadOnlyList<TourCardViewModel> RelatedTours { get; init; }
}

public class HomeViewModel
{
    public required IReadOnlyList<HeroSlide> Slides { get; init; }
    public required IReadOnlyList<Feature> Features { get; init; }
    public required IReadOnlyList<Review> Reviews { get; init; }
    public required IReadOnlyList<TourCardViewModel> Tours { get; init; }
}

public class DestinationDetailViewModel
{
    public required Destination Destination { get; init; }
    public required IReadOnlyList<TourCardViewModel> Tours { get; init; }
}

public class ContactFormModel
{
    [Required(ErrorMessage = "Please enter your name.")]
    [StringLength(80)]
    [Display(Name = "Full name")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Please enter your email.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "WhatsApp number (optional)")]
    public string? Phone { get; set; }

    [Display(Name = "Tour you are interested in")]
    public string? TourSlug { get; set; }

    [Required(ErrorMessage = "Please write your message.")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Your message should be at least 10 characters.")]
    [Display(Name = "Message")]
    public string Message { get; set; } = "";
}

/// <summary>Kontak, sosial media, dan lokasi gambar. Diisi dari appsettings.json bagian "Site".</summary>
public class SiteOptions
{
    public string BrandName { get; set; } = "Epic Escapes";
    public string BrandFullName { get; set; } = "Epic Escapes Indonesia";

    /// <summary>Nomor WhatsApp format internasional tanpa + / spasi, contoh 62811399769.</summary>
    public string WhatsAppNumber { get; set; } = "62811399769";
    public string WhatsAppGreeting { get; set; } = "Hello Epic Escapes, may I ask for more information?";
    public string PhoneDisplay { get; set; } = "(62) 811-399769";
    public string Email { get; set; } = "epicescapeindonesia@gmail.com";
    public string Address { get; set; } = "";
    public string MapUrl { get; set; } = "";
    public string Facebook { get; set; } = "#";
    public string Instagram { get; set; } = "#";
    public string TikTok { get; set; } = "#";
    public string YouTube { get; set; } = "#";
    public string TripAdvisor { get; set; } = "#";

    /// <summary>
    /// Lokasi folder gambar. Default "/images/" = folder wwwroot/images di project ini.
    /// (Bisa diganti ke "https://epicescapes.id/images/" untuk memakai gambar dari website lama.)
    /// </summary>
    public string ImageBaseUrl { get; set; } = "/images/";

    /// <summary>Pakai versi .webp (lebih ringan) untuk foto. Matikan kalau file .webp tidak ada.</summary>
    public bool UseWebp { get; set; } = true;

    /// <summary>URL gambar apa adanya (untuk ikon .png / .svg).</summary>
    public string Img(string path) =>
        string.IsNullOrEmpty(path) ? "" : ImageBaseUrl.TrimEnd('/') + "/" + path.TrimStart('/').Replace(" ", "%20");

    /// <summary>URL foto versi .webp (sama seperti epicescapes.id), kecuali UseWebp = false.</summary>
    public string Webp(string path) =>
        string.IsNullOrEmpty(path) ? "" : UseWebp && !path.EndsWith(".webp") ? Img(path) + ".webp" : Img(path);

    /// <summary>Link chat WhatsApp dengan pesan pembuka.</summary>
    public string WhatsAppUrl(string? message = null) =>
        $"https://api.whatsapp.com/send?phone={WhatsAppNumber}&text={Uri.EscapeDataString(message ?? WhatsAppGreeting)}";

    public string PhoneUrl => "tel:+" + WhatsAppNumber;
}

public static class Format
{
    private static readonly CultureInfo Id = CultureInfo.GetCultureInfo("id-ID");

    /// <summary>Format harga seperti di epicescapes.id: "IDR 350.505".</summary>
    public static string Idr(decimal amount) => "IDR " + amount.ToString("N0", Id);
}
