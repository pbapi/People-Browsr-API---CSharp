using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;

namespace Kred
{
    /// <summary>
    /// Usage:
    /// KredAPI consumer = new KredAPI("[my app id]", "[my key]");
    /// List<KeyValuePair<string, string>> QueryParams = new List<KeyValuePair<string, string>>();
    /// QueryParams.Add(new KeyValuePair<string, string>("source", "twitter"));
    /// QueryParams.Add(new KeyValuePair<string, string>("term", "zombies"));
    /// </summary>
    public class KredAPI
    {
        private string Id, Key, BaseURL;

        public KredAPI(string id, string key)
        {
            this.Id = id;
            this.Key = key;
            this.BaseURL = "http://api.kred.com";
        }

        public IDictionary<string, object> Kred(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("kred", QueryParams);
        }

        public IDictionary<string, object> KredScore(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("kredscore", QueryParams);
        }

        /// <summary>
        /// This method uses the open source Json parser found here:
        /// https://github.com/danielcrenna/json
        /// </summary>
        private IDictionary<string, object> Fetch(string page, List<KeyValuePair<string, string>> QueryParams)
        {
            string url = String.Format("{0}/{1}?app_id={2}&app_key={3}&{4}",
                this.BaseURL, page, Id, Key, String.Join("&", QueryParams.Select(n => String.Format("{0}={1}", n.Key, n.Value))));

            IDictionary<string, object> data = null;
            int NumTries = 0;
            do
            {
                if (NumTries++ > 0)
                    Thread.Sleep(500);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (var rs = new StreamReader(response.GetResponseStream()))
                        data = JsonParser.JsonParser.FromJson(rs.ReadToEnd());
                }
            }
            while (!data["status"].Equals("complete"));

            return data;
        }
    }
}