using System.ServiceModel;
using System.ServiceModel.Web;
using Kalambury.WcfServer.Models;

namespace Kalambury.WcfServer.Interfaces
{
    [ServiceContract]
    public interface IRoomService
    {
        [WebInvoke(UriTemplate = "/SetUpNewRoom/{userId}/{gameName}/{password}/{usersLimit}", Method = "GET",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        bool SetUpNewRoom(string userId, string gameName, string password, string usersLimit);
    }
}
