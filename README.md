# Epic Escapes Indonesia — Website (ASP.NET Core MVC)

Website tur **Epic Escapes Indonesia** (folder project masih bernama KelanaTours).
Tampilan, konten, foto, dan URL mengikuti https://epicescapes.id.

## Cara menjalankan

Butuh **.NET 10 SDK** (cek dengan `dotnet --list-sdks`).

```bash
cd KelanaTours
dotnet run --launch-profile http
```

Lalu buka **http://localhost:5203** di browser.
Di Visual Studio: buka `KelanaTours.csproj`, lalu tekan F5.

> Hanya punya .NET 8? Ubah `<TargetFramework>net10.0</TargetFramework>` menjadi `net8.0`
> di `KelanaTours.csproj`. Kodenya kompatibel.

## Halaman (URL sama dengan epicescapes.id)

| URL | Isi |
|---|---|
| `/` | Beranda: slider, Why Travelers Choose, Signature Adventures, ulasan TripAdvisor, CTA WhatsApp |
| `/about-us` | Tentang Epic Escapes |
| `/our-tours` | Semua tur |
| `/our-tours/category/{slug}` | `sharing-daily-tour`, `day-tour-`, `multi-day-tour-` |
| `/tours/{slug}` | Detail tur: galeri, itinerary, highlights, included, info penting, Book via WhatsApp |
| `/destinations`, `/destinations/{slug}` | `banyuwangi-`, `yogyakarta`, `surabaya` |
| `/search?keyword=ijen` | Pencarian tur |
| `/contact` | Form kontak → diteruskan ke WhatsApp |
| `/wishlist` | Tur yang ditandai ikon hati (disimpan di browser) |

## Mengubah konten

- **Tur** (judul, harga, durasi, itinerary, included, foto galeri): `Data/tours.json`.
  Teks itinerary/deskripsi boleh berisi HTML sederhana (`<p>`, `<ul>`, `<li>`, `<strong>`, `<br>`).
- **Kategori, destinasi, slider, keunggulan, ulasan, partner**: `Services/TourRepository.cs`.
- **Kontak, WhatsApp, sosial media**: `appsettings.json` bagian `"Site"`.
- **Warna tema**: `:root` di `wwwroot/css/site.css`.

## Gambar

Semua gambar ada di **`wwwroot/images`** (struktur folder sama dengan `images` di epicescapes.id,
misalnya `tour/...`, `tour_gallery/...`, `slider/...`). Lokasinya diatur di `appsettings.json`:
`"ImageBaseUrl": "/images/"`.

- Foto dipakai versi `.webp` (lebih ringan): path `tour/foto.png` di data → file `wwwroot/images/tour/foto.png.webp`.
- Menambah tur/foto baru: simpan file di `wwwroot/images/tour/` atau `tour_gallery/`, lalu tulis path-nya di
  `Data/tours.json`. Kalau file-nya sudah `.webp`, tulis path lengkap, misalnya `tour/foto-baru.webp`.
- `download-images.ps1` bisa dijalankan ulang untuk mengunduh gambar yang hilang dari epicescapes.id.

## Library di `wwwroot/lib`

Bootstrap 5, jQuery, Swiper (slider), Unicons + Font Awesome (ikon), flag-icons (bendera bahasa).
Bendera bahasa memakai Google Translate (butuh internet).
