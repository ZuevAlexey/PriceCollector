using System.Threading.Tasks;

namespace PriceCollector.Core.Loader {
    public interface ILoader {
        Task<string> Load(string url);
        string Name { get; }
    }
}
