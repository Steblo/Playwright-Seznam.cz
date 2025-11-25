using Microsoft.Playwright;

namespace TestProject3.Pages
{
    public class HomePageSeznam(IPage page)
    {
        public readonly IPage _page = page;

        public ILocator LogoLocator => _page.GetByAltText("Seznam.cz - hlavní strana");
        public ILocator MainContentLocator => _page.Locator("id=hl-obsah");
        public ILocator ServicesLocator => _page.Locator(".h-full.d-flex.align-items-end.font-12.line-height-14.text-center.atm-service-dashboard-badge__title");
        public ILocator WeatherLocator => _page.Locator(".atm-weather-item__temp-value");
        public ILocator SearchInput => _page.GetByRole(AriaRole.Textbox, new() { Name = "Vyhledat" });
        private ILocator SearchTab => _page.Locator(".ogm-search__tabs");
        public ILocator SearchTabs => SearchTab.Locator("li");
        public ILocator NameDay => _page.GetByRole(AriaRole.Link, new() { Name = "Kateřina" });

        public async Task GotoAsync()
        {
            _page.SetDefaultTimeout(60000);
            await _page.GotoAsync("https://seznam.cz");
        }

        public async Task ConsentAsync()
        {
            await _page.GetByTestId("cwl-dialog-headline").ClickAsync();

            await _page.GetByTestId("cw-button-agree-with-ads").ClickAsync();
        }

        public async Task Close()
        {
            await _page.CloseAsync();
        }

        public async Task<SearchPageSeznam> SearchAsync(string searchText)
        {
            await SearchInput.ClickAsync();
            await SearchInput.FillAsync(searchText);
            await SearchInput.PressAsync("Enter");

            return new SearchPageSeznam(_page);
        }
    }
}
