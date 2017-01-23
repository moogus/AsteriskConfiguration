using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Asterisk.NET.Manager.Event;
using RoCtiServer.Annotations;

namespace RoCtiServer
{
    public enum CallState { Initializing, Ringing, RingingOtherEnd, OnCall, Terminated }

    internal class AsteriskCall : INotifyPropertyChanged
    {
        private static int callId;

        private int _callId;

        public int CallId { get { return _callId; } }

        private CallState _state;
        private string _otherEndNumber;
        public string Channel { get; private set; }
        public string OtherEndNumber
        {
            get { return _otherEndNumber; }
            private set { _otherEndNumber = value; OnPropertyChanged(); }
        }

        public bool Incoming { get; private set; }

        public CallState State
        {
            get { return _state; }
            private set { _state = value; OnPropertyChanged(); }
        }

        public AsteriskCall(NewChannelEvent e)
        {
            _callId = callId;
            callId++;
            State = CallState.Initializing;
            Channel = e.Channel;
            if (e.Attributes.ContainsKey("exten"))
            {
                OtherEndNumber = e.Attributes["exten"];
                Incoming = false;
            }
            else
            {
                Incoming = true;
            }
        }

        public void Handle(HangupEvent e)
        {
            State = CallState.Terminated;
        }

        public void Handle(NewStateEvent e)
        {
            switch (e.ChannelStateDesc)
            {
                case "Up":
                    State = CallState.OnCall;
                    break;
                case "Ringing":
                    State = CallState.Ringing;
                    if (e.Attributes.ContainsKey("connectedlinenum"))
                    {
                        OtherEndNumber = e.Attributes["connectedlinenum"];
                    }
                    break;
                case "Ring":
                    State = CallState.RingingOtherEnd;
                    break;
            }
        }

        public void Handle(TransferEvent e)
        {
            if (e.TransferExten!="")
                OtherEndNumber = e.TransferExten;
        }


        public void Handle(DialEvent e)
        {
            //if (e.CallerIdNum != "")
             //   OtherEndNumber = e.CallerIdNum;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}