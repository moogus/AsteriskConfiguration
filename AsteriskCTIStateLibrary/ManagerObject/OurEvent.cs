using Asterisk.NET.Manager.Event;
using AsteriskCTIStateLibrary.Enums;

namespace AsteriskCTIStateLibrary.ManagerObject
{
  public class OurEvent
  {
    public SIPEvent Type { get; private set; }
    public string DialStatus { get; private set; }
    public string SubEvent { get; private set; }
    public string UniqueId { get; private set; }
    public string Channel { get; private set; }
    public string ChannelNumber { get; set; }
    public string ChannelStateDesc { get; private set; }
    public string CallerIdNum { get; private set; }
    public string NumberDialed { get; private set; }

    public OurEvent(DialEvent e)
    {
      Type = SIPEvent.Dial;
      UniqueId = e.UniqueId;
      DialStatus = e.DialStatus ?? "";
      SubEvent = e.SubEvent ?? "";
      Channel = e.Channel ?? "";
      ChannelNumber =!string.IsNullOrEmpty(e.Channel)? e.Channel.Substring(4).Split('-')[0] : "";
      ChannelStateDesc = "";
      CallerIdNum = e.CallerIdNum ?? "";
      NumberDialed = "";
    }

    public OurEvent(NewChannelEvent e)
    {
      Type = SIPEvent.NewChannel;
      UniqueId = e.UniqueId;
      DialStatus ="";
      SubEvent = "";
      Channel = e.Channel ?? "";
      ChannelNumber = !string.IsNullOrEmpty(e.Channel) ? e.Channel.Substring(4).Split('-')[0] : "";
      ChannelStateDesc = "";
      CallerIdNum = e.CallerIdNum ?? "";
      NumberDialed = e.Attributes.ContainsKey("exten") ? e.Attributes["exten"] : "";
    }

    public OurEvent(NewCallerIdEvent e)
    {
      Type = SIPEvent.NewCallerId;
      UniqueId = e.UniqueId;
      DialStatus = "";
      SubEvent = "";
      Channel = e.Channel ?? "";
      ChannelNumber = !string.IsNullOrEmpty(e.Channel) ? e.Channel.Substring(4).Split('-')[0] : "";
      ChannelStateDesc = "";
      CallerIdNum = e.CallerIdNum ?? "";
      NumberDialed = "";
    }

    public OurEvent(NewStateEvent e)
    {
      Type = SIPEvent.NewState;
      UniqueId = e.UniqueId;
      DialStatus = "";
      SubEvent = "";
      Channel = e.Channel ?? "";
      ChannelNumber = !string.IsNullOrEmpty(e.Channel) ? e.Channel.Substring(4).Split('-')[0] : "";
      ChannelStateDesc = e.ChannelStateDesc??"";
      CallerIdNum = e.CallerIdNum ?? "";
      NumberDialed = "";
    }

  }
}
