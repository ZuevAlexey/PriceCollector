using Newtonsoft.Json;
using PriceCollector.Core.Data.Enums;

namespace PriceCollector.Core.Data.Result {
   public class PriceResult {
      public decimal Price { get; set; }
      public string Url { get; set; }
      public ResultStatus Status { get; set; }

       /// <summary>Returns a string that represents the current object.</summary>
       /// <returns>A string that represents the current object.</returns>
       public override string ToString() {
           return JsonConvert.SerializeObject(this);
       }
   }
}