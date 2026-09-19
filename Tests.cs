using Microsoft.Playwright;
using System.Text.RegularExpressions;
using Pages;

namespace Tests
{
    [TestClass]
    public class Seznam
    {
        private HomePageSeznam homePage = null!;

        [TestInitialize]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
                //SlowMo = 100
            });

            homePage = new HomePageSeznam(await browser.NewPageAsync());
            await homePage.GotoAsync();
            await homePage.ConsentAsync();
        }

        [TestCleanup]
        public async Task Teardown()
        {
            await homePage.Close();
        }

        [TestMethod]
        [Priority(1)]
        public async Task UrlSeznam()
        {
            await Assertions.Expect(homePage._page).ToHaveURLAsync(new Regex("https://www.seznam.cz"));
        }

        [TestMethod]
        [Priority(2)]
        public async Task LogoSeznam()
        {
            await Assertions.Expect(homePage.LogoLocator).ToHaveCountAsync(4);
        }

        [TestMethod]
        [Priority(3)]
        public async Task MainpageSeznam()
        {
            await Assertions.Expect(homePage.MainContentLocator).ToBeVisibleAsync();
        }

        [TestMethod]
        [Priority(4)]
        public async Task ServicesSeznam()
        {
            await Assertions.Expect(homePage.ServicesLocator).ToHaveCountAsync(23);
        }

        [TestMethod]
        [Priority(5)]
        public async Task WeatherNowAndTodaySeznam()
        {
            await Assertions.Expect(homePage.WeatherLocator.Last).ToHaveTextAsync(new Regex(@"\d"));

            await Assertions.Expect(homePage.WeatherLocator.Nth(3)).ToHaveTextAsync(new Regex(@"\d"));
        }

        [TestMethod]
        [Priority(6)]
        public async Task SearchSeznam()
        {
            var searachPage = await homePage.SearchAsync("Playwright");

            await Assertions.Expect(searachPage._page).ToHaveURLAsync(new Regex("https://search.seznam.cz/"));

            await Assertions.Expect(searachPage.ResultsLocator.First).ToHaveTextAsync(new Regex(@"playwright.dev"));
        }
    }
}