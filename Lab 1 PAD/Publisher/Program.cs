using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml;
using System.Threading;

namespace Publisher
{
    class Program
    {
        public static string title;
        public static string description;
        public static string date, dateRaw;
        public static int i;
        public static string url0 = "https://www.sciencedaily.com/rss/top.xml";
        public static string url1 = "https://www.sciencedaily.com/rss/science_society/sports.xml";
        public static string url2 = "https://www.sciencedaily.com/rss/top/technology.xml";
        public static string[] urlStr = {url0, url1, url2 };
        static void Main(string[] args)
        {
              
        Console.WriteLine("Publisher");

            var publisherSocket = new PublisherSocket();
            publisherSocket.Connect("127.0.0.1", 9999);

            if (publisherSocket.isConnected)
            {
                while (true)
                {
                    for(i = 0; i < 3; i++)
                    {
                        XmlDocument rssXmlDoc = new XmlDocument();
                       
                        // Load the RSS file from the RSS URL
                        rssXmlDoc.Load(urlStr[i]);
                      
                        // Parse the Items in the RSS file
                        XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");
                       

                        var newsArr = new List<string>();


                        // Iterate through the items in the RSS file
                        int count = 0;
                        foreach (XmlNode rssNode in rssNodes)
                        {
                            count++;
                            XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                            title = rssSubNode != null ? rssSubNode.InnerText : "";

                            rssSubNode = rssNode.SelectSingleNode("description");
                            description = rssSubNode != null ? rssSubNode.InnerText : "";

                            rssSubNode = rssNode.SelectSingleNode("pubDate");
                            dateRaw = rssSubNode != null ? rssSubNode.InnerText : "";
                            if(dateRaw != null)
                            {
                                date = dateRaw.Substring(dateRaw.Length - 24, 20);
                            }
                            

                            newsArr.Add(date + "\n" + title + "\n" + description);


                            if (count == 3)
                            {
                                break;
                            }

                        }



                        //Console.WriteLine("Enter the topic:");
                        //phandler.newsCategory = Console.ReadLine().ToLower();
                        //Console.WriteLine("Enter the message:");
                        foreach (string aux in newsArr)
                        {
                            var phandler = new PHandler();
                            if (i == 0)
                            {
                                phandler.newsCategory = "news";
                            } else if(i==1){
                                phandler.newsCategory = "sport";
                            } else
                            {
                                phandler.newsCategory = "tech";
                            }
                            phandler.newsDate = aux.Substring(0, aux.IndexOf("\n"));
                            phandler.newsBody = aux.Substring(aux.IndexOf("\n") + 1);
                            var loadString = JsonConvert.SerializeObject(phandler);

                            byte[] data = Encoding.UTF8.GetBytes(loadString);

                            publisherSocket.Send(data);
                            Thread.Sleep(1000);

                        }
                    }
                    

                    Thread.Sleep(60000);

                }
            }



            Console.ReadLine();
        }


        
       
        
    }

}
