using System;
using System.ServiceModel;

namespace DatabaseServerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost userServiceHost = null;
            ServiceHost phraseServiceHost = null;
            try
            {
                userServiceHost = new ServiceHost(typeof(Kalambury.WcfServer.Services.UserService));
                userServiceHost.Open();
                Console.WriteLine("User service started at ");
                Console.WriteLine("\nlocalhost:8733/UserService");
                phraseServiceHost = new ServiceHost(typeof(Kalambury.WcfServer.Services.PhraseService));
                phraseServiceHost.Open();
                Console.WriteLine("Phrase service started at ");
                Console.WriteLine("\nlocalhost:8733/PhraseService");
            }
            catch (Exception ex)
            {
                userServiceHost = null;
                phraseServiceHost = null;
                Console.WriteLine("Service can not be started \n\nError message: " + ex.Message);
            }
            if (userServiceHost == null || phraseServiceHost == null) return;
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
            userServiceHost.Close();
            phraseServiceHost.Close();
        }
    }
}
