using System;

namespace PriceCollector.Core.Data.Settings {
    [Serializable]
    public class SellerItemParseSettings {
        public string Name { get; set;}
        public string ElementSelector { get; set;}
        public string AttributeName { get; set;}
        public string LoaderName { get; set;}

        public bool IsContentSelector => AttributeName == null;
    }
}
