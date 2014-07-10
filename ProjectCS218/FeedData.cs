﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ServiceModel.Syndication;
//using Windows.Web.Syndication;
using System.Collections.ObjectModel;
using System.Xml;
using System.Runtime.Serialization;

//C:\Program Files (x86)\Microsoft SDKs\Silverlight\v4.0\Libraries\Client
namespace WindowsBlogReader
{
    // FeedData
    // Holds info for a single blog feed, including a list of blog posts (FeedItem)
    [DataContract]
    public class FeedData
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]

        public string Description { get; set; }
        [DataMember]

        public DateTime PubDate { get; set; }

      
        private List<FeedItem> _Items = new List<FeedItem>();

        [DataMember]
        public List<FeedItem> Items
        {
            get
            {
                return this._Items;
            }
            set {this._Items = value; }
        }
    }

    // FeedItem
    // Holds info for a single blog post
    [DataContract]
    public class FeedItem
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Author { get; set; }
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public DateTime PubDate { get; set; }
        [DataMember]
        public Uri Link { get; set; }
    }

}