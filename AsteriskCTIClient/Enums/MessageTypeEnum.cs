namespace AsteriskCTIClient.Enums
{
  //TODO separation required this is on too many levels
  public enum MessageTypeEnum
  {
    DialInternalRing,
    DialInternalOnCall,
    DialInternalHangup,
    DialExternalRing,
    DialExternalOnCall,
    DialExternalHangup,
    IncommingRing,
    IncomingOnCall,
    IncomingHangup,
    InvalidDial,
    NumberToDial
  }
}