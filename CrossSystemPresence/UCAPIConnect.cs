using System;
using System.Collections.Generic;
using System.Linq;
using AMIWrapper;
using UCAPI;
using UCAPI.Telephony;

namespace CrossSystemPresence
{
    public class UCAPIConnect
    {
        private readonly Connector _amiConnection;
        private TelephonySession _mainSession;

        public UCAPIConnect()
        {
            _amiConnection = new Connector("10.10.1.50");
            _amiConnection.Connect();

            _mainSession = new TelephonySession();
            _mainSession.Session.Connect("10.10.51.4", "2208");


            Device myDevice = null;
            if (_mainSession.Session.Connected == true)
            {
                 _mainSession.AsyncEvent += ParseEvent;
                _mainSession.MonitorAllDevices();
            }
        }

        public void Pickup(string from, string to)
        {

            Console.WriteLine("Getting device");
            var devices = _mainSession.Session.getAllExtensions();
            var device = devices.Cast<Device>().FirstOrDefault(device1 => device1.ShortName == from);

            Console.WriteLine("getting calls");
            var calls = device.getCalls();
            Console.WriteLine("got calls");
            foreach (Call call in calls)
            {
                if (call.State == Enums.ConnectionState.Ringing)
                {
                    Console.WriteLine("Transfering to "+to);
                    call.TransferBlind(to);
                }
            }

            Console.WriteLine("Done");
        }

        private void ParseEvent(BaseEvent e)
        {
            if (e.Type != EventType.Device) return;
            var ee = (DeviceEvent)e;
            if (ee.Device.ShortName == "2208")
            {
                Console.WriteLine(ee.Device.State);
                switch (ee.Device.State)
                {
                    case Enums.DeviceState.Onhook:
                        _amiConnection.SetState(ee.Device.ShortName, Connector.BLFSates.Free);
                        break;
                    case Enums.DeviceState.OffHook:
                        _amiConnection.SetState(ee.Device.ShortName, Connector.BLFSates.InCall);
                        break;
                    case Enums.DeviceState.Ringing:
                        _amiConnection.SetState(ee.Device.ShortName, Connector.BLFSates.Ringing);
                        break;
                }
            }
        }
    }
}