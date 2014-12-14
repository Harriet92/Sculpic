using System;
using System.ServiceModel;

namespace DatabaseServerHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost svcHost = null;
            try
            {
                svcHost = new ServiceHost(typeof(Kalambury.WcfServer.Services.UserService));
                svcHost.Open(); 
                Console.WriteLine("Server started at ");
                Console.WriteLine("\nlocalhost:8733/UserService");
            }
            catch (Exception eX)
            {
                svcHost = null;
                Console.WriteLine("Service can not be started \n\nError Message [" + eX.Message + "]");
            }
            if (svcHost == null) return;
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
            svcHost.Close();
        }
    }
}
