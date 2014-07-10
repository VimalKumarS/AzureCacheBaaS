using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectCS218
{
    public class CS218CacheTable : TableEntity
    {
        public CS218CacheTable()
        {
        }
        public CS218CacheTable(string id, string cahceservicename,string cahcename, string feedname)
        {
            Id = id;
            CacheName = cahcename;
            FeedName = feedname;
            CacheServiceName = cahceservicename;
            this.PartitionKey = cahcename;
            this.RowKey = feedname;
           // PartitionKey = id.ToString();
           // RowKey = CacheName;
        }
        public string Id { get; set; }
        public string CacheServiceName { get; set; } 
        public string CacheName { get; set; }
        public string FeedName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastFetchDate { get; set; }
        public long FetchCount { get; set; }
    }
}
