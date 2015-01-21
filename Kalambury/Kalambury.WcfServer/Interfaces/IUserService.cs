using System.ServiceModel;
using System.ServiceModel.Web;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Interfaces
{
    [ServiceContract]
    public interface IUserService
    {
        [WebInvoke(UriTemplate = "/LoginUser/{username}/{password}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        User LoginUser(string username, string password);

        [WebInvoke(UriTemplate = "/AddNewUser/{username}/{password}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        User AddNewUser(string username, string password);

        [WebInvoke(UriTemplate = "/UpdateRanking/{usernames}/{points}", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        string UpdateRanking(string usernames, string points);

        [WebInvoke(UriTemplate = "/PingService", Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        string PingService();
    }
}
