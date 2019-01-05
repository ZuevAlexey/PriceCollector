using System;

namespace PriceCollector.Core.Data.Settings {
    [Serializable]
    public class ParseItemSettings {
        public ParseItemSettings(string url, string elementSelector, string loaderName, string attributeName) {
            Url = url;
            ElementSelector = elementSelector;
            LoaderName = loaderName;
            AttributeName = attributeName;
        }

        public string Url { get; }
        public string ElementSelector { get; }
        public string AttributeName { get; }
        public string LoaderName { get; }

        public bool IsContentSelector => AttributeName == null;
    }
}