using System;
using System.Collections.Generic;
using PriceCollector.Core.Data.Item;

namespace PriceCollector.Core.Data.Settings {
   [Serializable]
   public class CollectorSettings {
      public CollectorSettings(List<ParseItem> settings) {
         Settings = settings ?? throw new ArgumentNullException(nameof(settings));
      }

      public List<ParseItem> Settings { get; }
   }
}