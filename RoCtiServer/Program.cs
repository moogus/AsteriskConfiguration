using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asterisk.NET.Manager;
using Asterisk.NET.Manager.Event;

namespace RoCtiServer
{
    class Program
    {
        private static Dictionary<string, AsteriskCall> _callsLookup;
        private static ObservableCollection<AsteriskCall> _calls;

        private static ManagerConnection _managerConnection;

        static void Main(string[] args)
        {
            _managerConnection = new ManagerConnection("10.10.20.188", 5038, "admin", "31994");
            _managerConnection.ConnectionState += (s, e) => Console.WriteLine("Connection state: {0}", _managerConnection.IsConnected());
            _managerConnection.Login();

            _callsLookup = new Dictionary<string, AsteriskCall>();
            _calls = new ObservableCollection<AsteriskCall>();

            WireUpEvents();

            _calls.CollectionChanged += (s, e) =>
                                            {
                                                switch (e.Action)
                                                {
                                                    case NotifyCollectionChangedAction.Add:
                                                        foreach (AsteriskCall call in e.NewItems)
                                                        {
                                                            

                                                                Console.WriteLine("{1} New Call on {0}", call.Channel, call.CallId);
                                                                call.PropertyChanged +=
                                                                    (ss, ee) =>
                                                                    {
                                                                        Console.WriteLine("{1} State: {0}", call.State, call.CallId);
                                                                        Console.WriteLine("{1} OON: {0}",
                                                                                          call.OtherEndNumber, call.CallId);
                                                                    };
                                                            
                                                        }
                                                        break;
                                                }
                                            };
            Console.ReadLine();
        }

        private static void WireUpEvents()
        {
            _managerConnection.FireAllEvents = true;

            _managerConnection.NewChannel += (s, e) =>
                                                 {
                                                     var call = new AsteriskCall(e);
                                                     _callsLookup.Add(e.UniqueId, call);
                                                     _calls.Add(call);
                                                 };
            _managerConnection.Hangup += (sender, e) =>
                                             {
                                                 if (_callsLookup.ContainsKey(e.UniqueId))
                                                     _callsLookup[e.UniqueId].Handle(e);
                                             };
            _managerConnection.NewState += (sender, e) =>
                                               {
                                                   if (_callsLookup.ContainsKey(e.UniqueId))
                                                       _callsLookup[e.UniqueId].Handle(e);
                                               };
            _managerConnection.Dial += (sender, e) =>
                                                {
                                                    if (_callsLookup.ContainsKey(e.UniqueId))
                                                        _callsLookup[e.UniqueId].Handle(e);
                                                };
            _managerConnection.UnhandledEvent += (sender, e) =>
                                                     {
                                                         var te = (e as TransferEvent);
                                                         if (te != null)
                                                             if (_callsLookup.ContainsKey(te.TargetUniqueId))
                                                                 _callsLookup[te.TargetUniqueId].Handle(te);
                                                     };

        }
    }


}