using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Bot.Spider;
using Encog.Bot.HTML;
using System.IO;

namespace DownloadSite
{
    class SpiderReport : ISpiderReportable
    {
        /// <summary>
        /// The base host. Only URL's from this host will be downloaded.
        /// </summary>
        private String baseUri;

        /// <summary>
        /// The local path to save downloaded files to.
        /// </summary>
        private String path;


        /// <summary>
        /// Construct a SpiderReport object.
        /// </summary>
        /// <param name="path">The local file path to store the files to.</param>
        public SpiderReport(String path)
        {
            this.path = path;
        }

        /// <summary>
        /// This function is called when the spider is ready to
        /// process a new host. This function simply stores the
        /// value of the current host.
        /// </summary>
        /// <param name="host">The new host that is about to be processed.</param>
        /// <returns>True if this host should be processed, false otherwise.</returns>
        public bool BeginHost(String host)
        {
            if (this.baseUri == null)
            {
                this.baseUri = host;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Called when the spider is starting up. This method
        /// provides the SpiderReportable class with the spider
        /// object. This method is not used in this manager.
        /// </summary>
        /// <param name="spider">The spider that will be working with this object.</param>
        public void Init(Spider spider)
        {
        }

        /// <summary>
        /// Called when the spider encounters a URL. If the URL is
        /// on the same host as the base host, then the function
        /// will return true, indicating that the URL is to be
        /// processed.
        /// </summary>
        /// <param name="url">The URL that the spider found.</param>
        /// <param name="source">The page that the URL was found on.</param>
        /// <param name="type">The type of link this URL is.</param>
        /// <returns>True if the spider should scan for links on this page.</returns>
        public bool SpiderFoundURL(Uri url, Uri source,
            Encog.Bot.Spider.Spider.URLType type)
        {
            Console.WriteLine(url);
            if ((this.baseUri != null) && (string.Compare(this.baseUri, url.Host, true) != 0))
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Called when the spider is about to process a NON-HTML URL.
        /// </summary>
        /// <param name="url">The URL that the spider found.</param>
        /// <param name="stream">An InputStream to read the page contents from.</param>
        public void SpiderProcessURL(Uri url, Stream stream)
        {
            byte[] buffer = new byte[1024];

            int length;
            String filename = URLUtility.ConvertFilename(this.path, url, true);

            try
            {
                Stream os = File.OpenWrite(filename);
                do
                {
                    length = stream.Read(buffer, 0, buffer.Length);
                    if (length != 0)
                    {
                        os.Write(buffer, 0, length);
                    }
                } while (length != 0);
                os.Close();

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Called when the spider is ready to process an HTML
        /// URL. Download the contents of the URL to a local file.
        /// </summary>
        /// <param name="url">The URL that the spider is about to process.</param>
        /// <param name="parse">An object that will allow you you to parse the
        /// HTML on this page.</param>
        public void SpiderProcessURL(Uri url, SpiderParseHTML parse)
        {
            String filename = URLUtility.ConvertFilename(this.path, url, true);
            Stream os = File.OpenWrite(filename);
            parse.Stream.OutputStream = os;
            parse.ReadAll();
            os.Close();

        }

        /// <summary>
        /// Called when the spider tries to process a URL but gets
        /// an error. This method is not used in tries manager.
        /// </summary>
        /// <param name="url">The URL that generated an error.</param>
        public void SpiderURLError(Uri url)
        {
        }
    }
}
