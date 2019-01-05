using PriceCollector.Core.Data.Enums;

namespace PriceCollector.Core.Selector {
    public interface ISelectorFactory {
        ISelector GetSelector(SelectorType type);
    }
}
