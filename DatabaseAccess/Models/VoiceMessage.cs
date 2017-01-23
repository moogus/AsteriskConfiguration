using System;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class VoiceMessage : IVoiceMessage ,IModel
  {
    private readonly AstVoiceMessage _under;
    private readonly ISessionWrapper _session;
    private readonly IRepository _repository;

    internal VoiceMessage(AstVoiceMessage under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal VoiceMessage(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new AstVoiceMessage();

    }

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
    }

    public void ExtraDelete()
    {
    }

    public int Id { get { return _under.Id; } }
    public string Folder { get { return GetMessageFolder(); } set { _under.Directrory = SetMessageFolder(value); } }
    public string CallerId { get { return GetCallerId(); } }
    public string CallerNumber { get { return GetCallerNumber(); } }
    public DateTime CalledAt { get { return ConvertFromUnixToDateTime(_under.OrigTime); } }
    public int Duration { get { return int.Parse(_under.Duration); } }
    public IVoiceMail MailBox { get { return _repository.GetFromName<IVoiceMail>(_under.MailBox); } }

    public TimeSpan TimeSinceEdited { get { return GetTimeSinceEdited(); } }

    public IAsteriskAudioStream Audiostream
    {
      get
      {
        return new AsteriskAudioStream(_under.Recording);
      }
    }

    private static DateTime ConvertFromUnixToDateTime(string timestamp)
    {
      var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
      if (!string.IsNullOrEmpty(timestamp))
      {
        return origin.AddSeconds(double.Parse(timestamp));
      }
      return origin;
    }

    private string GetMessageFolder()
    {
      return _under.Directrory.Split('/')[_under.Directrory.Split('/').Length - 1];
    }

    private string SetMessageFolder(string value)
    {
      var fileStructure = _under.Directrory.Split('/');
      fileStructure[_under.Directrory.Split('/').Length - 1] = value;

      return fileStructure.Aggregate("", (current, s) => string.IsNullOrEmpty(current) ? s != ""
                                                      ? string.Format("/{0}", s) : current : s != ""
                                                      ? string.Format("{0}/{1}", current, s) : current);

    }

    private string GetCallerId()
    {
      var callNumber = GetCallerNumber();

      return IsSipExtension() ? GetSipCallerName(callNumber) : callNumber;
    }

    private bool IsSipExtension()
    {
      return IsSipExtensionGetCallerNumber().Item2;
    }

    private string GetCallerNumber()
    {
      return IsSipExtensionGetCallerNumber().Item1;
    }

    private Tuple<string, bool> IsSipExtensionGetCallerNumber()
    {
      var rtn = (_under.CallerId.Contains('<')) ? new Tuple<string, bool>(_under.CallerId.Split('<')[1].Split('>')[0], true)
                  : _under.CallerId.Length > 5 ? new Tuple<string, bool>(string.Format("0{0}", _under.CallerId), false)
                      : new Tuple<string, bool>(_under.CallerId, false);

      return string.IsNullOrEmpty(_under.CallerId) ? new Tuple<string, bool>("", false) : rtn;
    }

    private string GetSipCallerName(string callNumber)
    {
      var rtn = "No caller ID";

      foreach (var e in _repository.GetList<IExtension>().Where(e => e.Number.Equals(callNumber)))
      {
        rtn = string.Format("{0} {1}", e.FirstName, e.LastName);
      }

      foreach (var k in _repository.GetList<IKnownNumber>().Where(k=>k.Number.Equals(callNumber)))
      {
        rtn = string.Format("{0}", k.Description);
      }

      return rtn;
    }

    private TimeSpan GetTimeSinceEdited()
    {
      return new TimeSpan(DateTime.Now.Ticks - _under.TimeStamp.Ticks);
    }

  }
}
