using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Interfaces
{
    [ServiceContract]
    public interface IUserService
    {
        [WebInvoke(UriTemplate = "/LoginUser", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        User LoginUser(string username, string password);

        [WebInvoke(UriTemplate = "/AddNewUser", Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        User AddNewUser(string username, string password);

        [WebInvoke(UriTemplate = "/PingService", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        string PingService();
    }
}
