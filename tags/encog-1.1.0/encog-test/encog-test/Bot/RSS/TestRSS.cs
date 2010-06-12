using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using Encog.Bot.RSS;

namespace encog_test.Bot.RSS
{
    [TestFixture]
    public class TestRSS
    {
        private void Test(Uri url)
        {
            Encog.Bot.RSS.RSS rss = new Encog.Bot.RSS.RSS();
            rss.Load(url);
            Assert.IsTrue(rss.ToString().Length > 0);
            Assert.AreEqual(14, rss.Items.Count);
            RSSItem item = rss.Items[0];
            Assert.AreEqual("Chapter 1: The Structure of HTTP Requests", item.Title);
            Assert.AreEqual("http://www.httprecipes.com/1/1/", item.Link);
        }

        [Test]
        public void RSS2()
        {
            Uri url = new Uri("http://www.httprecipes.com/1/12/rss2.xml");
            Test(url);
        }

        [Test]
        public void RSS1()
        {
            Uri url = new Uri("http://www.httprecipes.com/1/12/rss1.xml");
            Test(url);
        }
    }
}
