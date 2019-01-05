using System.IO;
using Newtonsoft.Json;
using PriceCollector.Core.Config;
using PriceCollector.Core.Data.Settings;
using PriceCollector.Core.Extensions;

namespace PriceCollector.Config {
   public class JsonConfig : IConfig {
      private readonly CollectorSettings _settings;

      private JsonConfig(CollectorSettings settings) {
         _settings = settings;
      }

      public CollectorSettings GetSettings() {
         return _settings.DeepClone();
      }

      public static JsonConfig Load(string path) {
         var content = File.ReadAllText(path);
         var settings = JsonConvert.DeserializeObject<CollectorSettings>(content);
         return new JsonConfig(settings);
      }
   }
}