using System;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  public class Extension : IExtension
  {
    private readonly FuContactDetails _contactDetails;
    private readonly AstExtension _internalAstExtension;
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly AstSipFriend _under;
    private readonly AstExtension _voicemailExtension;
    private AstExtension _ddiAstExtension;

    public Extension(AstSipFriend under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;

      _contactDetails = session.Query<FuContactDetails>().FirstOrDefault(d => d.FuExtensionNumber == under.Number);
      _internalAstExtension =
          session.Query<AstExtension>().FirstOrDefault(
              e =>
              (e.Appdata == "SIP/" + under.Number || e.Appdata.StartsWith("SIP/" + under.Number + ",")) &&
              e.Context == "LocalSets");

      if (_internalAstExtension != null && _internalAstExtension.Appdata.Contains(","))
      {
        VoicemailDelay = int.Parse(_internalAstExtension.Appdata.Split(',')[1]);
      }
      _ddiAstExtension =
          session.Query<AstExtension>().FirstOrDefault(
              e => e.Appdata == "SIP/" + under.Number && e.Context == "incoming");
      DDINumber = _ddiAstExtension == null ? "" : _ddiAstExtension.ExtensionNumber;


      _voicemailExtension =
          session.Query<AstExtension>().FirstOrDefault(
              e =>
              e.App == "Macro" && e.ExtensionNumber == under.Number && e.Context == "LocalSets" &&
              e.Appdata.StartsWith("voicemail,"));
      if (_voicemailExtension != null)
      {
        VoiceMail = _voicemailExtension == null
                        ? null
                        : _repository.GetFromName<IVoiceMail>(_voicemailExtension.Appdata.Split(',')[1]);
      }
    }

    public Extension(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;

      _under = new AstSipFriend
                   {
                     Host = "Dynamic",
                     Type = "friend",
                     Context = "LocalSets",
                     SubscribeContext = "BLF_Group_1",
                     CallGroup = 1,
                     PickupGroup = 1,
                     AllowSubscribe = "yes",
                     NotifyRinging = "yes",
                     NotifyHold = "yes",
                     NotifyCid = "yes",
                     CallLimit = 2
                   };

      _contactDetails = new FuContactDetails();
      _internalAstExtension = new AstExtension
                                  {
                                    Context = "LocalSets",
                                    Priority = 1,
                                    App = "Dial",
                                    DestinationNumber = "",
                                    DestinationType = ""
                                  };
    }

    #region IExtension Members

    public int Id
    {
      get { return _under.Id; }
    }

    public string Number
    {
      get { return _under.Number; }
      set
      {
        _under.Number = value;
        _contactDetails.FuExtensionNumber = value;
        _internalAstExtension.ExtensionNumber = value;

        if (_ddiAstExtension != null)
        {
          _ddiAstExtension.Appdata = "SIP/" + value;
        }
      }
    }

    public string Password
    {
      get { return _under.Password; }
      set { _under.Password = value; }
    }

    public string IpAddress
    {
      get { return _under.IpAddress; }
    }

    public string Model
    {
      get { return _under.Model; }
    }

    public string Notes
    {
      get { return _contactDetails.Notes; }
      set { _contactDetails.Notes = value; }
    }

    public string FirstName
    {
      get { return _contactDetails.FirstName; }
      set { _contactDetails.FirstName = value; }
    }

    public string LastName
    {
      get { return _contactDetails.LastName; }
      set { _contactDetails.LastName = value; }
    }

    public string Department
    {
      get { return _contactDetails.Department; }
      set { _contactDetails.Department = value; }
    }

    public string Email
    {
      get { return _contactDetails.Email; }
      set { _contactDetails.Email = value; }
    }

    public string JobTitle
    {
      get { return _contactDetails.JobTitle; }
      set { _contactDetails.JobTitle = value; }
    }

    public string Status
    {
      get
      {
        if (_under.StatusTime < 1)
          return "never used";
        return ConvertFromUnixToDateTime(_under.StatusTime) > DateTime.UtcNow
                   ? "active"
                   : "not active";
      }
    }

    public string DDINumber { get; set; }

    public int ForwardingType { get; set; }
    

    public IVoiceMail VoiceMail { get; set; }

    public int VoicemailDelay { get; set; }

    object IModel.Under
    {
      get { return _under; }
    }

    ISessionWrapper IModel.Session
    {
      get { return _session; }
    }

    public void ExtraUpdate()
    {
      _internalAstExtension.Appdata = "SIP/" + Number;
      if (VoiceMail != null)
        _internalAstExtension.Appdata += "," + VoicemailDelay;


      _session.SaveOrUpdate(_internalAstExtension);
      _session.SaveOrUpdate(_contactDetails);

      if (_ddiAstExtension != null)
      {
        SaveOrDeleteDDIExtension(DDINumber);
      }
      else
      {
        CreateNewDDIExtension(DDINumber);
      }

      if (_voicemailExtension != null)
      {
        SaveOrDeleteVoicemailExtension(VoiceMail);
      }
      else
      {
        CreateNewVoicemailExtension(VoiceMail);
      }
    }

    public void ExtraDelete()
    {
      _session.Delete(_internalAstExtension);
      _session.Delete(_contactDetails);

      if (_ddiAstExtension != null)
        _session.Delete(_ddiAstExtension);

      if (_voicemailExtension != null)
        _session.Delete(_voicemailExtension);
    }

    #endregion

    private static DateTime ConvertFromUnixToDateTime(double timestamp)
    {
      var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
      return origin.AddSeconds(timestamp);
    }

    private void SaveOrDeleteDDIExtension(string dDiNumber)
    {
      if (!string.IsNullOrEmpty(dDiNumber))
      {
        _ddiAstExtension.ExtensionNumber = dDiNumber;
        _session.SaveOrUpdate(_ddiAstExtension);
      }
      else
        _session.Delete(_ddiAstExtension);
    }

    private void CreateNewDDIExtension(string dDiNumber)
    {
      if (!string.IsNullOrEmpty(dDiNumber))
      {
        _ddiAstExtension = new AstExtension
                               {
                                 Context = "incoming",
                                 Priority = 1,
                                 App = "Dial",
                                 Appdata = "SIP/" + Number,
                                 ExtensionNumber = dDiNumber,
                                 DestinationNumber = "",
                                 DestinationType = ""
                               };
        _session.SaveOrUpdate(_ddiAstExtension);
      }
    }

    private void SaveOrDeleteVoicemailExtension(IVoiceMail voicemail)
    {
      if (voicemail != null)
      {
        _voicemailExtension.Appdata = "voicemail," + voicemail.Number;
        _session.SaveOrUpdate(_voicemailExtension);
      }
      else
        _session.Delete(_voicemailExtension);
    }

    private void CreateNewVoicemailExtension(IVoiceMail voicemail)
    {
      if (voicemail != null)
      {
        _ddiAstExtension = new AstExtension
                               {
                                 Context = "LocalSets",
                                 Priority = 2,
                                 App = "Macro",
                                 Appdata = "voicemail," + voicemail.Number,
                                 ExtensionNumber = Number,
                                 DestinationNumber = "",
                                 DestinationType = ""
                               };
        _session.SaveOrUpdate(_ddiAstExtension);
      }
    }
  }
}