using System;
using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class MusicOnHold : IMusicOnHold
  {
    private readonly IComMusicOnHold _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public MusicOnHold(IComMusicOnHold music, IRepositoryWithDelete modelRepository)
    {
      _under = music;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string Name
    {
      get { return _under.ComMusicOnHoldName; }
      set { SetNameAndDirectory(value); }
    }

    public string Directory
    {
      get { return _under.Directory; }
    }

    public string Application
    {
      get { return _under.Application; }
      set { SetApplicationAndMode(value); }
    }

    public MusicOnHoldMode Mode
    {
      get { return (MusicOnHoldMode) Enum.Parse(typeof (MusicOnHoldMode), _under.Mode); }
    }

    public bool Sort
    {
      get { return _under.Sort == "aplha"; }
      set { _under.Sort = value ? "aplha" : ""; }
    }

    public string Format
    {
      get { return _under.Format; }
      set { _under.Format = value; }
    }

    //TODO Digits enable the customer to cycle through the available MOH's
    public char Digit
    {
      get { return _under.Digit; }
      set { _under.Digit = value; }
    }

    public bool Random { get; set; }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }

    private void SetApplicationAndMode(string value)
    {
      _under.Application = value.Equals("play-wav") ? string.Empty : value;
      _under.Mode = value.Equals("play-wav") ? MusicOnHoldMode.files.ToString() : MusicOnHoldMode.custom.ToString();
    }

    private void SetNameAndDirectory(string value)
    {
      _under.ComMusicOnHoldName = value;
      _under.Directory = value.Equals("Default")
                           ? "/var/lib/asterisk/moh{0}"
                           : string.Format("/var/lib/asterisk/moh/{0}", value);
    }
  }
}