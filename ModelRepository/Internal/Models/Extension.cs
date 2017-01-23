using System;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class Extension : IExtension
  {
    private readonly IAstSipReg _underAstSipReg;
    private readonly IRepositoryWithDelete _modelRepository;
    private readonly IComExtension _underExt;

    public Extension(IComExtension comExtension, IAstSipReg astSipReg, IRepositoryWithDelete modelRepository)
    {
      _underExt = comExtension;
      _underAstSipReg = astSipReg;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _underExt.Id; }
    }

    public string IpAddress
    {
      get { return _underAstSipReg.IpAddress; }
      set { _underAstSipReg.IpAddress = value; }
    }

    public string Model
    {
      get { return _underAstSipReg.Model; }
    }

    public string Status
    {
      get { return GetState(); }
    }

    public string Password
    {
      get { return _underExt.Password; }
      set { _underExt.Password = value; }
    }

    public string Notes
    {
      get { return _underExt.Notes; }
      set { _underExt.Notes = value; }
    }

    //used by the modelRepository for get by name
    public string Number
    {
      get { return _underExt.Number; }
      set
      {
        _underAstSipReg.Number = value;
        _underExt.Number = value;
      }
    }

    public string FirstName
    {
      get { return _underExt.FirstName; }
      set { _underExt.FirstName = value; }
    }

    public string LastName
    {
      get { return _underExt.LastName; }
      set { _underExt.LastName = value; }
    }

    public string Department
    {
      get { return _underExt.Department; }
      set { _underExt.Department = value; }
    }

    public string Email
    {
      get { return _underExt.Email; }
      set { _underExt.Email = value; }
    }

    public string JobTitle
    {
      get { return _underExt.JobTitle; }
      set { _underExt.JobTitle = value; }
    }

    public IDDI DDI
    {
      get { return !string.IsNullOrEmpty(_underExt.DDINumber) ? _modelRepository.GetFromName<IDDI>(_underExt.DDINumber) : null; }
      set
      {
        if (value == null)
        {
          _underExt.DDINumber = string.Empty;
          return;
        }
        _underExt.DDINumber = value.DDINumber;
      }
    }

    public ICLI CLI
    {
      get { return _underExt.CLIId != 0 ? _modelRepository.GetFromId<ICLI>(_underExt.CLIId) : null; }
      set { _underExt.CLIId = value == null ? 0 : _modelRepository.GetFromName<ICLI>(value.CLINumber).Id; }
    }

    public bool DND
    {
      get { return _underExt.DND; }
      set { _underExt.DND = value; }
    }


    public IVoiceMail VoiceMail
    {
      get { return _modelRepository.GetFromId<IVoiceMail>(_underExt.VoiceMailId); }
      set { _underExt.VoiceMailId = value == null ? 0 : value.Id; }
    }

    public int VoicemailDelay
    {
      get { return _underExt.VoiceMailDelay; }
      set { _underExt.VoiceMailDelay = value; }
    }

    public IPermisionClass PermisionClass
    {
      get { return _modelRepository.GetFromId<IPermisionClass>(_underExt.PermissionClassId); }
      set { _underExt.PermissionClassId = value.Id; }
    }

    public bool IncludeInDirectory
    {
      get { return _underExt.IncludeInDirectory; }
      set { _underExt.IncludeInDirectory = value; }
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

    public void Delete()
    {
     _modelRepository.Delete(_underAstSipReg);
      _modelRepository.Delete(_underExt);
    }
  }


}