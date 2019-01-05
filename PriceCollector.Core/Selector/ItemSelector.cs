using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using System.Collections.Generic;
using PriceCollector.Core.Data.Result;
using NLog;
using PriceCollector.Core.Data.Settings;
using System.Threading.Tasks;
using PriceCollector.Core.Extensions;

namespace PriceCollector.Core.Selector {
    public class ItemSelector : ISelector {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public async Task<List<ResultItem>> Select(string content, ParseItem item) {
            var document = await new HtmlParser().ParseAsync(content);
            



            var element = document.QuerySelector(sellerParseSettings.ElementSelector);

            var dirtyPrice = sellerParseSettings.IsContentSelector
                ? element.InnerHtml
                : element.Attributes[sellerParseSettings.AttributeName].Value;
            var price = ParseDirtyPrice(dirtyPrice);
            result.Result.Price = price;
            result.Result.Status = ResultStatus.OK;
            _logger.Info($"[{item.ToJson()}] успешно спарсили страницу: {result.ToJson()}");
        }
    }
}
