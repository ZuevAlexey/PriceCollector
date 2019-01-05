using System.Threading.Tasks;
using System.Collections.Generic;
using PriceCollector.Core.Data.Result;
using PriceCollector.Core.Data.Settings;

namespace PriceCollector.Core.Selector {
    public interface ISelector {
        Task<List<ResultItem>> Select(string content, ParseItem item);
    }
}