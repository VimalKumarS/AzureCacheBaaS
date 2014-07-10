using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WindowsBlogReader;

namespace ProjectCS218
{
    [ServiceContract]
    public interface IServiceFeed
    {
        [OperationContract]
        [WebGet(UriTemplate = "/GetFeedXml/{FeedName}/{UserID}",
               ResponseFormat = WebMessageFormat.Xml,
                RequestFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Bare)]
        List<FeedData> GetFeedXml(string FeedName,string UserID);

        [OperationContract]
        [WebGet(UriTemplate = "/GetFeedJson/{FeedName}/{UserID}",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Bare)]
        List<FeedData> GetFeedJson(string FeedName,string UserID);

        [OperationContract]
        [WebGet(UriTemplate = "/GetFeedCacheJson/{FeedName}/{UserID}",
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Bare)]
        List<FeedData> GetFeedCacheJson(string FeedName, string UserID);
    }
}
