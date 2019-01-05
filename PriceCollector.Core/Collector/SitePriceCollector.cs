using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using PriceCollector.Core.Config;
using PriceCollector.Core.Data.Enums;
using PriceCollector.Core.Data.Item;
using PriceCollector.Core.Data.Result;
using PriceCollector.Core.Data.Settings;
using PriceCollector.Core.Extensions;
using PriceCollector.Core.Loader;
using PriceCollector.Core.Selector;


namespace PriceCollector.Core.Collector {
    public class SitePriceCollector : IPriceCollector {
        private readonly ILoaderFactory _loaderFactory;
        private readonly ISelectorFactory _selectorFactory;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly ILogger _errorPareslogger = LogManager.GetLogger($"{nameof(SitePriceCollector)}_ErrorContent");
        private readonly IConfig _parseConfig;
        private readonly IResultSaver _saver;

        public SitePriceCollector(IResultSaver saver, IConfig parseConfig, ILoaderFactory loaderFactory, ISelectorFactory selectorFactory) {
            _saver = saver;
            _parseConfig = parseConfig;
            _loaderFactory = loaderFactory;
            _selectorFactory = selectorFactory;
        }

        public async Task Collect() {
            var settings = _parseConfig.GetSettings();
            var tasks = settings.Items
                .Select(e => CollectItem(e, settings))
                .ToArray();

            await Task.WhenAll(tasks);
            var itemsToSave = tasks
                .SelectMany(e => e.Result)
                .Where(e => e.Result.Status == ResultStatus.OK)
                .ToArray();

            await _saver.Save(itemsToSave);
        }

        private async Task<List<ResultItem>> CollectItem(ParseItem item, CollectorSettings settings) {
            var sellerParseSettings = settings.GetParseSettings(item.SellerName);
            string content;
            try {
                var loader = _loaderFactory.GetLoader(sellerParseSettings.LoaderName);
                content = await loader.Load(item.Url);
            } catch (Exception ex) {
                _logger.Error(ex, $"[{item.ToJson()}] ошибка при загрузке страницы");
                return new List<ResultItem>(0);
            }

            try {
                var selector = _selectorFactory.GetSelector(item.SelectorType);
                return await selector.Select(content);
            } catch (Exception ex) {
                _logger.Error(ex, $"[{item.ToJson()}] ошибка при парсинге страницы");
                _errorPareslogger.Error(ex, $"[{item.ToJson()}] ошибка при парсинге страницы : {content}");
                return new List<ResultItem>(0);
            }
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
    }
}