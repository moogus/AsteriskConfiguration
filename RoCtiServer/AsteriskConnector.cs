using System;
using System.Threading;
using Asterisk.NET.Manager;

namespace RoCtiServer
{
    public class AsteriskConnector
    {
        public delegate void MergedAsteriskEventHandler(AsteriskConnector sender, MergedAsteriskEventArgs e);
         
        private ManagerConnection _managerConnection;

        public AsteriskConnector()
        {
            _managerConnection = new ManagerConnection("10.10.20.188", 5038, "admin", "31994");
            _managerConnection.ConnectionState += (s, e) => HandleConnectionStateChanged(_managerConnection);
            _managerConnection.Login();

            WireUpEvents();
        }

        private static void HandleConnectionStateChanged(ManagerConnection managerConnection)
        {
            Console.WriteLine("Connection state: {0}", managerConnection.IsConnected());
           
        }

        private void WireUpEvents()
        {
            _managerConnection.FireAllEvents = true;
            _managerConnection.NewChannel += (sender, e) => FireCallEvent(new MergedAsteriskEventArgs(e));
            _managerConnection.Dial += (sender, e) => FireCallEvent(new MergedAsteriskEventArgs(e));
            _managerConnection.NewState += (sender, e) => FireCallEvent(new MergedAsteriskEventArgs(e));
            _managerConnection.NewCallerId += (sender, e) => FireCallEvent(new MergedAsteriskEventArgs(e));
            _managerConnection.Hangup += (sender, e) => FireCallEvent(new MergedAsteriskEventArgs(e));
        }

        public event MergedAsteriskEventHandler CallEvent;

        private void FireCallEvent(MergedAsteriskEventArgs e)
        {
            if (CallEvent != null)
                CallEvent(this, e);
        }
    }
}