using Newtonsoft.Json;

namespace PriceCollector.Core.Extensions {
    public static class JsonSerialize {
        public static string ToJson(this object obj) {
            return $"{obj.GetType().Name} = {JsonConvert.SerializeObject(obj)}";
        }
    }
}