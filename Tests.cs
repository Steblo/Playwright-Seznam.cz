using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Pages;

namespace Tests
{
    [TestClass]
    public class Seznam
    {
        public TestContext TestContext { get; set; } = default!;

        private static IPlaywright? _playwright;
        private static IBrowser? _browser;
        private static IBrowserContext? _context;
        private HomePageSeznam homePage = null!;

        [ClassInitialize]
        public static async Task ClassSetup(TestContext context)
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
            _context = await _browser!.NewContextAsync();
        }


        [TestInitialize]
        public async Task Setup()
        {
            var context = await _browser!.NewContextAsync();

            await context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });

            homePage = new HomePageSeznam(await context.NewPageAsync());
            await homePage.GotoAsync();
            await homePage.ConsentAsync();
        }

        [ClassCleanup]
        public static async Task ClassTeardown()
        {
            await _browser!.CloseAsync();
            _playwright!.Dispose();
        }

        [TestCleanup]
        public async Task Teardown()
        {
            var testName = TestContext?.TestName ?? "UnknownTest";
            await homePage.Page.ScreenshotAsync(new() { Path = $"screenshots/{testName}.png" });

            if (TestContext!.CurrentTestOutcome != UnitTestOutcome.Passed)
            {
                await _context!.Tracing.StopAsync(new TracingStopOptions
                {
                    Path = $"traces/{testName}.zip"
                });
            }
            else
            {
                await _context!.Tracing.StopAsync();
            }

            await homePage.Page.Context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = $"traces/{testName}.zip"
            });

            foreach (var page in _context.Pages)
            {
                await page.CloseAsync();
            }
        }

        [TestMethod]
        [Priority(1)]
        public async Task UrlSeznam()
        {
            await Assertions.Expect(homePage.Page).ToHaveURLAsync(new Regex("https://www.seznam.cz"));
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

            await Assertions.Expect(searachPage.Page).ToHaveURLAsync(new Regex("https://search.seznam.cz/"));

            await Assertions.Expect(searachPage.ResultsLocator.First).ToContainTextAsync("playwright.dev");
        }
    }
}