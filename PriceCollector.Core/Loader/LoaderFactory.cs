using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCollector.Core.Loader {
    public class LoaderFactory : ILoaderFactory {
        private readonly Dictionary<string, ILoader> _loaders = new Dictionary<string, ILoader> ();
        private readonly string _defaultLoaderName;

        public LoaderFactory(ICollection<ILoader> loaders, string defaultLoaderName = null) {
            _defaultLoaderName = defaultLoaderName ?? HttpClientLoader.LOADER_NAME;
            AddDefaultLoaders();

            if (loaders == null || !loaders.Any()) {
                return;
            }

            foreach (var loader in loaders) {
                if (!_loaders.ContainsKey(loader.Name)) {
                    _loaders.Add(loader.Name, loader);
                }
            }
        }

        private void AddDefaultLoaders() {
            var defaultLoaders = new ILoader[] {
                new HttpClientLoader(),
                new ChromeDriverLoader()
            };

            foreach (var defaultLoader in defaultLoaders) {
                _loaders.Add(defaultLoader.Name, defaultLoader);
            }
        }

        public ILoader GetLoader(string name) {
            var key = name ?? _defaultLoaderName;
            if (_loaders.TryGetValue(key, out var result)) {
                return result;
            }

            throw new ArgumentOutOfRangeException($"Нет лоадера с указанным типом {name}");
        }
    }
}