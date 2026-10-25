# =====================================================================
#  download-images.ps1
#  Mengunduh semua gambar yang dipakai website dari https://epicescapes.id/images/
#  ke folder wwwroot\images, lalu mengubah appsettings.json supaya website
#  memakai gambar lokal ("ImageBaseUrl": "/images/").
#
#  Cara pakai: klik kanan file ini -> "Run with PowerShell"
#  atau di terminal:  powershell -ExecutionPolicy Bypass -File .\download-images.ps1
#  Aman dijalankan ulang: file yang sudah ada akan dilewati.
# =====================================================================

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

$base   = 'https://epicescapes.id/images/'
$root   = Split-Path -Parent $MyInvocation.MyCommand.Path
$target = Join-Path $root 'wwwroot\images'

$files = @(
    'destination/banyuwangi--0612202513262084.png.webp',
    'destination/surabaya-0612202513280477.png.webp',
    'destination/yogyakarta-0612202513271015.png.webp',
    'keunggulan/small/247-customer-support-1809202515390062.png.webp',
    'keunggulan/small/authentic-cultural-encounters-180920251537094.png.webp',
    'keunggulan/small/expert-local-guides-1809202515352413.png.webp',
    'keunggulan/small/flexible-packages-1809202515382535.png.webp',
    'keunggulan/small/personalized-itineraries-1809202515340321.png.webp',
    'keunggulan/small/seamless-travel-experience-180920251536178.png.webp',
    'modul/28logo epicescapes.png.webp',
    'modul/56logo epicescapes.png.webp',
    'page/-0512202515104349.png.webp',
    'page/text--image-0412202512231019.png.webp',
    'page/title--image-0412202512170459.png.webp',
    'page/title--image-0412202516504354.png.webp',
    'page/title--image-0512202511140249.png.webp',
    'page/title--image-cta-0412202511113844.png.webp',
    'partner/getyourguide-1909202512534857.png.webp',
    'partner/tab-travel-1909202512510393.png.webp',
    'partner/tripadvisor-1909202512451839.png.webp',
    'partner/visiting-jogja-1612202510115338.png.webp',
    'partner/wonderful-indonesia-1909202512490110.png.webp',
    'slider/travel-smart-travel-together-0812202515272769.png.webp',
    'slider/uncover-timeless-heritage-0212202516393711.png.webp',
    'slider/witness-the-magic-of-nature-0212202516385335.png.webp',
    'slider/your-journey-our-expertise-021220251640519.png.webp',
    'testimoni/rina-a-23.jpeg.webp',
    'tour/banyuwangi-alas-purwo-national-park--gland-beach-tour-26.png.webp',
    'tour/banyuwangibali-ijen-volcano-trip-with-professional-guide-39.png.webp',
    'tour/from-malangsurabaya-2days-tour-mt-bromo-ijen-blue-flames-3.png.webp',
    'tour/from-surabaya-sunrise-or-sunset-mount-bromo-tour-44.png.webp',
    'tour/from-yogyakarta--3days-bromoijen-vulcano-drop-bali-hotel-49.png.webp',
    'tour/gilimanuk-port--hotel-bali-transfer-with-options-15.png.webp',
    'tour/ijen-volcano-private-morning-hiking-with-pickup--all-fees-46.png.webp',
    'tour/pangkalan-bun-in-borneo-3-day-jungle-cruise--orangutan-tour-92.png.webp',
    'tour/pangkalan-bun-tanjung-puting-orangutan-day-tour-by-speedboat-8.png.webp',
    'tour/the-authentic-sunset-of-baluran-national-park-banyuwangi-17.png.webp',
    'tour/yogyakarta--explore-hidden-temple-and-prambanan-66.png.webp',
    'tour/yogyakarta-borobudur-sunrise-climbup-and-prambanan-tour-14.png.webp',
    'tour/yogyakarta-mt-merbabu-1-day-hike--camping-options-35.png.webp',
    'tour/yogyakarta-walk-sightseeing-and-local-food-guided-tour-17.png.webp',
    'tour_gallery/banyuwangi-alas-purwo-national-park--gland-beach-tour3620251208_095516.png.webp',
    'tour_gallery/banyuwangi-alas-purwo-national-park--gland-beach-tour5920251208_095451.png.webp',
    'tour_gallery/banyuwangi-alas-purwo-national-park--gland-beach-tour6920251208_095443.png.webp',
    'tour_gallery/banyuwangi-alas-purwo-national-park--gland-beach-tour8620251208_095504.png.webp',
    'tour_gallery/banyuwangibali-ijen-volcano-trip-with-professional-guide020251204_153954.png.webp',
    'tour_gallery/banyuwangibali-ijen-volcano-trip-with-professional-guide320251204_153945.png.webp',
    'tour_gallery/banyuwangibali-ijen-volcano-trip-with-professional-guide6420251204_154013.png.webp',
    'tour_gallery/from-malangsurabaya-2days-tour-mt-bromo-ijen-blue-flames1220251208_105820.png.webp',
    'tour_gallery/from-malangsurabaya-2days-tour-mt-bromo-ijen-blue-flames4220251208_105808.png.webp',
    'tour_gallery/from-malangsurabaya-2days-tour-mt-bromo-ijen-blue-flames5720251208_105748.png.webp',
    'tour_gallery/from-malangsurabaya-2days-tour-mt-bromo-ijen-blue-flames8720251208_105757.png.webp',
    'tour_gallery/from-surabaya-sunrise-or-sunset-mount-bromo-tour2720251208_101008.png.webp',
    'tour_gallery/from-surabaya-sunrise-or-sunset-mount-bromo-tour8220251208_101018.png.webp',
    'tour_gallery/from-surabaya-sunrise-or-sunset-mount-bromo-tour8220251208_101029.png.webp',
    'tour_gallery/from-yogyakarta--3days-bromoijen-vulcano-drop-bali-hotel4320251208_093909.png.webp',
    'tour_gallery/from-yogyakarta--3days-bromoijen-vulcano-drop-bali-hotel5020251208_093844.png.webp',
    'tour_gallery/from-yogyakarta--3days-bromoijen-vulcano-drop-bali-hotel8620251208_093854.png.webp',
    'tour_gallery/from-yogyakarta--3days-bromoijen-vulcano-drop-bali-hotel8820251208_093925.png.webp',
    'tour_gallery/gilimanuk-port--hotel-bali-transfer-with-options1720251208_113539.png.webp',
    'tour_gallery/gilimanuk-port--hotel-bali-transfer-with-options6620251208_113507.png.webp',
    'tour_gallery/gilimanuk-port--hotel-bali-transfer-with-options8820251208_113528.png.webp',
    'tour_gallery/gilimanuk-port--hotel-bali-transfer-with-options9920251208_113516.png.webp',
    'tour_gallery/ijen-volcano-private-morning-hiking-with-pickup--all-fees3520251208_115450.png.webp',
    'tour_gallery/ijen-volcano-private-morning-hiking-with-pickup--all-fees3720251208_115459.png.webp',
    'tour_gallery/ijen-volcano-private-morning-hiking-with-pickup--all-fees7820251208_115520.png.webp',
    'tour_gallery/ijen-volcano-private-morning-hiking-with-pickup--all-fees8020251208_115510.png.webp',
    'tour_gallery/pangkalan-bun-in-borneo-3-day-jungle-cruise--orangutan-tour3120251208_102340.png.webp',
    'tour_gallery/pangkalan-bun-in-borneo-3-day-jungle-cruise--orangutan-tour5020251208_102329.png.webp',
    'tour_gallery/pangkalan-bun-in-borneo-3-day-jungle-cruise--orangutan-tour6120251208_102320.png.webp',
    'tour_gallery/pangkalan-bun-in-borneo-3-day-jungle-cruise--orangutan-tour8720251208_102353.png.webp',
    'tour_gallery/pangkalan-bun-tanjung-puting-orangutan-day-tour-by-speedboat1120251208_120531.png.webp',
    'tour_gallery/pangkalan-bun-tanjung-puting-orangutan-day-tour-by-speedboat120251208_120521.png.webp',
    'tour_gallery/pangkalan-bun-tanjung-puting-orangutan-day-tour-by-speedboat4320251208_120504.png.webp',
    'tour_gallery/pangkalan-bun-tanjung-puting-orangutan-day-tour-by-speedboat8120251208_120512.png.webp',
    'tour_gallery/the-authentic-sunset-of-baluran-national-park-banyuwangi1220251206_114154.png.webp',
    'tour_gallery/the-authentic-sunset-of-baluran-national-park-banyuwangi3320251208_124423.png.webp',
    'tour_gallery/the-authentic-sunset-of-baluran-national-park-banyuwangi5920251208_124434.png.webp',
    'tour_gallery/yogyakarta--explore-hidden-temple-and-prambanan4120251208_122056.png.webp',
    'tour_gallery/yogyakarta--explore-hidden-temple-and-prambanan420251208_122037.png.webp',
    'tour_gallery/yogyakarta--explore-hidden-temple-and-prambanan8220251208_122046.png.webp',
    'tour_gallery/yogyakarta-borobudur-sunrise-climbup-and-prambanan-tour4220251208_104005.png.webp',
    'tour_gallery/yogyakarta-borobudur-sunrise-climbup-and-prambanan-tour5220251208_103942.png.webp',
    'tour_gallery/yogyakarta-borobudur-sunrise-climbup-and-prambanan-tour7620251208_103932.png.webp',
    'tour_gallery/yogyakarta-borobudur-sunrise-climbup-and-prambanan-tour9420251208_103952.png.webp',
    'tour_gallery/yogyakarta-mt-merbabu-1-day-hike--camping-options7320251208_112739.png.webp',
    'tour_gallery/yogyakarta-mt-merbabu-1-day-hike--camping-options7720251208_112759.png.webp',
    'tour_gallery/yogyakarta-mt-merbabu-1-day-hike--camping-options8120251208_112809.png.webp',
    'tour_gallery/yogyakarta-mt-merbabu-1-day-hike--camping-options9020251208_112748.png.webp',
    'tour_gallery/yogyakarta-walk-sightseeing-and-local-food-guided-tour1220251208_123344.png.webp',
    'tour_gallery/yogyakarta-walk-sightseeing-and-local-food-guided-tour2020251208_123320.png.webp',
    'tour_gallery/yogyakarta-walk-sightseeing-and-local-food-guided-tour5420251208_123311.png.webp',
    'tour_gallery/yogyakarta-walk-sightseeing-and-local-food-guided-tour7420251208_123333.png.webp',
    'tour_kategori/day-tour--4.png.webp',
    'tour_kategori/multi-day-tour--74.png.webp',
    'tour_kategori/sharing-daily-tour-82.png.webp',
    'apple-touch-icon.png',
    'favicon-96x96.png',
    'favicon.svg',
    'money-back.png',
    'suitcase.png',
    'time.png',
    'tour-guide.png',
    'trip-advisor-icon.svg',
    'trip-advisor-star-full.svg'
)

$ok = 0
$skipped = 0
$failed = @()
$i = 0

Write-Host "Mengunduh $($files.Count) gambar ke $target" -ForegroundColor Cyan

foreach ($f in $files) {
    $i++
    $local = Join-Path $target ($f -replace '/', '\')
    $dir = Split-Path -Parent $local
    if (-not (Test-Path -LiteralPath $dir)) { New-Item -ItemType Directory -Path $dir -Force | Out-Null }

    if ((Test-Path -LiteralPath $local) -and ((Get-Item -LiteralPath $local).Length -gt 0)) {
        $skipped++
        continue
    }

    $url = $base + ($f -replace ' ', '%20')
    try {
        Invoke-WebRequest -Uri $url -OutFile $local -UseBasicParsing
        $ok++
        Write-Host ("[{0}/{1}] OK     {2}" -f $i, $files.Count, $f)
    }
    catch {
        if (Test-Path -LiteralPath $local) { Remove-Item -LiteralPath $local -Force }
        $failed += $f
        Write-Host ("[{0}/{1}] GAGAL  {2}" -f $i, $files.Count, $f) -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "Berhasil: $ok   Sudah ada: $skipped   Gagal: $($failed.Count)" -ForegroundColor Cyan

if ($failed.Count -eq 0) {
    $cfg = Join-Path $root 'appsettings.json'
    $json = Get-Content -LiteralPath $cfg -Raw -Encoding UTF8
    $json = $json -replace '"ImageBaseUrl"\s*:\s*"[^"]*"', '"ImageBaseUrl": "/images/"'
    [IO.File]::WriteAllText($cfg, $json, (New-Object Text.UTF8Encoding $false))
    Write-Host 'Selesai. appsettings.json sudah diubah ke "ImageBaseUrl": "/images/".' -ForegroundColor Green
    Write-Host 'Jalankan ulang website (Ctrl+C lalu dotnet run) supaya memakai gambar lokal.' -ForegroundColor Green
}
else {
    Write-Host 'Ada gambar yang gagal diunduh. Cek koneksi internet lalu jalankan script ini lagi.' -ForegroundColor Yellow
    Write-Host 'appsettings.json belum diubah, website masih memakai gambar dari epicescapes.id.' -ForegroundColor Yellow
}

Write-Host ""
Read-Host 'Tekan Enter untuk menutup'
