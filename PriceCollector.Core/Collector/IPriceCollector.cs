using System.Threading.Tasks;

namespace PriceCollector.Core.Collector {
   public interface IPriceCollector {
      Task Collect();
   }
}