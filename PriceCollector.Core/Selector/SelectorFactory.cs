using System.Collections.Generic;
using PriceCollector.Core.Data.Enums;

namespace PriceCollector.Core.Selector {
    public class SelectorFactory : ISelectorFactory {
        private Dictionary<SelectorType, ISelector> _selectors = new Dictionary<SelectorType, ISelector> {
            {SelectorType.Item, new ItemSelector()},
            {SelectorType.List, new ListSelector()},
        };


        public ISelector GetSelector(SelectorType type) {
            return _selectors[type];
        }
    }
}
