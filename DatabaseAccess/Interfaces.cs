using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using DatabaseAccess.DatabaseTables;
using DatabaseAccess.Models;

namespace DatabaseAccess
{

  public interface IModel
  {
    object Under { get; }
    ISessionWrapper Session { get; }
    void ExtraUpdate();
    void ExtraDelete();

  }

  public interface IDDI : IModel
  {
    int Id { get; }
    string DDINumber { get; set; }
    ITrunk Trunk { get; set; }
    DDIUsedOn UsedOn { get; set; }
    string Used { get; }
  }

  public interface ICLI : IModel
  {
    int Id { get; }
    string CLINumber { get; set; }
    string CLIName { get; set; }
    ITrunk Trunk { get; set; }
  }

  public interface IVoiceMessage : IModel
  {
    int Id { get; }
    string Folder { get; set; }
    string CallerId { get; }
    string CallerNumber { get; }
    DateTime CalledAt { get; }
    int Duration { get; }
    IVoiceMail MailBox { get; }
    TimeSpan TimeSinceEdited { get; }
    IAsteriskAudioStream Audiostream { get; }
  }

  public interface IKnownNumber : IModel
  {
    int Id { get; }
    string Number { get; set; }
    string Description { get; set; }
    bool IsInternal { get; set; }
  }

  public interface IEmergencyNumber : IModel
  {
    int Id { get; }
    string Number { get; set; }
    string Description { get; set; }
  }

  public interface IPattern : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string Pattern { get; set; }
  }

  public interface IPermisionClass : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string Description { get; set; }
    List<IPermissionClassMember> PermissionClassMemebers { get; set; }
    void AddPermissionClassMemeber(IPermissionClassMember permissionClassMember);
  }

  public interface IPermissionClassMember : IModel
  {
    int Id { get; }
    IPermisionClass ParentPermissionClass { get; set; }
    IPattern Pattern { get; set; }
    IDialplan Dialplan { get; set; }
  }

  public interface IMusicOnHold : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string Directory { get; }
    string Application { get; set; }
    MusicOnHoldMode Mode { get; }
    char Digit { get; set; }
    bool Sort { get; set; }
    string Format { get; set; }
    bool Random { get; set; }
  }

  public enum MusicOnHoldMode
  {
    // ReSharper disable InconsistentNaming
    files, custom
    // ReSharper restore InconsistentNaming
  }

  public interface IFederation : IModel
  {
    int Id { get; }
    string Name { get; set; }
    FederationType Type { get; set; }
    string Description { get; set; }
    IExtension Extension { get; }
    ITrunk Trunk { get; }
    IRoutingRule RoutingRule { get; }

    bool SetFederationValues(string name, string accessCode, string password, ITrunk trunk);
  }

  public enum FederationType
  {
    FourCom, Samsung
  }

  public interface IAsteriskAudioStream : IIsForwardable
  {
    Stream Stream { get; }
  }

  public interface IIsForwardable
  {
    Stream ForwardableStream { get; }
  }

  public interface IServer : IModel
  {
    int Id { get; }
    string IpAddress { get; set; }
    NetworkCredential Credentials { get; set; }
    SmtpClient MailServer { get; set; }
    string VoicemailDialNumber { get; set; }
    IExtension AdminExtension { get; set; }
    string ExtensionIpRange { get; set; }
  }

  public interface IExtension : IModel
  {
    string IpAddress { get; }
    string Model { get; }
    int Id { get; }
    string Status { get; }
    string Password { get; set; }
    string Notes { get; set; }
    string Number { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Department { get; set; }
    string Email { get; set; }
    string JobTitle { get; set; }
    IDDI DDI { get; set; }
    ICLI CLI { get; set; }
    bool DND { get; set; }
    IVoiceMail VoiceMail { get; set; }
    int VoicemailDelay { get; set; }
    IPermisionClass PermisionClass { get; set; }
    bool IncludeInDirectory { get; set; }
  }


  public interface IRoutingRule : IModel
  {
    int Id { get; }
    IDialplan Dialplan { get; set; }
    string Number { get; set; }
    int Time { get; set; }
    int Order { get; set; }
    RoutingRuleDestination DestinationType { get; set; }
    string DestinationNumber { get; set; }
  }

  public interface IVoiceMail : IModel
  {
    int Id { get; }
    string Number { get; set; }
    string Password { get; set; }
    int NumberOfMessages { get; set; }
    int MessageLength { get; set; }
    int HeldNumberOfMessages { get; set; }
    string DefaultEmail { get; set; }
    bool EmailNotificationHasMp3 { get; set; }
    IEnumerable<VoiceMessageFolder> MessageFolders { get; }
  }

  public interface IDialplan : IModel, IEquatable<IDialplan>
  {
    int Id { get; }
    string Name { get; set; }
  }

  public interface IDefault : IModel
  {
    int Id { get; set; }
    string Type { get; set; }
    int Index { get; set; }
    string JavascriptColumnType { get; set; }
    string ColumnTitle { get; set; }
    string JavascriptProperty { get; set; }
    string DefaultValue { get; set; }
    string Picker { get; set; }
  }

  public interface IDialplanDate : IModel
  {
    int Id { get; }
    DateTime StartDate { get; set; }
    DateTime EndDate { get; set; }
    IDialplan Dialplan { get; set; }
  }

  public interface IDialplanRange : IModel
  {
    int Id { get; }
    string DaysOfWeek { get; set; }
    string TimeRange { get; set; }
    int Priority { get; set; }
    IDialplan Dialplan { get; set; }
  }
  public interface IQueue : IModel
  {
    int Id { get; }
    string Number { get; set; }
    string Notes { get; set; }
    string QueueName { get; set; }
    string Department { get; set; }
    QueueStrategy Strategy { get; set; }
    bool RingOnBusy { get; set; }
    string DDINumber { get; set; }
    string CLINumber { get; set; }
    IVoiceMail VoiceMail { get; set; }
    int VoicemailDelay { get; set; }
    IMusicOnHold MusicOnHold { get; set; }

    IEnumerable<IQueueMember> QueueMembers { get; }
    bool IncludeInDirectory { get; set; }

    void AddExtensionAsQueueMember(IExtension extension, int queueName);
    void AddQueueAsQueueMember(IQueue queueToAdd, int queueName);
  }

  public interface IQueueMember : IModel
  {
    int Id { get; }
    IQueue ParentQueue { get; set; }
    int Penalty { get; set; }
    int Paused { get; set; }
    QueueMemberType Type { get; }
    IExtension Extension { get; set; }
    IQueue Queue { get; set; }
  }

  public interface ITrunk : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string DefaultDestination { get; set; }
    TrunkType TrunkType { get; set; }
    TrunkInPresentationType TrunkInPresentationType1 { get; set; }
    string TrunkInPresentationValue1 { get; set; }
    TrunkInPresentationType TrunkInPresentationType2 { get; set; }
    string TrunkInPresentationValue2 { get; set; }
    IEnumerable<IDDI> DDIs { get; }
    List<AccessCodeAndPriority> AccessCodes { get; }
    bool SetAccessCodes(string accessCodes);
  }

  public interface IBriTrunk :ITrunk
  {
    List<string> DahdiChannels { get; set; } 
  }

  public interface ISipTrunk : ITrunk
  {
    string SipUserName { get; set; }
    string SipPassword { get; set; }
    string SipHost { get; set; }
    int SipAllowedChannles { get; set; }
  }

  public interface IIaxTrunk : ITrunk
  {
    string IaxName { get; set; }
    string HostAddress { get; set; }
    int IaxAllowedChannles { get; set; }
  }

  //TODO:this is wrong do we go back to just putting in IAccessCode???
  public class AccessCodeAndPriority
  {
    public int Priority { get; set; }
    public string AccessCode { get; set; }
  }

  public interface IUserConfig : IModel
  {
    int Id { get; set; }
    string Number { get; set; }
    string Password { get; set; }
    string Role { get; set; }
    IExtension UserExtension { get; }
  }
  public interface ICurrentDialPlan : IModel
  {
    IDialplan Dialplan { get; set; }
    bool AutomaticallyChange { get; set; }
  }

  public interface IRingTone : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string SipHeader { get; set; }
  }

  public interface IAutoAttendant : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string Announcement { get; set; }
    int Timeout { get; set; }
    IEnumerable<IAutoAttendantRules> Rules { get; }
  }
  public interface IAutoAttendantRules : IModel
  {
    int Id { get; }
    string AaName { get; set; }
    string Entry { get; set; }
    string DestinationNumber { get; set; }
    RoutingRuleDestination DestinationType { get; set; }
  }

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
    StartWith, EndWith, RegExp, None
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
}

