using System;
using System.Linq;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class Extension : IExtension, IModel
  {
    private readonly IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly ComExtension _underExt;
    private readonly AstSipReg _underAstSipReg;

    internal Extension(ComExtension underExt, ISessionWrapper session, IRepository repository)
    {
      _underExt = underExt;
      _session = session;

      _underAstSipReg = _session.Query<AstSipReg>().ToList().FirstOrDefault(s => s.Number == _underExt.Number) ??
                        new AstSipReg { Number = _underExt.Number };

      _repository = repository;
    }

    internal Extension(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _underExt = new ComExtension();
      _underAstSipReg = new AstSipReg();
    }

    public int Id { get { return _underExt.Id; } }
    public string IpAddress { get { return _underAstSipReg.IpAddress; } set { _underAstSipReg.IpAddress = value; } }
    public string Model { get { return _underAstSipReg.Model; } }
    public string Status { get { return GetState(); } }
    public string Password { get { return _underExt.Password; } set { _underExt.Password = value; } }
    public string Notes { get { return _underExt.Notes; } set { _underExt.Notes = value; } }

    public string Number
    {
      get { return _underExt.Number; }
      set { _underAstSipReg.Number = value; _underExt.Number = value; }
    }

    public string FirstName { get { return _underExt.FirstName; } set { _underExt.FirstName = value; } }
    public string LastName { get { return _underExt.LastName; } set { _underExt.LastName = value; } }
    public string Department { get { return _underExt.Department; } set { _underExt.Department = value; } }
    public string Email { get { return _underExt.Email; } set { _underExt.Email = value; } }
    public string JobTitle { get { return _underExt.JobTitle; } set { _underExt.JobTitle = value; } }

    public IDDI DDI
    {
      get
      {
        return !string.IsNullOrEmpty(_underExt.DDINumber) ? _repository.GetFromName<IDDI>(_underExt.DDINumber) : null;
      }
      set
      {
        if (value == null)
        {
          //TODO: this needs to be fixed!!!
          //var ddi = _repository.GetFromName<IDDI>(_underExt.DDINumber);
          //ddi.UsedOn = _repository.GetList<IRoutingRule>().
          //                          FirstOrDefault(
          //                                         r => r.Dialplan.Id == 12 && r.Number == DDI.DDINumber
          //                                         ) != null ? DDIUsedOn.Default : DDIUsedOn.NotUsed;

          //ddi.Update();
          //_underExt.DDINumber = string.Empty;
          return;
        }
       
        value.UsedOn = DDIUsedOn.Extension;
        value.Update();
        _underExt.DDINumber = value.DDINumber;
      }
    }

    public ICLI CLI
    {
      get
      {
        return _underExt.CLIId != 0 ? _repository.GetFromId<ICLI>(_underExt.CLIId) : _repository.Add<ICLI>();
      }
      set { _underExt.CLIId = value != null ? _repository.GetFromName<ICLI>(value.CLINumber).Id : 0; }
    }

    public bool DND { get { return _underExt.DND; } set { _underExt.DND = value; } }
    public int VoicemailDelay { get { return _underExt.VoiceMailDelay; } set { _underExt.VoiceMailDelay = value; } }
    public IPermisionClass PermisionClass { get { return _repository.GetFromId<IPermisionClass>(_underExt.PermissionClassId); } set { _underExt.PermissionClassId = value.Id; } }
    public bool IncludeInDirectory { get { return _underExt.IncludeInDirectory; } set { _underExt.IncludeInDirectory = value; } }

    public IVoiceMail VoiceMail
    {
      get { return GetVoiceMail(); }

      set { SetVoiceMail(value); }
    }

    private IVoiceMail GetVoiceMail()
    {
      return _repository.GetFromId<IVoiceMail>(_underExt.VoiceMailId);
    }

    private void SetVoiceMail(IVoiceMail value)
    {
      _underExt.VoiceMailId = value == null ? 0 : value.Id;
    }

    object IModel.Under
    {
      get { return _underExt; }
    }

    ISessionWrapper IModel.Session
    {
      get { return _session; }
    }

    public void ExtraUpdate()
    {
      _session.SaveOrUpdate(_underAstSipReg);
    }

    public void ExtraDelete()
    {
      _session.Delete(_underAstSipReg);

      //todo: remove dependancy...with NHibernate a another repository is need to do this (second session)
      if (DDI == null) return;

      DDI.UsedOn = DDIUsedOn.NotUsed;
      DDI.Update();
    }

    private string GetState()
    {
      if (_underAstSipReg.StatusTime > 1)
      {
        return ConvertFromUnixToDateTime(_underAstSipReg.StatusTime) > DateTime.UtcNow
                   ? "active"
                   : "not active";
      }
      return "never used";
    }

    private static DateTime ConvertFromUnixToDateTime(double timestamp)
    {
      var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
      return origin.AddSeconds(timestamp);
    }

  }
}