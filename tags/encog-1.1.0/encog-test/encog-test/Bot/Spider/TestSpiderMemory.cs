using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Encog.Bot.Spider;
using System.IO;
using Encog.Bot.Spider.Workload.Memory;

namespace encog_test.Bot.Spider
{
    [TestFixture]
    public class TestSpiderMemory : ISpiderReportable
    {
        const String baseUri = "www.httprecipes.com";

        private int urlsProcessed;

        [Test]
        public void Spider()
        {
            SpiderOptions options = new SpiderOptions();

            options.Startup = SpiderOptions.STARTUP_CLEAR;
            options.WorkloadManager = "Encog.Bot.Spider.Workload.Memory.MemoryWorkloadManager";
           
            options.Filter.Add("Encog.Bot.Spider.Filter.RobotsFilter");
            Encog.Bot.Spider.Spider spider = new Encog.Bot.Spider.Spider(options, this);

            spider.AddURL(new Uri("http://www.httprecipes.com"), null, 1);
            spider.Process();
            Console.WriteLine("::"+this.urlsProcessed);
            Assert.IsTrue(this.urlsProcessed > 100);



        }


        public bool BeginHost(String host)
        {
            return string.Compare(host, "www.httprecipes.com", true) == 0;
        }


        public void Init(Encog.Bot.Spider.Spider spider)
        {
            // TODO Auto-generated method stub

        }


        public bool SpiderFoundURL(Uri url, Uri source, Encog.Bot.Spider.Spider.URLType type)
        {
            if (type != Encog.Bot.Spider.Spider.URLType.HYPERLINK)
            {
                return true;
            }
            else if ((baseUri != null) && (string.Compare(baseUri, url.Host) != 0))
            {
                return false;
            }

            return true;
        }


        public void SpiderProcessURL(Uri url, Stream stream)
        {
            // TODO Auto-generated method stub

        }


        public void SpiderProcessURL(Uri url, SpiderParseHTML parse)
        {
            try
            {
                parse.ReadAll();
            }
            catch (IOException)
            {
            }
            this.urlsProcessed++;

        }


        public void SpiderURLError(Uri url)
        {
            // TODO Auto-generated method stub

        }
    }
}
