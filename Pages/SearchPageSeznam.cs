using Microsoft.Playwright;

namespace TestProject3.Pages
{
    public class SearchPageSeznam(IPage page)
    {
        public readonly IPage _page = page;
        public ILocator ResultsLocator => _page.Locator("a[data-e-a=\"reference\"]");

        public async Task Close()
        {
            await _page.CloseAsync();
        }

    }
}
