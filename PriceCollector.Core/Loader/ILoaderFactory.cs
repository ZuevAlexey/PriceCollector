namespace PriceCollector.Core.Loader {
    public interface ILoaderFactory {
        ILoader GetLoader(string name);
    }
}
   