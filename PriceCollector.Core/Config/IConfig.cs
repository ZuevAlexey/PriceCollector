using PriceCollector.Core.Data.Settings;

namespace PriceCollector.Core.Config {
   public interface IConfig {
      CollectorSettings GetSettings();
   }
}