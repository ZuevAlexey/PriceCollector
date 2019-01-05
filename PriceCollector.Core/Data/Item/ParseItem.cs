using System;
using PriceCollector.Core.Data.Settings;

namespace PriceCollector.Core.Data.Item {
   [Serializable]
   public class ParseItem {
      public ParseItem(string productName, string sellerName, ParseItemSettings parseSettings) {
         Info = new ItemInfo {
            ProductName = productName,
            SellerName = sellerName
         };
         ParseSettings = parseSettings;
      }

      public ItemInfo Info { get; set; }
      public ParseItemSettings ParseSettings { get; }
   }
}