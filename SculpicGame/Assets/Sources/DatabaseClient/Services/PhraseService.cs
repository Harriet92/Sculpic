using Assets.Sources.DatabaseClient.REST;

namespace Assets.Sources.DatabaseClient.Services
{
    public class PhraseService
    {
        public static string LastError;
        private static string ServiceEndpoint = RestCommunication.BaseURL + "/PhraseService";
        private readonly RestCommunication restCommunication;
        public PhraseService()
        {
            LastError = string.Empty;
            restCommunication = new RestCommunication();
        }

        public string DrawPhrase()
        {
            var url = ServiceEndpoint + "/DrawPhrase";
            return restCommunication.SendAndReceive<string>(url);
        }

    }
}
