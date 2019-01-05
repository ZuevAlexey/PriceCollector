using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;

namespace PriceCollector.Core.Loader {
    public class ChromeDriverLoader : ILoader {
        public const string LOADER_NAME = "ChromeDriver";

        public async Task<string> Load(string url) {
            using (var driver = new ChromeDriver()) {
                driver.Navigate().GoToUrl(url);
                var source = driver.PageSource;
                return source;
            }
        }

        public string Name { get; } = LOADER_NAME;
    }
}