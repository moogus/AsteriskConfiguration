using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace CrossSystemPresence
{
    class Program
    {
        public static UCAPIConnect Connection;

        static void Main(string[] args)
        {
            Connection = new UCAPIConnect();
            Console.WriteLine("UCAPI Connection is up");

            var host = new WebServiceHost(typeof(PickupService), new Uri("http://localhost:8000"));
            var endPoint = host.AddServiceEndpoint(typeof(IPickupService), new WebHttpBinding(), "");
            host.Open();
            Console.WriteLine("Web Server is running");
            Console.WriteLine("Press enter to end");
            Console.ReadLine();
        }
    }

    [ServiceContract]
    public interface IPickupService
    {
        [OperationContract]
        [WebGet]
        string Pickup(string from, string @to);
    }

    public class PickupService : IPickupService
    {
        public string Pickup(string @from, string @to)
        {
            Program.Connection.Pickup(from,to);
            return "Success";
        }
    }
}
