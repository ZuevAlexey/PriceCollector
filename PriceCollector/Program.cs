using System;
using Topshelf;

namespace PriceCollector {
    public class Program {
        public static void Main() {
            var rc = HostFactory.Run(x => {
                x.Service<PriceCollectorService>(s => {
                    s.ConstructUsing(name => new PriceCollectorService());
                    s.WhenStarted(e => e.Start());
                    s.WhenStopped(e => e.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("Сервис для мониторинга цен на конкретные товары");
                x.SetDisplayName("MyCompany PriceCollector Service");
                x.SetServiceName("MyCompanyPriceCollector2");
            });

            var exitCode = (int) Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}