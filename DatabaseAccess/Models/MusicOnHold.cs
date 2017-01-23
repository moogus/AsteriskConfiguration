using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAccess.DatabaseTables;

namespace DatabaseAccess.Models
{
  internal class MusicOnHold : IMusicOnHold
  {
    private IRepository _repository;
    private readonly ISessionWrapper _session;
    private readonly ComMusicOnHold _under;

    internal MusicOnHold(ComMusicOnHold under, ISessionWrapper session, IRepository repository)
    {
      _under = under;
      _session = session;
      _repository = repository;
    }

    internal MusicOnHold(ISessionWrapper session, IRepository repository)
    {
      _session = session;
      _repository = repository;
      _under = new ComMusicOnHold();

    }

    public int Id { get { return _under.Id; } }
    public string Name
    {
      get
      {
        return _under.Name;
      }
      set
      {
        SetNameAndDirectory(value);
      }
    }

    public string Directory { get { return _under.Directory; } }

    public string Application
    {
      get
      {
        return _under.Application;
      }
      set
      {
        SetApplicationAndMode(value);
      }
    }

    //TODO Digits enable different audio formats more relivant if applications are used through the available MOH's
    public string Format { get { return _under.Format; } set { _under.Format = value; } }

    //TODO Digits enable the customer to cycle through the available MOH's
    public char Digit { get { return _under.Digit; } set { _under.Digit = value; } }

    public MusicOnHoldMode Mode
    {
      get
      {
        return (MusicOnHoldMode)Enum.Parse(typeof(MusicOnHoldMode), _under.Mode);
      }
    }

    public bool Sort
    {
      get
      {
        return _under.Sort == "aplha";
      }
      set
      {
        _under.Sort = value ? "aplha" : "";
      }
    }
    public bool Random
    {
      get
      {
        return _under.Random == "yes";
      }
      set
      {
        _under.Random = value ? "yes" : "no";
      }
    }

    private void SetApplicationAndMode(string value)
    {
      _under.Application = value.Equals("play-wav") ? string.Empty : value;
      _under.Mode = value.Equals("play-wav") ? MusicOnHoldMode.files.ToString() : MusicOnHoldMode.custom.ToString();
    }

    private void SetNameAndDirectory(string value)
    {
      _under.Name =  value;
      _under.Directory = value.Equals("Default") ? "/var/lib/asterisk/moh{0}" : string.Format("/var/lib/asterisk/moh/{0}", value);
    }

    #region IModel Members

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

    #endregion

  }
}
