using System.Text.RegularExpressions;
using TestProject3.Pages;

namespace TestProject3
{
    [TestClass]
    public class Seznam : PageTest
    {
        private HomePageSeznam homePage = null!;

        [TestInitialize]
        public async Task Setup()
        {
            homePage = new HomePageSeznam(Page);
            await homePage.GotoAsync();
            await homePage.ConsentAsync();
        }

        [TestCleanup]
        public async Task Teardown()
        {
            await homePage.Close();
        }

        [TestMethod]
        public async Task UrlSeznam()
        {
            await Expect(homePage._page).ToHaveURLAsync(new Regex("https://www.seznam.cz"));
        }

        [TestMethod]
        public async Task LogoSeznam()
        {
            await Expect(homePage.LogoLocator).ToHaveCountAsync(4);
        }

        [TestMethod]
        public async Task MainpageSeznam()
        {
            await Expect(homePage.MainContentLocator).ToBeVisibleAsync();
        }

        [TestMethod]
        public async Task ServicesSeznam()
        {
            await Expect(homePage.ServicesLocator).ToHaveCountAsync(24);
        }

        [TestMethod]
        public async Task WeatherNowAndTodaySeznam()
        {
            await Expect(homePage.WeatherLocator.Last).ToHaveTextAsync(new Regex(@"\d"));

            await Expect(homePage.WeatherLocator.Nth(3)).ToHaveTextAsync(new Regex(@"\d"));
        }
    }
}