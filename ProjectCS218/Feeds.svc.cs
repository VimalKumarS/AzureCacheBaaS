using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WindowsBlogReader;
using System.Collections.ObjectModel;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Diagnostics;
using Microsoft.ApplicationServer.Caching;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading;

namespace ProjectCS218
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ServiceFeed : IServiceFeed
    {
        public const string CACHENAME = "cs218";
        private string local_Lock = string.Empty;
        public List<FeedData> GetFeedXml(string FeedName, string UserID)
        {
            Stopwatch stpwatch = new Stopwatch();
            stpwatch.Start();
            string strUrl = getFeedsURL(FeedName);
            getFeed(strUrl); //http://www.thestreet.com/topic/47781/rss.html
            Thread.Sleep(500);
            stpwatch.Stop();
            Trace.TraceInformation(@"Feed " + FeedName + " called for userID " + UserID +
                                     " ,Result fetched from API - result returned as XML "
                                    + stpwatch.ElapsedMilliseconds + " milli-sec");

            return this.Feeds;
        }

        public List<FeedData> GetFeedJson(string FeedName, string UserID)
        {
            Stopwatch stpwatch = new Stopwatch();
            stpwatch.Start();
            string strUrl = getFeedsURL(FeedName);
            getFeed(strUrl);
            Thread.Sleep(500);
            stpwatch.Stop();
            Trace.TraceInformation(@"Feed " + FeedName + " called for userID " + UserID +
                         " ,Result fetched from API- result returned as JSON "
                        + stpwatch.ElapsedMilliseconds + " milli-sec");
            return this.Feeds;
        }

        public string getFeedsURL(string strFeedName)
        {
            switch (strFeedName)
            {
                case "MostPopular":
                    return "http://www.forbes.com/most-popular/feed/";
                //break;
                case "Stocks":
                    return "http://articlefeeds.nasdaq.com/nasdaq/categories?category=Stocks";
                //break;
                case "Business":
                    return "http://www.forbes.com/business/index.xml";
                case "Technology":
                    return "http://www.forbes.com/technology/index.xml";
                case "Education":
                    return "http://www.forbes.com/education/index.xml";
                default:
                    return string.Empty;

            }
        }

        /// <summary>
        /// Get the cached version of the feed
        /// If data is cached it is retrieved from the cache
        /// If data is not in the cahce then it is stored in the cache and feed result is fetched
        /// </summary>
        /// <param name="FeedName"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<FeedData> GetFeedCacheJson(string FeedName, string UserID)
        {
            try
            {


                string _caheName = string.Empty;
                string _cacheService = string.Empty;
                lock (local_Lock)
                {
                    QueryCacheForFeed(FeedName, out _caheName, out _cacheService);
                }

                if (string.IsNullOrEmpty(_caheName) && string.IsNullOrEmpty(_cacheService))
                {
                    DataCacheFactory cachefactory = new DataCacheFactory(new DataCacheFactoryConfiguration(CACHENAME));
                    DataCache cache = cachefactory.GetCache("CS218NonExpiration"); //GetDefaultCache();  // TO.Do GetCache 

                    //object result = cache.Get(FeedName);

                    // if (result == null)
                    // {
                    Stopwatch stpwatch = new Stopwatch();
                    stpwatch.Start();
                    string strUrl = getFeedsURL(FeedName);
                    getFeed(strUrl);
                    DataCacheItemVersion ver = cache.Put(FeedName, this.Feeds);  // store result in cache
                    //cache.AddItemLevelCallback(FeedName, DataCacheOperations.RemoveItem, );
                    stpwatch.Stop();
                    Trace.TraceInformation(@"Feed " + FeedName + " called for userID " + UserID +
                                            " ,Total time ellapse for calling API and result stored in cache "
                                           + stpwatch.ElapsedMilliseconds + " milli-sec");
                    StoreCacheDetailInTable(CACHENAME, FeedName, "CS218NonExpiration");

                    return this.Feeds;

                }
                else
                {
                    List<FeedData> data =null;
                    lock (local_Lock)
                    {
                    DataCacheFactory cachefactory = new DataCacheFactory(new DataCacheFactoryConfiguration(_caheName));
                    DataCache cache = cachefactory.GetCache(_cacheService); //GetDefaultCache();  // TO.Do GetCache 
                   
                    //Trace.TraceInformation("Service Cache start time" + DateTime.Now.ToLongDateString());
                    Stopwatch stpwatch = new Stopwatch();
                    stpwatch.Start();
                    data = (List<FeedData>)cache.Get(FeedName);

                    stpwatch.Stop();
                    Trace.TraceInformation(@"Feed " + FeedName + " called for userID " + UserID +
                                             " ,Result Called from Cache "
                                            + stpwatch.ElapsedMilliseconds + " milli-sec");
                  
                        UpdateFeedCacheItem(_caheName, FeedName);
                    }
                    return data;
                }
            }
            catch (Exception exp)
            {

                Trace.TraceError(exp.Message + Environment.NewLine + exp.StackTrace);
                return null;
            }

        }

        private void UpdateFeedCacheItem(string cachename, string FeedName)
        {
            string connStr = CloudConfigurationManager.GetSetting("StorageConnectionString");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connStr);
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("cs218cache");
            TableOperation retrieveOperation = TableOperation.Retrieve<CS218CacheTable>(cachename, FeedName);
            TableResult retrievedResult = table.Execute(retrieveOperation);
            CS218CacheTable updateEntity = (CS218CacheTable)retrievedResult.Result;
            updateEntity.FetchCount = updateEntity.FetchCount + 1;
            updateEntity.LastFetchDate = DateTime.Now;
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(updateEntity);
            table.Execute(insertOrReplaceOperation);

        }
        private List<FeedData> _Feeds = new List<FeedData>();

        public List<FeedData> Feeds
        {
            get
            {
                return this._Feeds;
            }
            set
            {
                this._Feeds = value;
            }
        }
        public void getFeed(string url)
        {
            //Trace.TraceInformation("Service start time " + DateTime.Now.ToLongDateString());
            //Stopwatch stpwatch = new Stopwatch();
            // stpwatch.Start();
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);

            FeedData feedData = new FeedData();

            feedData.Title = feed.Title.Text;
            if (feed.Description != null && feed.Description.Text != null)
            {
                feedData.Description = feed.Description.Text;
            }

            feedData.PubDate = feed.LastUpdatedTime.DateTime;

            foreach (SyndicationItem item in feed.Items)
            {
                FeedItem feedItem = new FeedItem();
                feedItem.Title = item.Title.Text;
                feedItem.PubDate = item.PublishDate.DateTime;
                feedItem.Author = (item.Authors.Count != 0 ? item.Authors[0].Name : string.Empty);
                feedItem.Content = (item.Summary != null ? item.Summary.Text : string.Empty);
                feedItem.Link = item.Links[0].Uri;

                feedData.Items.Add(feedItem);
            }
            this.Feeds.Add(feedData);
            //stpwatch.Stop();
            //  Trace.TraceInformation("Service elapse time" + stpwatch.ElapsedMilliseconds);
            // Trace.TraceInformation("Service finish time" + DateTime.Now.ToLongDateString());


        }

        public void StoreCacheDetailInTable(string cahceName, string feedName, string cacheServiceName)
        {
            try
            {
                string connStr = CloudConfigurationManager.GetSetting("StorageConnectionString");
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connStr);
                CloudTableClient client = storageAccount.CreateCloudTableClient();
                CloudTable table = client.GetTableReference("cs218cache");
                // table.CreateIfNotExists();
                CS218CacheTable cacheTable = new CS218CacheTable();
                cacheTable.Id = Guid.NewGuid().ToString();
                cacheTable.CacheName = cahceName;
                cacheTable.FeedName = feedName;
                cacheTable.CacheServiceName = cacheServiceName;
                cacheTable.PartitionKey = cahceName;
                cacheTable.RowKey = feedName;
                cacheTable.CreatedDate = DateTime.Now;
                cacheTable.LastFetchDate = DateTime.Now;
                cacheTable.FetchCount = 0;
                TableOperation insertOperation = TableOperation.Insert(cacheTable);
                table.Execute(insertOperation);
            }
            catch (Exception exp)
            {

                Trace.TraceError(exp.Message + Environment.NewLine + exp.StackTrace);
            }
        }

        //IEnumerable<CS218CacheTable>
        public void QueryCacheForFeed(string feedName, out string cacheName, out string cacheServiceName)
        {
            string connStr = CloudConfigurationManager.GetSetting("StorageConnectionString");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connStr);
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("cs218cache");
            TableQuery<CS218CacheTable> query =
                    new TableQuery<CS218CacheTable>().
                    Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, feedName));
            IEnumerable<CS218CacheTable> tableresult = table.ExecuteQuery(query);
            cacheName = string.Empty;
            cacheServiceName = string.Empty;
            if (tableresult.Any())
            {
                cacheName = tableresult.ElementAt(0).CacheName;
                cacheServiceName = tableresult.ElementAt(0).CacheServiceName;
                // return table.ExecuteQuery(query);
            }
        }



    }
}


//http://www.nasdaq.com/services/rss.aspx
//Stocks
//http://articlefeeds.nasdaq.com/nasdaq/categories?category=Stocks
//technology
//http://www.forbes.com/technology/index.xml
//Business
//http://www.forbes.com/business/index.xml
//Education
//http://www.forbes.com/education/index.xml

