// NBC Exchange Rate App JavaScript

// Determine API base URL (works whether hosted inside ASP.NET Core or opened standalone)
const API_BASE_URL = window.location.origin.startsWith("http")
  ? window.location.origin
  : "http://localhost:5000";

const flagMap = {
  USD: "🇺🇸",
  KHR: "🇰🇭",
  THB: "🇹🇭",
  EUR: "🇪🇺",
  JPY: "🇯🇵",
  GBP: "🇬🇧",
  CNY: "🇨🇳",
  CNH: "🇨🇳",
  SGD: "🇸🇬",
  AUD: "🇦🇺",
  CAD: "🇨🇦",
  CHF: "🇨🇭",
  HKD: "🇭🇰",
  IDR: "🇮🇩",
  INR: "🇮🇳",
  KRW: "🇰🇷",
  LAK: "🇱🇦",
  MMK: "🇲🇲",
  MYR: "🇲🇾",
  NZD: "🇳🇿",
  PHP: "🇵🇭",
  SEK: "🇸🇪",
  TWD: "🇹🇼",
  VND: "🇻🇳",
  AED: "🇦🇪",
  ZAR: "🇿🇦",
  NGN: "🇳🇬",
  DKK: "🇩🇰",
  SAR: "🇸🇦",
  SDR: "🌐"
};

const i18n = {
  en: {
    title: "NBC Exchange Rates",
    subtitle: "Official Rates from National Bank of Cambodia",
    officialBadge: "Official Rate",
    officialLabel: "USD / KHR Rate",
    converterTitle: "💱 Currency Converter",
    amount: "Amount",
    from: "From Currency",
    to: "To Currency",
    searchPlaceholder: "Search currency name or code...",
    colCurrency: "Currency",
    colSymbol: "Symbol",
    colUnit: "Unit",
    colBid: "Bid (KHR)",
    colAsk: "Ask (KHR)",
    colAverage: "Average (KHR)",
    colRate1: "Rate for 1 Unit",
    refresh: "Refresh",
    ratesTitle: "Market Cross Rates",
    asOf: "Date:",
    popular: "Popular Pairs:"
  },
  km: {
    title: "អត្រាប្តូរប្រាក់ ធនាគារជាតិនៃកម្ពុជា",
    subtitle: "ទិន្នន័យផ្លូវការពីធនាគារជាតិនៃកម្ពុជា (NBC)",
    officialBadge: "អត្រាផ្លូវការ",
    officialLabel: "អត្រា USD / KHR",
    converterTitle: "💱 ឧបករណ៍គណនាប្តូរប្រាក់",
    amount: "ចំនួនទឹកប្រាក់",
    from: "រូបិយប័ណ្ណដើម",
    to: "រូបិយប័ណ្ណគោលដៅ",
    searchPlaceholder: "ស្វែងរកឈ្មោះរូបិយប័ណ្ណ ឬកូដ...",
    colCurrency: "រូបិយប័ណ្ណ",
    colSymbol: "និមិត្តសញ្ញា",
    colUnit: "ឯកតា",
    colBid: "ទិញ (រៀល)",
    colAsk: "លក់ (រៀល)",
    colAverage: "មធ្យម (រៀល)",
    colRate1: "អត្រាសម្រាប់ ១ ឯកតា",
    refresh: "ផ្ទុកឡើងវិញ",
    ratesTitle: "អត្រាប្តូរប្រាក់រូបិយប័ណ្ណផ្សេងៗ",
    asOf: "កាលបរិច្ឆេទ៖",
    popular: "រូបិយប័ណ្ណពេញនិយម៖"
  }
};

let currentLang = "en";
let exchangeData = null;

// DOM Elements
const officialRateEl = document.getElementById("officialRateVal");
const officialDateEl = document.getElementById("officialDate");
const popularChipsEl = document.getElementById("popularChips");
const ratesTableBody = document.getElementById("ratesTableBody");
const searchInput = document.getElementById("searchInput");
const refreshBtn = document.getElementById("refreshBtn");
const refreshIcon = document.getElementById("refreshIcon");
const langEnBtn = document.getElementById("langEn");
const langKmBtn = document.getElementById("langKm");
const fromCurrencySelect = document.getElementById("fromCurrency");
const toCurrencySelect = document.getElementById("toCurrency");
const amountInput = document.getElementById("amountInput");
const swapBtn = document.getElementById("swapBtn");
const resultAmountEl = document.getElementById("resultAmount");
const resultRateEl = document.getElementById("resultRate");
const errorBanner = document.getElementById("errorBanner");

// Initialization
document.addEventListener("DOMContentLoaded", () => {
  setupEventListeners();
  loadExchangeRates();
});

function setupEventListeners() {
  langEnBtn.addEventListener("click", () => setLanguage("en"));
  langKmBtn.addEventListener("click", () => setLanguage("km"));

  refreshBtn.addEventListener("click", () => {
    refreshIcon.classList.add("spin");
    loadExchangeRates(true).finally(() => {
      setTimeout(() => refreshIcon.classList.remove("spin"), 600);
    });
  });

  searchInput.addEventListener("input", filterTable);

  amountInput.addEventListener("input", updateConversion);
  fromCurrencySelect.addEventListener("change", updateConversion);
  toCurrencySelect.addEventListener("change", updateConversion);

  swapBtn.addEventListener("click", () => {
    const temp = fromCurrencySelect.value;
    fromCurrencySelect.value = toCurrencySelect.value;
    toCurrencySelect.value = temp;
    updateConversion();
  });
}

function setLanguage(lang) {
  if (currentLang === lang) return;
  currentLang = lang;

  if (lang === "km") {
    langKmBtn.classList.add("active");
    langEnBtn.classList.remove("active");
  } else {
    langEnBtn.classList.add("active");
    langKmBtn.classList.remove("active");
  }

  applyI18nLabels();
  loadExchangeRates();
}

function applyI18nLabels() {
  const t = i18n[currentLang];
  document.querySelectorAll("[data-i18n]").forEach(el => {
    const key = el.getAttribute("data-i18n");
    if (t[key]) {
      el.textContent = t[key];
    }
  });
  searchInput.placeholder = t.searchPlaceholder;
}

async function loadExchangeRates(forceRefresh = false) {
  hideError();
  const url = `${API_BASE_URL}/api/exchangerate?lang=${currentLang}&refresh=${forceRefresh}`;

  try {
    const res = await fetch(url);
    if (!res.ok) throw new Error(`HTTP ${res.status}: ${res.statusText}`);

    exchangeData = await res.json();
    renderData();
  } catch (err) {
    console.error("Failed to load exchange rates:", err);
    showError(`Could not fetch data from ${url}. Make sure the ASP.NET Core API is running.`);
  }
}

function renderData() {
  if (!exchangeData) return;

  const t = i18n[currentLang];

  // 1. Official USD/KHR Rate
  const officialRate = exchangeData.officialRate ? exchangeData.officialRate.rate : 0;
  officialRateEl.innerHTML = `${formatNumber(officialRate)} <span class="currency-unit">KHR / USD</span>`;
  officialDateEl.textContent = `${t.asOf} ${exchangeData.date || "-"}`;

  // 2. Populate Dropdowns for Converter
  populateCurrencyDropdowns();

  // 3. Render Top Popular Chips
  renderPopularChips();

  // 4. Render Table
  renderTable(exchangeData.rates || []);

  // 5. Update Live Converter
  updateConversion();
}

function populateCurrencyDropdowns() {
  const currentFrom = fromCurrencySelect.value || "USD";
  const currentTo = toCurrencySelect.value || "KHR";

  fromCurrencySelect.innerHTML = "";
  toCurrencySelect.innerHTML = "";

  const allCurrencies = [
    { code: "USD", name: currentLang === "km" ? "ដុល្លារអាមេរិក" : "US Dollar" },
    { code: "KHR", name: currentLang === "km" ? "រៀលកម្ពុជា" : "Cambodian Riel" }
  ];

  if (exchangeData && exchangeData.rates) {
    exchangeData.rates.forEach(r => {
      allCurrencies.push({
        code: r.currencyCode,
        name: r.currencyName
      });
    });
  }

  allCurrencies.forEach(c => {
    const flag = flagMap[c.code] || "💰";
    const optionText = `${flag} ${c.code} - ${c.name}`;

    const optFrom = new Option(optionText, c.code);
    const optTo = new Option(optionText, c.code);

    fromCurrencySelect.add(optFrom);
    toCurrencySelect.add(optTo);
  });

  fromCurrencySelect.value = currentFrom;
  toCurrencySelect.value = currentTo;
}

function renderPopularChips() {
  const popularCodes = ["USD", "THB", "EUR", "JPY", "CNY", "SGD", "VND"];
  popularChipsEl.innerHTML = "";

  popularCodes.forEach(code => {
    let rateStr = "";
    if (code === "USD") {
      rateStr = `${formatNumber(exchangeData.officialRate?.rate || 0)} ៛`;
    } else {
      const item = exchangeData.rates?.find(r => r.currencyCode === code);
      if (item) {
        rateStr = `${formatNumber(item.ratePerOneUnit)} ៛`;
      }
    }

    if (rateStr) {
      const chip = document.createElement("div");
      chip.className = "popular-chip";
      chip.innerHTML = `<span>${flagMap[code] || ""} ${code}</span> <strong>${rateStr}</strong>`;
      chip.style.cursor = "pointer";
      chip.title = `Convert with ${code}`;
      chip.addEventListener("click", () => {
        fromCurrencySelect.value = code;
        toCurrencySelect.value = "KHR";
        updateConversion();
      });
      popularChipsEl.appendChild(chip);
    }
  });
}

function renderTable(rates) {
  ratesTableBody.innerHTML = "";

  if (!rates || rates.length === 0) {
    ratesTableBody.innerHTML = `<tr><td colspan="7" class="text-center" style="padding: 24px; color: var(--text-muted);">No currency rates found.</td></tr>`;
    return;
  }

  rates.forEach(r => {
    const flag = flagMap[r.currencyCode] || "💰";
    const tr = document.createElement("tr");

    tr.innerHTML = `
      <td>
        <div class="currency-cell">
          <span class="flag">${flag}</span>
          <div>
            <div>${r.currencyName}</div>
            <small style="color: var(--text-muted); font-size: 0.75rem;">${r.currencyCode}</small>
          </div>
        </div>
      </td>
      <td class="text-center"><strong>${r.symbol}</strong></td>
      <td class="text-center"><span class="badge-unit">${formatNumber(r.unit)}</span></td>
      <td class="num-cell">${formatNumber(r.bid)}</td>
      <td class="num-cell">${formatNumber(r.ask)}</td>
      <td class="num-cell"><strong>${formatNumber(r.average)}</strong></td>
      <td class="num-cell" style="color: var(--primary); font-weight: 700;">${formatNumber(r.ratePerOneUnit)} ៛</td>
    `;

    ratesTableBody.appendChild(tr);
  });
}

function filterTable() {
  if (!exchangeData || !exchangeData.rates) return;
  const q = searchInput.value.toLowerCase().trim();

  const filtered = exchangeData.rates.filter(r => 
    r.currencyName.toLowerCase().includes(q) ||
    r.currencyCode.toLowerCase().includes(q) ||
    r.symbol.toLowerCase().includes(q)
  );

  renderTable(filtered);
}

function updateConversion() {
  if (!exchangeData) return;

  const amount = parseFloat(amountInput.value);
  if (isNaN(amount) || amount <= 0) {
    resultAmountEl.textContent = "0.00";
    resultRateEl.textContent = "";
    return;
  }

  const from = fromCurrencySelect.value;
  const to = toCurrencySelect.value;

  function getKhrRate(code) {
    if (code === "KHR") return 1;
    if (code === "USD") return exchangeData.officialRate?.rate || 4047;
    const item = exchangeData.rates?.find(r => r.currencyCode === code);
    return item ? item.ratePerOneUnit : 0;
  }

  const fromRate = getKhrRate(from);
  const toRate = getKhrRate(to);

  if (toRate === 0) {
    resultAmountEl.textContent = "N/A";
    return;
  }

  const amountInKhr = amount * fromRate;
  const converted = amountInKhr / toRate;
  const effectiveRate = fromRate / toRate;

  const fromFlag = flagMap[from] || "";
  const toFlag = flagMap[to] || "";

  resultAmountEl.innerHTML = `${formatNumber(converted, 4)} <span style="font-size: 1rem; color: var(--text-muted);">${to} ${toFlag}</span>`;
  resultRateEl.textContent = `1 ${from} = ${formatNumber(effectiveRate, 6)} ${to}`;
}

function formatNumber(num, maxDecimals = 2) {
  if (num === null || num === undefined) return "0";
  return Number(num).toLocaleString(undefined, {
    minimumFractionDigits: 0,
    maximumFractionDigits: maxDecimals
  });
}

function showError(msg) {
  errorBanner.textContent = msg;
  errorBanner.style.display = "block";
}

function hideError() {
  errorBanner.style.display = "none";
}
