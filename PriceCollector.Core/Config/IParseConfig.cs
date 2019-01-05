using PriceCollector.Core.Data.Settings;

namespace PriceCollector.Core.Config {
   public interface IParseConfig {
      CollectorSettings GetSettings();
   }
}