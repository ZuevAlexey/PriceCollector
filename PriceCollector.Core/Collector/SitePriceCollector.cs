using System;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using NLog;
using PriceCollector.Core.Config;
using PriceCollector.Core.Data.Enums;
using PriceCollector.Core.Data.Item;
using PriceCollector.Core.Data.Result;
using PriceCollector.Core.Data.Settings;
using PriceCollector.Core.Extensions;
using PriceCollector.Core.Loader;

namespace PriceCollector.Core.Collector {
    public class SitePriceCollector : IPriceCollector {
        private readonly ILoaderFactory _loaderFactory;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly ILogger _errorPareslogger = LogManager.GetLogger($"{nameof(SitePriceCollector)}_ErrorContent");
        private readonly IParseConfig _parseConfig;
        private readonly IResultSaver _saver;

        public SitePriceCollector(IResultSaver saver, IParseConfig parseConfig, ILoaderFactory loaderFactory) {
            _saver = saver;
            _parseConfig = parseConfig;
            _loaderFactory = loaderFactory;
        }

        public async Task Collect() {
            var settings = _parseConfig.GetSettings();
            var tasks = settings.Settings
                .Select(CollectItem)
                .ToArray();

            await Task.WhenAll(tasks);
            var itemsToSave = tasks
                .Select(e => e.Result)
                .Where(e => e.Result.Status == ResultStatus.OK)
                .ToArray();

            await _saver.Save(itemsToSave);
        }

        private async Task<ResultItem> CollectItem(ParseItem item) {
            var settings = item.ParseSettings;

            var result = new ResultItem {
                Info = item.Info,
                Result = new PriceResult {
                    Url = item.ParseSettings.Url
                }
            };
            IHtmlDocument document;
            try {
                document = await GetDocument(settings);
            } catch (Exception ex) {
                _logger.Error(ex, $"[{item.ToJson()}] ошибка при загрузке страницы");
                result.Result.Status = ResultStatus.LoadFail;
                return result;
            }

            var element = document.QuerySelector(settings.ElementSelector);

            try {
                var dirtyPrice = settings.IsContentSelector
                    ? element.InnerHtml
                    : element.Attributes[settings.AttributeName].Value;
                var price = ParseDirtyPrice(dirtyPrice);
                result.Result.Price = price;
                result.Result.Status = ResultStatus.OK;
                _logger.Info($"[{item.ToJson()}] успешно спарсили страницу: {result.ToJson()}");
            } catch (Exception ex) {
                _logger.Error(ex, $"[{item.ToJson()}] ошибка при парсинге страницы");
                _errorPareslogger.Error(ex, $"[{item.ToJson()}] ошибка при парсинге страницы : {document.ToHtml()}");
                result.Result.Status = ResultStatus.LoadFail;
            }

            return result;
        }

        private static decimal ParseDirtyPrice(string dirtyPrice) {
            const decimal INVALID_VALUE = -1;

            var cleanPrice = dirtyPrice.Replace(" ", "");
            var buffer = "";
            var result = INVALID_VALUE;
            foreach (var curChar in cleanPrice) {
                buffer += curChar;
                if (decimal.TryParse(buffer, out var curResult)) {
                    result = curResult;
                    continue;
                }

                if (buffer.Length > 1) {
                    break;
                }

                buffer = "";
            }

            if (result == INVALID_VALUE) {
                throw new Exception($"Не удалось распарсить значение {dirtyPrice} к типу decimal");
            }

            return result;
        }

        private async Task<IHtmlDocument> GetDocument(ParseItemSettings settings) {
            var content = await _loaderFactory.GetLoader(settings.LoaderName).Load(settings.Url);
            return await new HtmlParser().ParseAsync(content);
        }
    }
}