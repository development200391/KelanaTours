using System.Text.Json;
using KelanaTours.Models;

namespace KelanaTours.Services;

public interface ITourRepository
{
    IReadOnlyList<TourCategory> Categories { get; }
    IReadOnlyList<Destination> Destinations { get; }
    IReadOnlyList<Tour> Tours { get; }
    IReadOnlyList<HeroSlide> Slides { get; }
    IReadOnlyList<Feature> Features { get; }
    IReadOnlyList<Review> Reviews { get; }
    IReadOnlyList<Partner> Partners { get; }
    TourCategory? FindCategory(string slug);
    Destination? FindDestination(string slug);
    Tour? FindTour(string slug);
    TourCardViewModel ToCard(Tour tour);
    IReadOnlyList<Tour> Search(string? query, string? categorySlug = null, string? destinationSlug = null);
}

/// <summary>
/// Konten website Epic Escapes Indonesia.
/// - Daftar tur (judul, harga, itinerary, dll) ada di Data/tours.json.
/// - Kategori, destinasi, slider, keunggulan, ulasan, dan partner ada di file ini.
/// </summary>
public class TourRepository : ITourRepository
{
    public TourRepository(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "Data", "tours.json");
        using var stream = File.OpenRead(path);
        Tours = JsonSerializer.Deserialize<List<Tour>>(stream) ?? [];
    }

    public IReadOnlyList<Tour> Tours { get; }

    public IReadOnlyList<TourCategory> Categories { get; } =
    [
        new("sharing-daily-tour", "Sharing Daily Tour", "tour_kategori/sharing-daily-tour-82.png"),
        new("day-tour-", "Day Tour", "tour_kategori/day-tour--4.png"),
        new("multi-day-tour-", "Multi Day Tour", "tour_kategori/multi-day-tour--74.png"),
    ];

    public IReadOnlyList<Destination> Destinations { get; } =
    [
        new("banyuwangi-", "Banyuwangi", "destination/banyuwangi--0612202513262084.png"),
        new("yogyakarta", "Yogyakarta", "destination/yogyakarta-0612202513271015.png"),
        new("surabaya", "Surabaya", "destination/surabaya-0612202513280477.png"),
    ];

    public IReadOnlyList<HeroSlide> Slides { get; } =
    [
        new("Witness the Magic of Nature", "Explore the Legendary Blue Fire of Ijen",
            "Experience one of the world’s rarest natural wonders with our guided Ijen Crater tour. Trek under the stars, witness the electrifying blue flame phenomenon, and greet the sunrise above a turquoise acid lake. A journey designed for adventurers seeking unforgettable moments.",
            "slider/witness-the-magic-of-nature-0212202516385335.png"),
        new("Uncover Timeless Heritage", "Borobudur Sunrise Experience",
            "Enjoy the golden sunrise over the world’s largest Buddhist temple. Walk through ancient reliefs, discover cultural treasures, and feel the serene spiritual atmosphere that makes Borobudur a UNESCO icon. Perfect for history lovers and culture seekers.",
            "slider/uncover-timeless-heritage-0212202516393711.png"),
        new("Travel Smart, Travel Together", "Comfortable & Affordable Sharing Tours",
            "Join small-group adventures to explore top Yogyakarta and East Java attractions at the best price. Meet new friends, enjoy curated itineraries, and discover stunning destinations effortlessly with our experienced guides.",
            "slider/travel-smart-travel-together-0812202515272769.png"),
        new("Your Journey, Our Expertise", "Multi-Day Trips Across Java",
            "From volcano trekking to cultural exploration, our multi-day packages offer a seamless travel experience across Java Island. Enjoy well-organized routes, professional service, and handpicked accommodations for a truly memorable adventure.",
            "slider/your-journey-our-expertise-021220251640519.png"),
    ];

    public IReadOnlyList<Feature> Features { get; } =
    [
        new("keunggulan/small/personalized-itineraries-1809202515340321.png", "Expert Local Guides",
            "Experience each destination with knowledgeable local experts who bring stories, culture, and hidden gems to life."),
        new("keunggulan/small/expert-local-guides-1809202515352413.png", "Premium Customized Tours",
            "Every itinerary is thoughtfully crafted to match your travel style, comfort needs, and adventure level."),
        new("keunggulan/small/seamless-travel-experience-180920251536178.png", "Best Price & Value",
            "Enjoy competitive pricing with exceptional service quality, ensuring you get the best experience for every moment spent."),
        new("keunggulan/small/authentic-cultural-encounters-180920251537094.png", "Trusted by Global Travelers",
            "Thousands of international travelers trust Epic Escapes Indonesia for safe, seamless, and memorable journeys."),
        new("keunggulan/small/flexible-packages-1809202515382535.png", "Easy & Secure Booking",
            "With a reliable payment gateway and transparent policies, booking your adventure is fast, safe, and hassle-free."),
        new("keunggulan/small/247-customer-support-1809202515390062.png", "Focus on Blue Fire Ijen",
            "As specialists in the iconic Blue Fire Ijen tour, we deliver the most complete, safe, and unforgettable volcano experience."),
    ];

    private const string TripAdvisorReviews =
        "https://www.tripadvisor.com/Attraction_Review-g14782503-d25338651-Reviews-Yogyakarta_Trip_with_Nasir_Anang-Yogyakarta_Yogyakarta_Region_Java.html";

    public IReadOnlyList<Review> Reviews { get; } =
    [
        new("HappyGoAround88",
            "This 7-day tour has exceeded our expectations with spectacular scenery and a professional tour guide. Nasir had been extremely attentive, thoughtful, and helpful for the entire trip. At the end of the tour, Nasir even went out the way to help us load the luggage on the ferry and stayed with us until we arrived in Bali before we bid farewell. Nasir always maintained a positive attitude. His humorous personality had added more fun to the long hour of travel between cities.",
            TripAdvisorReviews),
        new("Stefano c",
            "Nasir is the perfect guide for a trip in Indonesia! He is always funny and ready to help everybody, keeping smiling! Very good driver and respectful during our journey. Definetely recommended, do not hesitate to contact him for organizing your beautiful experiences",
            TripAdvisorReviews),
        new("Voyager33476229631",
            "We had a great time, Nasir was the perfect guide! We booked the tour through our hostel in Yogyakarta the day before and paid in advance, which was really convenient - it should be possible to book it through Getyourguide aswell! We were picked up at our hostel in the morning, along with two other people. The group was small and intimate but we really enjoyed it, especially as Nasir provided a very comfortable air, contributing to keeping the conversation going. He was easy to talk to and we could ask him anything regarding the two temples or Indonesia in general! In the end we were driven back to our hostel and dropped off there. The drive and tour were easy to understand and comfortable, we have absolutely no complaints.",
            TripAdvisorReviews, "testimoni/rina-a-23.jpeg"),
        new("MollyB",
            "Incredible, once of a lifetime roadtrip across Java, be prepared for some amazing hikes through beautiful national parks, the itenary for all 3 days was well thought out and everything ran smoothly. I saw some stunning sunrises and blue flame lava, and had panoramic views of the volcanic landscape. Accomodation was clean and comfortable and the local guide at Mount Ijen helped us avoid the crowds and provided a positive upbeat experience. My driver and guide Nasir Anang is a fountain of local knowledge and made me feel completly comfortable. He was warm, friendly and adapted the shecdule to my preference. He was able to answer all my questions about Indonesian culture and made great suggestions to try local food. I felt in safe hands and was able to relax and enjoy as he took care of all the tickets and accomodation bookings. It was like traveling with a dear friend and I hope to travel again with him in future. Although the price may seem high so much is included it's excellent value!",
            TripAdvisorReviews),
    ];

    public IReadOnlyList<Partner> Partners { get; } =
    [
        new("TAB Travel", "partner/tab-travel-1909202512510393.png", "https://business.tab.travel/"),
        new("GetYourGuide", "partner/getyourguide-1909202512534857.png", "https://www.getyourguide.com/"),
        new("Tripadvisor", "partner/tripadvisor-1909202512451839.png", "https://www.tripadvisor.com/"),
        new("Wonderful Indonesia", "partner/wonderful-indonesia-1909202512490110.png", "https://wonderfulindonesia.co.id/"),
        new("Visiting Jogja", "partner/visiting-jogja-1612202510115338.png", null),
    ];

    public TourCategory? FindCategory(string slug) =>
        Categories.FirstOrDefault(c => c.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

    public Destination? FindDestination(string slug) =>
        Destinations.FirstOrDefault(d => d.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

    public Tour? FindTour(string slug) =>
        Tours.FirstOrDefault(t => t.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

    public TourCardViewModel ToCard(Tour tour) =>
        new(tour,
            FindCategory(tour.CategorySlug) ?? new TourCategory(tour.CategorySlug, tour.CategorySlug, ""),
            FindDestination(tour.DestinationSlug) ?? new Destination(tour.DestinationSlug, tour.DestinationSlug, ""));

    public IReadOnlyList<Tour> Search(string? query, string? categorySlug = null, string? destinationSlug = null)
    {
        IEnumerable<Tour> result = Tours;

        if (!string.IsNullOrWhiteSpace(categorySlug))
            result = result.Where(t => t.CategorySlug == categorySlug);

        if (!string.IsNullOrWhiteSpace(destinationSlug))
            result = result.Where(t => t.DestinationSlug == destinationSlug);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var q = query.Trim();
            result = result.Where(t =>
                t.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                t.SummaryHtml.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                (FindDestination(t.DestinationSlug)?.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (FindCategory(t.CategorySlug)?.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        return result.ToList();
    }
}
