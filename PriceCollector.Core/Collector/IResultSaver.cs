using System.Collections.Generic;
using System.Threading.Tasks;
using PriceCollector.Core.Data.Result;

namespace PriceCollector.Core.Collector {
   public interface IResultSaver {
      Task Save(ICollection<ResultItem> items);
   }
}