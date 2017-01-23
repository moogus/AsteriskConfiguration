using Asterisk.NET.Manager.Event;

namespace RoCtiServer
{
    public enum MergedAsteriskEventType
    {
        Dial,
        NewChannel,
        NewState,
        NewCallerId,
        Hangup
    }

    public class MergedAsteriskEventArgs
    {
        public MergedAsteriskEventArgs(DialEvent e)
        {
            Type = MergedAsteriskEventType.Dial;
            UniqueId = e.UniqueId;
            DialStatus = e.DialStatus ?? "";
            SubEvent = e.SubEvent ?? "";
            Channel = e.Channel ?? "";
            ChannelNumber = !string.IsNullOrEmpty(e.Channel) ? e.Channel.Substring(4).Split('-')[0] : "";
            ChannelStateDesc = "";
            CallerIdNum = e.CallerIdNum ?? "";
            NumberDialed = e.DialString;
        }

        public MergedAsteriskEventArgs(NewChannelEvent e)
        {
            Type = MergedAsteriskEventType.NewChannel;
            UniqueId = e.UniqueId;
            DialStatus = "";
            SubEvent = "";
            Channel = e.Channel ?? "";
            ChannelNumber = !string.IsNullOrEmpty(e.Channel) ? e.Channel.Substring(4).Split('-')[0] : "";
            ChannelStateDesc = "";
            CallerIdNum = e.CallerIdNum ?? "";
            NumberDialed = e.Attributes.ContainsKey("exten") ? e.Attributes["exten"] : "";
        }

        public MergedAsteriskEventArgs(NewCallerIdEvent e)
        {
            Type = MergedAsteriskEventType.NewCallerId;
            UniqueId = e.UniqueId;
            DialStatus = "";
            SubEvent = "";
            Channel = e.Channel ?? "";
            ChannelNumber = !string.IsNullOrEmpty(e.Channel) ? e.Channel.Substring(4).Split('-')[0] : "";
            ChannelStateDesc = "";
            CallerIdNum = e.CallerIdNum ?? "";
            NumberDialed = "";
        }

        public MergedAsteriskEventArgs(NewStateEvent e)
        {
            Type = MergedAsteriskEventType.NewState;
            UniqueId = e.UniqueId;
            DialStatus = "";
            SubEvent = "";
            Channel = e.Channel ?? "";
            ChannelNumber = !string.IsNullOrEmpty(e.Channel) ? e.Channel.Substring(4).Split('-')[0] : "";
            ChannelStateDesc = e.ChannelStateDesc ?? "";
            CallerIdNum = e.CallerIdNum ?? "";
            NumberDialed = "";
        }

        public MergedAsteriskEventArgs(HangupEvent e)
        {
            Type = MergedAsteriskEventType.Hangup;
            UniqueId = e.UniqueId;
            DialStatus = "";
            SubEvent = "";
            Channel = e.Channel ?? "";
            ChannelNumber = !string.IsNullOrEmpty(e.Channel) ? e.Channel.Substring(4).Split('-')[0] : "";
            ChannelStateDesc = e.ChannelStateDesc ?? "";
            CallerIdNum = e.CallerIdNum ?? "";
            NumberDialed = "";
        }

        public MergedAsteriskEventType Type { get; private set; }
        public string DialStatus { get; private set; }
        public string SubEvent { get; private set; }
        public string UniqueId { get; private set; }
        public string Channel { get; private set; }
        public string ChannelNumber { get; set; }
        public string ChannelStateDesc { get; private set; }
        public string CallerIdNum { get; private set; }
        public string NumberDialed { get; private set; }
    }
}