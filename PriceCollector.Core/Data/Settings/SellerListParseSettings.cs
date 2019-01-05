using System;

namespace PriceCollector.Core.Data.Settings {
    [Serializable]
    public class SellerListParseSettings {
        public string Name { get; set;}
        public string ItemSelector { get; set;}
        public string NameSelector { get; set;}
        public string UrlSelector { get; set;}
        public string PriceSelector { get; set;}
    }
}
