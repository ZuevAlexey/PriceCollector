using PriceCollector.Core.Data.Item;

namespace PriceCollector.Core.Data.Result {
   public class ResultItem {
      public ItemInfo Info { get; set; }
      public PriceResult Result { get; set; }
   }
}