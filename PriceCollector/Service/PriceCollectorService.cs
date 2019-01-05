using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using NLog;
using PriceCollector.Config;
using PriceCollector.Core.Collector;
using PriceCollector.Core.Loader;
using PriceCollector.Saver;

public class PriceCollectorService {
    readonly Timer _timer;

    public PriceCollectorService() {
        _timer = new Timer(TimeSpan.Parse(ConfigurationManager.AppSettings["interval"]).TotalMilliseconds) {AutoReset = true};
        _timer.Elapsed += (sender, eventArgs) => Collect();
    }

    public void Start() {
        Task.Run(() => Collect());
        _timer.Start();
    }

    public void Stop() {
        _timer.Stop();
    }

    private void Collect() {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "ParseConfig.json");
        var config = JsonConfig.Load(configPath);
        var collector = new SitePriceCollector(new SQLiteResultSaver(), config, new LoaderFactory(null));
        collector.Collect();
    }
}