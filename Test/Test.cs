using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PeopleBrowsr;
using Kred;
using System.Reflection;

namespace ConsoleApplication1
{
    class Test
    {
        static void Main(string[] args)
        {
            PeopleBrowsrTest();
            KredTest();
        }

        private static void KredTest()
        {
            Kred.KredAPI consumer = new KredAPI("[app_id]", "[app_key]");
            List<KeyValuePair<string, string>> QueryParams = new List<KeyValuePair<string, string>>(
                new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("source", "twitter"),
                    new KeyValuePair<string, string>("term", "zombies"),
                }
            );

            string[] methods = {
                "Kred",
                "KredScore"
            };
            List<string> NumErrors = new List<string>();
            List<IDictionary<string, object>> Results = new List<IDictionary<string, object>>();
            StringBuilder sb = new StringBuilder();
            foreach (var m in methods)
            {
                try
                {
                    Type t = consumer.GetType();
                    var result = (IDictionary<string, object>)t.InvokeMember(m,
                        BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                        null,
                        consumer,
                        new object[] { QueryParams }
                    );
                    Results.Add(result);
                    if (!result["status"].Equals("complete"))
                        throw new Exception("Status not complete!");
                    string raw = JsonParser.JsonParser.ToJson(result);
                    sb.AppendLine(raw);
                }
                catch (Exception err)
                {
                    NumErrors.Add(String.Format("{0} => {1}", m, err.Message));
                }
            }
            Console.WriteLine(sb.ToString() + "\r\n" + NumErrors.Count);
        }

        private static void PeopleBrowsrTest()
        {
            PeopleBrowsr.PeopleBrowsrAPI consumer = new PeopleBrowsr.PeopleBrowsrAPI("[app_id]", "[app_key]");
            List<KeyValuePair<string, string>> QueryParams = new List<KeyValuePair<string, string>>(
                new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>("last", "yesterday"),
                    new KeyValuePair<string, string>("count", "30"),
                    new KeyValuePair<string, string>("source", "twitter"),
                    new KeyValuePair<string, string>("term", "zombies"),
                    new KeyValuePair<string, string>("limit", "300")
                }
            );

            string[] methods = {
                "AtNameCloud",
                "Mentions",
                "Density",
                "WordCloud",
                "HashTagCloud",
                "MentionsRetweets",
                "FriendsAndFollowers",
                "TopFollowers",
                "PositiveTopFollowers",
                "NegativeTopFollowers",
                "Popularity",
                "Sentiment",
                "TopUsState",
                "TopCountries",
                "TopUrls",
                "TopPictures",
                "TopVideos",
                "KredOutreach",
                "KredInfluence"
            };
            List<string> NumErrors = new List<string>();
            List<IDictionary<string, object>> Results = new List<IDictionary<string, object>>();
            StringBuilder sb = new StringBuilder();
            foreach (var m in methods)
            {
                try
                {
                    Type t = consumer.GetType();
                    var result = (IDictionary<string, object>)t.InvokeMember(m,
                        BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                        null,
                        consumer,
                        new object[] { QueryParams }
                    );
                    Results.Add(result);
                    if (!result["status"].Equals("complete"))
                        throw new Exception("Status not complete!");
                    string raw = JsonParser.JsonParser.ToJson(result);
                    sb.AppendLine(raw);
                }
                catch(Exception err)
                {
                    NumErrors.Add(String.Format("{0} => {1}", m, err.Message));
                }
            }
            Console.WriteLine(sb.ToString() + "\r\n" + NumErrors.Count);
        }
    }
}
