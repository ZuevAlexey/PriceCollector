using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PriceCollector.Core.Data.Result;
using PriceCollector.Core.Data.Settings;

namespace PriceCollector.Core.Selector {
    public class ListSelector : ISelector {
        public Task<List<ResultItem>> Select(string content, ParseItem item) {
            throw new NotImplementedException();
        }
    }
}
