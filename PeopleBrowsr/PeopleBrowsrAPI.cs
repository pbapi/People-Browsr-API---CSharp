using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace PeopleBrowsr
{
    /// <summary>
    /// Usage:
    /// PeopleBrowsrAPI consumer = new PeopleBrowsrAPI("[my app id]", "[my key]");
    /// List<KeyValuePair<string, string>> QueryParams = new List<KeyValuePair<string, string>>();
    /// QueryParams.Add(new KeyValuePair<string, string>("last", "yesterday"));
    /// QueryParams.Add(new KeyValuePair<string, string>("count", "30"));
    /// QueryParams.Add(new KeyValuePair<string, string>("source", "twitter"));
    /// QueryParams.Add(new KeyValuePair<string, string>("term", "zombies"));
    /// QueryParams.Add(new KeyValuePair<string, string>("limit", "300"));
    /// </summary>
    public class PeopleBrowsrAPI
    {
        private string Id, Key, BaseURL;

        public PeopleBrowsrAPI(string id, string key)
        {
            this.Id = id;
            this.Key = key;
            this.BaseURL = "http://api.peoplebrowsr.com";
        }

        public IDictionary<string, object> AtNameCloud(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("atnamecloud", QueryParams);
        }

        public IDictionary<string, object> Mentions(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("mentions", QueryParams);
        }

        public IDictionary<string, object> Density(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("density", QueryParams);
        }

        public IDictionary<string, object> WordCloud(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("wordcloud", QueryParams);
        }

        public IDictionary<string, object> HashTagCloud(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("hashtagcloud", QueryParams);
        }

        public IDictionary<string, object> MentionsRetweets(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("mentions-retweets", QueryParams);

        }

        public IDictionary<string, object> FriendsAndFollowers(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("friendsandfollowers", QueryParams);
        }

        public IDictionary<string, object> TopFollowers(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("top-followers", QueryParams);
        }

        public IDictionary<string, object> PositiveTopFollowers(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("Top-Positive-Followers", QueryParams);
        }

        public IDictionary<string, object> NegativeTopFollowers(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("wordcloud", QueryParams);
        }

        public IDictionary<string, object> Popularity(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("popularity", QueryParams);
        }

        public IDictionary<string, object> Sentiment(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("sentiment", QueryParams);
        }

        public IDictionary<string, object> TopUsState(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("top-usarea", QueryParams);
        }

        public IDictionary<string, object> TopCountries(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("topcountries", QueryParams);
        }

        public IDictionary<string, object> TopUrls(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("topurls", QueryParams);
        }

        public IDictionary<string, object> TopPictures(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("toppictures", QueryParams);
        }

        public IDictionary<string, object> TopVideos(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("topvideos", QueryParams);
        }

        public IDictionary<string, object> KredOutreach(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("kredoutreach", QueryParams);
        }

        public IDictionary<string, object> KredInfluence(List<KeyValuePair<string, string>> QueryParams)
        {
            return Fetch("kredinfluence", QueryParams);
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