namespace ModelRepository.ModelInterfaces
{
  public enum QueueMemberType
  {
    Unknown,
    Extension,
    Queue
  }

  //Any changes done to this enum will need to be replicated in the database 
  public enum TrunkType
  {
    Sip,
    Bri,
    Iax
  }

  //Any changes done to this enum will need to be replicated in the database 
  public enum DDIUsedOn
  {
    NotUsed,
    Extension,
    Queue,
    Rule,
    Default
  }

  public enum FTPType
  {
    AutoAttendant
  }

  //Any changes done to this enum will need to be replicated in the database 
  public enum RoutingRuleDestination
  {
    Error,
    Extension,
    Group,
    Voicemail,
    External,
    Ringtone,
    Route,
    AutoAttendant,
    Playback,
    AddCode
  }

  //This enum should always mirror the enum above
  public enum ForwardingDestination
  {
    Error,
    Extension,
    Group,
    Voicemail,
    External
  }

  //Any changes done to this enum will need to be replicated in the database 
  public enum TrunkInPresentationType
  {
    StartWith,
    EndWith,
    RegExp,
    None
  }


  //Any changes done to this enum will need to be replicated in the database 
  public enum QueueStrategy
  {
    Ringall,
    Leastrecent,
    Fewestcalls,
    Random,
    Rrmemory,
    Linear,
    Wrandom
  }

  public enum MusicOnHoldMode
  {
    // ReSharper disable InconsistentNaming
    files,
    custom
    // ReSharper restore InconsistentNaming
  }

  public enum FederationType
  {
    FourCom,
    Samsung
  }
}