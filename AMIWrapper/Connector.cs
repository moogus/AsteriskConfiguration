using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asterisk.NET.Manager;
using Asterisk.NET.Manager.Action;

namespace AMIWrapper
{
    public class Connector
    {
        private readonly string _hostName;
        private ManagerConnection _manager;

        public Connector(string hostName)
        {
            _hostName = hostName;
        }

        public bool Connect()
        {
            _manager = new ManagerConnection(_hostName, 5038, "admin", "31994");
            try
            {
                // Uncomment next 2 line comments to Disable timeout (debug mode)
                // manager.DefaultResponseTimeout = 0;
                // manager.DefaultEventTimeout = 0;
                _manager.Login();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public void Disconnect()
        {
            _manager.Logoff();
        }

        public bool CallAsterisk(string extension, string variableName, string variable, string context)
        {
            try
            {
                var action = new OriginateAction { Channel = "SIP/" + extension, Context = context };
                action.SetVariable(variableName, variable);
                action.Priority = 1;
                action.Timeout = 10000;
                action.Async = true;
                _manager.SendAction(action, 0);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public enum BLFSates { Free, InCall, Ringing }

        public bool SetState(string extension, BLFSates state)
        {
            var prefix = "";
            switch (state)
            {
                case BLFSates.Free:
                    prefix = "nouse";
                    break;
                case BLFSates.InCall:
                    prefix = "inuse";
                    break;
                case BLFSates.Ringing:
                    prefix = "ringing";
                    break;
            }
            try
            {
                var action = new OriginateAction { Channel = "local/" + prefix + extension + "@CrossPresence", Context = "CrossPresence", Exten = prefix + extension };
                action.Priority = 1;
                action.Timeout = 10000;
                action.Async = true;
                _manager.SendAction(action, 0);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
