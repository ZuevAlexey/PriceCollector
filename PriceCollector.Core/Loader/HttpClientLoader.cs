using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PriceCollector.Core.Loader {
    public class HttpClientLoader : ILoader {
        public const string LOADER_NAME = "HttpClient";

        public async Task<string> Load(string url) {
            var client = new HttpClient(new HttpClientHandler {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = true,
            });
            var data = await client.GetAsync(url);
            return await data.Content.ReadAsStringAsync();
        }

        public string Name { get; } = LOADER_NAME;
    }
}