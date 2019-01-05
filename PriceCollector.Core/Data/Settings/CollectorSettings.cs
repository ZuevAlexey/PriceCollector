using System;
using System.Linq;
using System.Collections.Generic;
using PriceCollector.Core.Data.Item;

namespace PriceCollector.Core.Data.Settings {
    [Serializable]
    public class CollectorSettings {
        private Dictionary<string, SellerItemParseSettings> _sellerSettings;

        public CollectorSettings(List<ParseItem> items, List<SellerItemParseSettings> sellerSettings) {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            _sellerSettings = (sellerSettings ?? throw new ArgumentNullException(nameof(sellerSettings)))
                .ToDictionary(e => e.Name, e => e);
        }

        public List<ParseItem> Items { get; }

        public SellerItemParseSettings GetParseSettings(string sellerName) {
            if(sellerName == null) {
                throw new ArgumentNullException(nameof(sellerName));
            }

            if(!_sellerSettings.TryGetValue(sellerName, out var result)) {
                throw new ArgumentOutOfRangeException($"Seller {sellerName} not register");
            }

            return result;
        }
    }
}