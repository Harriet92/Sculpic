using System.ServiceModel;
using System.ServiceModel.Web;

namespace Kalambury.WcfServer.Interfaces
{
    [ServiceContract]
    public interface IPhraseService
    {
        [WebInvoke(UriTemplate = "/DrawPhrase", Method = "GET",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        [OperationContract]
        string DrawPhrase();
    }
}
