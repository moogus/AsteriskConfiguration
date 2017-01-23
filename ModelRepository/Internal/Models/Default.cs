using DataAccess.TableInterfaces;
using ModelRepository.ModelInterfaces;

namespace ModelRepository.Internal.Models
{
  internal class Default : IDefault
  {
    private readonly IFuDefaults _under;
    private readonly IRepositoryWithDelete _modelRepository;

    public Default(IFuDefaults fuDefaults, IRepositoryWithDelete modelRepository)
    {
      _under = fuDefaults;
      _modelRepository = modelRepository;
    }

    public int Id
    {
      get { return _under.Id; }
    }

    public string Type
    {
      get { return _under.Type; }
      set { _under.Type = value; }
    }

    public int Index
    {
      get { return _under.ColumnIndex; }
      set { _under.ColumnIndex = value; }
    }

    //used by the modelRepository for get by name
    public string ColumnTitle
    {
      get { return _under.ColumnTitle; }
      set { _under.ColumnTitle = value; }
    }

    public string JavascriptColumnType
    {
      get { return _under.ColumnType; }
      set { _under.ColumnType = value; }
    }

    public string DefaultValue
    {
      get { return _under.Default; }
      set { _under.Default = value; }
    }

    public string Picker
    {
      get { return _under.Picker; }
      set { _under.Picker = value; }
    }

    public string JavascriptProperty
    {
      get { return _under.Property; }
      set { _under.Property = value; }
    }

    public void Delete()
    {
      _modelRepository.Delete(_under);
    }
  }
}