using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CTIServer.CallHandler.CallHandlers;
using CTIServer.ConnectionManger.ConnectionManagers;
using CTIServer.Phone.Phones;
using CTIServer.PhoneManager.PhoneManagers;
using CTIServer.Presence;
using DatabaseAccess;

namespace PresenceTCPServer
{
    class Program
    {
        private static List<AsteriskPhone> _allPhones;

        private static List<string> _favourites; 

        static void Main(string[] args)
        {
            _allPhones = new Repository().GetList<IExtension>().Select(e => new AsteriskPhone(e.Number, e.DDINumber, e.FirstName + " " + e.LastName, e.Department)).ToList();
            _favourites=new List<string>();
            _favourites.Add("100");

            var server = new Server(commandHandler);

            var managerConnection = new AsteriskConnectionManager("10.10.1.50", 5038, "admin", "31994");
            var callHandler = new AsteriskCallHandler(managerConnection);
            var manager = new AsteriskPhoneManager(_allPhones, managerConnection, callHandler);


            foreach (var phone in _allPhones)
            {
                phone.PropertyChanged += (s, e) =>
                                             {
                                                 var p = (AsteriskPhone) s;
                                                 if (e.PropertyName == "State")
                                                 {
                                                     var message = String.Format("newState:{0};{1}", p.ExtensionNumber,
                                                                                 p.State.ToString().Substring(0, 1));
                                                     Console.WriteLine(message);
                                                     server.SendToAllClients(message);
                                                 }
                                             };
            }

            Console.ReadLine();
        }

        static private string commandHandler(string command)
        {
            string parameter = "";
            if (command.Contains(':'))
            {
                var split = command.Split(':');
                command = split[0];
                parameter = split[1];
            }
            switch (command)
            {
                case "listAllExtns":
                    Console.WriteLine("sending list");
                    var builder = String.Join(",",
                                              _allPhones.Select(
                                                  p =>
                                                  string.Format("{0};{1};{2};{3}", p.ExtensionNumber,
                                                                p.State.ToString().Substring(0, 1),p.Name,String.IsNullOrEmpty(p.Department) ? " " : p.Department)));
                    
                    return "extnList:"+builder;
                case "addToFavourites":
                    Console.WriteLine("adding {0} to favourites", parameter);
                    _favourites.Add(parameter);
                    return commandHandler("listAllFavourites");
                case "removeFromFavourites":
                    Console.WriteLine("remove {0} from favourites", parameter);
                    _favourites.Remove(parameter);
                    return commandHandler("listAllFavourites");
                case "listAllFavourites":
                    return "favourites:"+string.Join(",",_favourites);
                case "connected":
                    return commandHandler("listAllExtns") +"\n"+ commandHandler("listAllFavourites");
            }
            return "";
        }
    }
}
