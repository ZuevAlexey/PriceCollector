using System;

namespace PriceCollector.Core.Data.Item {
    [Serializable]
    public class ItemInfo {
        public string ProductName { get; set; }
        public string SellerName { get; set; }
    }
}