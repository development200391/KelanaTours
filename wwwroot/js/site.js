// =============================================================
// Kelana Tours — interaksi halaman
// 1. Wishlist (localStorage)   2. Slider Swiper   3. Header & tombol ke atas
// 4. Ganti bahasa lewat bendera (Google Translate)
// 5. Detail tur: Read more, Book via WhatsApp, Share
// =============================================================

// ---------- 1. Wishlist sederhana (hanya tersimpan di browser ini) ----------
(function () {
    const KEY = "kelana-wishlist";

    function load() {
        try { return new Set(JSON.parse(localStorage.getItem(KEY) || "[]")); }
        catch { return new Set(); }
    }

    function save(set) {
        try { localStorage.setItem(KEY, JSON.stringify([...set])); } catch { /* abaikan */ }
    }

    function render() {
        const saved = load();

        document.querySelectorAll("[data-wishlist-toggle]").forEach(btn => {
            const on = saved.has(btn.dataset.wishlistToggle);
            btn.classList.toggle("is-saved", on);
            btn.setAttribute("aria-pressed", String(on));
            const off = btn.querySelector("[data-label-off]");
            const onLabel = btn.querySelector("[data-label-on]");
            if (off && onLabel) { off.hidden = on; onLabel.hidden = !on; }
        });

        document.querySelectorAll("[data-wishlist-count]").forEach(el => {
            el.textContent = saved.size;
            el.hidden = saved.size === 0;
        });

        // Halaman /wishlist
        const items = document.querySelectorAll("[data-wishlist-item]");
        if (items.length) {
            let visible = 0;
            items.forEach(el => {
                const show = saved.has(el.dataset.wishlistItem);
                el.hidden = !show;
                if (show) visible++;
            });
            const empty = document.querySelector("[data-wishlist-empty]");
            if (empty) empty.hidden = visible > 0;
        }
    }

    document.addEventListener("click", e => {
        const btn = e.target.closest("[data-wishlist-toggle]");
        if (!btn) return;
        e.preventDefault();
        const saved = load();
        const slug = btn.dataset.wishlistToggle;
        saved.has(slug) ? saved.delete(slug) : saved.add(slug);
        save(saved);
        render();
    });

    document.addEventListener("DOMContentLoaded", render);
})();

// ---------- 2. Slider (Swiper) ----------
document.addEventListener("DOMContentLoaded", function () {
    if (typeof Swiper === "undefined") return;

    // Hero: fade, otomatis berganti, nomor slide di sisi kanan
    if (document.querySelector(".slider-home")) {
        new Swiper(".slider-home", {
            effect: "fade",
            fadeEffect: { crossFade: true },
            loop: true,
            speed: 1000,
            autoplay: { delay: 5000, disableOnInteraction: false },
            pagination: {
                el: ".slider-home .swiper-pagination",
                clickable: true,
                renderBullet: (i, cls) => `<span class="${cls}">${String(i + 1).padStart(2, "0")}</span>`,
            },
            navigation: { nextEl: ".slider-home .swiper-button-next", prevEl: ".slider-home .swiper-button-prev" },
        });
    }

    // Kartu "Kenapa memilih kami" + scrollbar
    if (document.querySelector(".layanan-swiper")) {
        new Swiper(".layanan-swiper", {
            slidesPerView: 1.15,
            spaceBetween: 20,
            grabCursor: true,
            scrollbar: { el: ".layanan-swiper .swiper-scrollbar", draggable: true },
            breakpoints: { 576: { slidesPerView: 2 }, 992: { slidesPerView: 3 }, 1200: { slidesPerView: 4.2 } },
        });
    }

    // Kartu tur
    if (document.querySelector(".tour-swiper")) {
        new Swiper(".tour-swiper", {
            slidesPerView: 1.2,
            spaceBetween: 20,
            grabCursor: true,
            navigation: { nextEl: ".tourSwiperWrapper .swiper-button-next", prevEl: ".tourSwiperWrapper .swiper-button-prev" },
            breakpoints: { 576: { slidesPerView: 2 }, 768: { slidesPerView: 3 }, 1200: { slidesPerView: 4 } },
        });
    }

    // Testimoni
    if (document.querySelector(".review-swiper")) {
        new Swiper(".review-swiper", {
            slidesPerView: 1,
            spaceBetween: 24,
            loop: true,
            autoplay: { delay: 3500, disableOnInteraction: false },
            pagination: { el: ".review-swiper .swiper-pagination", clickable: true },
            breakpoints: { 768: { slidesPerView: 2 } },
        });
    }
});

// ---------- 3. Bayangan header saat scroll & tombol kembali ke atas ----------
document.addEventListener("DOMContentLoaded", function () {
    const header = document.querySelector(".site-header");
    const wrap = document.querySelector(".progress-wrap");
    const path = wrap ? wrap.querySelector("path") : null;
    const length = path ? path.getTotalLength() : 0;
    if (path) {
        path.style.strokeDasharray = `${length} ${length}`;
        path.style.strokeDashoffset = length;
    }

    function onScroll() {
        const y = window.scrollY;
        if (header) header.classList.toggle("is-stuck", y > 40);
        if (wrap && path) {
            const max = document.documentElement.scrollHeight - window.innerHeight;
            path.style.strokeDashoffset = length - (max > 0 ? (y * length) / max : 0);
            wrap.classList.toggle("active-progress", y > 50);
        }
    }

    window.addEventListener("scroll", onScroll, { passive: true });
    onScroll();

    if (wrap) {
        const toTop = () => window.scrollTo({ top: 0, behavior: "smooth" });
        wrap.addEventListener("click", toTop);
        wrap.addEventListener("keydown", e => { if (e.key === "Enter" || e.key === " ") { e.preventDefault(); toTop(); } });
    }
});

// ---------- 4. Ganti bahasa lewat bendera (Google Translate) ----------
window.googleTranslateElementInit = function () {
    if (window.google && google.translate) {
        new google.translate.TranslateElement({ pageLanguage: "en", autoDisplay: false }, "google_translate_element");
    }
};

(function () {
    function setCookie(value) {
        const expires = value ? "" : "; expires=Thu, 01 Jan 1970 00:00:00 GMT";
        document.cookie = `googtrans=${value}${expires}; path=/`;
        document.cookie = `googtrans=${value}${expires}; path=/; domain=${location.hostname}`;
    }

    function currentLang() {
        const m = document.cookie.match(/(?:^|;\s*)googtrans=\/[^/]+\/([^;]+)/);
        return m ? decodeURIComponent(m[1]) : "en";
    }

    function changeLang(lang) {
        if (lang === "en") {
            setCookie("");
            location.reload();
            return;
        }
        setCookie(`/en/${lang}`);
        const select = document.querySelector(".goog-te-combo");
        if (select) {
            select.value = lang;
            select.dispatchEvent(new Event("change"));
            markActive(lang);
        } else {
            location.reload();
        }
    }

    function markActive(lang) {
        document.querySelectorAll(".flag-icon[data-lang]").forEach(f => f.classList.toggle("is-active", f.dataset.lang === lang));
    }

    document.addEventListener("click", e => {
        const flag = e.target.closest(".flag-icon[data-lang]");
        if (flag) changeLang(flag.dataset.lang);
    });
    document.addEventListener("keydown", e => {
        const flag = e.target.closest && e.target.closest(".flag-icon[data-lang]");
        if (flag && (e.key === "Enter" || e.key === " ")) { e.preventDefault(); changeLang(flag.dataset.lang); }
    });
    document.addEventListener("DOMContentLoaded", () => markActive(currentLang()));
})();

// ---------- 5. Ulasan & detail tur ----------
document.addEventListener("DOMContentLoaded", function () {
    // "Read more" pada kartu ulasan (teks panjang dipotong 5 baris)
    document.querySelectorAll("[data-review]").forEach(text => {
        text.classList.add("is-clamped");
        const btn = text.parentElement.querySelector("[data-review-toggle]");
        if (!btn) return;
        if (text.scrollHeight > text.clientHeight + 4) btn.hidden = false;
        btn.addEventListener("click", () => {
            const open = text.classList.toggle("is-clamped") === false;
            btn.textContent = open ? "Show less" : "Read more";
        });
    });

    // "Read more" pada itinerary / deskripsi yang panjang
    document.querySelectorAll('.rich-text[data-collapsible="true"]').forEach(box => {
        const btn = box.parentElement.querySelector("[data-read-more]");
        if (!btn || box.scrollHeight < 420) return;
        box.classList.add("is-collapsed");
        btn.hidden = false;
        btn.addEventListener("click", () => {
            const collapsed = box.classList.toggle("is-collapsed");
            btn.innerHTML = collapsed ? 'Read more <i class="uil uil-angle-down"></i>' : 'Show less <i class="uil uil-angle-up"></i>';
        });
    });

    // Tanggal minimal = hari ini
    const dateInput = document.querySelector("[data-tour-date]");
    if (dateInput) dateInput.min = new Date().toISOString().slice(0, 10);

    // Book via WhatsApp (format pesan sama seperti epicescapes.id)
    document.querySelectorAll("[data-book-whatsapp]").forEach(btn => {
        btn.addEventListener("click", () => {
            const error = document.querySelector("[data-date-error]");
            if (!dateInput || !dateInput.value) {
                if (error) error.hidden = false;
                if (dateInput) dateInput.focus();
                return;
            }
            if (error) error.hidden = true;
            const formattedDate = new Date(dateInput.value + "T00:00:00").toLocaleDateString("en-US", { day: "numeric", month: "long", year: "numeric" });
            const message =
`Hello 👋

I would like to make a booking with the following details:

📌 Tour Information
• Tour Name        : ${btn.dataset.tourTitle}
• Tour Date        : ${formattedDate}

👥 Participants
• Number of Guests : … persons

📍 Pickup Location
• Hotel / Address  : …

🔗 Tour Link
${btn.dataset.tourLink}

Thank you 😊`;
            window.open(`https://wa.me/${btn.dataset.wa}?text=${encodeURIComponent(message)}`, "_blank");
        });
    });

    // Share this tour
    document.querySelectorAll("[data-share]").forEach(btn => {
        btn.addEventListener("click", async () => {
            const url = btn.dataset.shareUrl || location.href;
            if (navigator.share) {
                try { await navigator.share({ title: btn.dataset.shareTitle || document.title, url }); } catch { /* dibatalkan */ }
                return;
            }
            try {
                await navigator.clipboard.writeText(url);
                showToast("Link copied successfully!");
            } catch {
                window.prompt("Copy this link:", url);
            }
        });
    });

    function showToast(text) {
        const el = document.createElement("div");
        el.className = "share-toast";
        el.textContent = text;
        document.body.appendChild(el);
        setTimeout(() => el.remove(), 2200);
    }
});
