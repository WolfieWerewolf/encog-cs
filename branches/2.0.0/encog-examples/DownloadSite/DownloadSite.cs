using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Bot.Spider;
using Encog.Neural.NeuralData.Market.Loader;
using Encog.Neural.NeuralData.Market;

namespace DownloadSite
{
    class DownloadSite
    {
        /// <summary>
        /// Download entire site.
        /// </summary>
        /// <param name="baseUri">URL to download from.</param>
        /// <param name="local">Local directory to download to.</param>
        public void Download(Uri baseUri, String local)
        {
            SpiderReport report = new SpiderReport(local);
            SpiderOptions options = new SpiderOptions();

            options.WorkloadManager = "Encog.Bot.Spider.Workload.Memory.MemoryWorkloadManager";

            //options.WorkloadManager = "Encog.Bot.Spider.Workload.SQL.SQLWorkloadManager";
            //options.DbConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=c:\\encog.mdb; User Id=admin; Password=";


            Spider spider = new Spider(options, report);
            spider.AddURL(baseUri, null, 1);
            spider.Process();
            Console.WriteLine(spider.Status);
        }



        static void Main(string[] args)
        {
            IMarketLoader loader = new YahooFinanceLoader();
            TickerSymbol tickerAAPL = new TickerSymbol("AAPL", null);
            TickerSymbol tickerMSFT = new TickerSymbol("MSFT", null);
            MarketNeuralDataSet marketData = new MarketNeuralDataSet(loader, 5, 1);
            marketData.AddDescription(new MarketDataDescription(tickerAAPL, MarketDataType.CLOSE, true, true));
            marketData.AddDescription(new MarketDataDescription(tickerMSFT, MarketDataType.CLOSE, true, false));
            marketData.AddDescription(new MarketDataDescription(tickerAAPL, MarketDataType.VOLUME, true, false));
            marketData.AddDescription(new MarketDataDescription(tickerMSFT, MarketDataType.VOLUME, true, false));
            DateTime begin = new DateTime(2008, 7, 1);
            DateTime end = new DateTime(2008, 7, 31);
            marketData.Load(begin, end);
            marketData.Generate();
            Console.WriteLine(marketData.Points.Count);
            //Assert.AreEqual(21, marketData.Points.Count);


            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine(
                    "Usage: DownloadSite [Path to download to] [URL to download]");
                }
                else
                {
                    DownloadSite download = new DownloadSite();
                    download.Download(new Uri(args[1]), args[0]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
