using System.Collections.Generic;
using System.Linq;
using DatabaseAccess;

namespace ModelAccess.Models
{
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

  public class Default : IDefault
  {
    private readonly IUnderlyingDefaults _underlyingDefaults;

    public Default(IUnderlyingDefaults underlyingDefaults)
    {
      _underlyingDefaults = underlyingDefaults;
    }

    public bool Update()
    {
      return _underlyingDefaults.Update();
    }

    public bool Delete()
    {
      _underlyingDefaults.Delete();
      return true;
    }

    public int Id
    {
      get { return _underlyingDefaults.Id; }
      set { _underlyingDefaults.Id = value; }
    }

    public string Type
    {
      get { return _underlyingDefaults.Type; }
      set { _underlyingDefaults.Type = value; }
    }

    public int Index
    {
      get { return _underlyingDefaults.Index; }
      set { _underlyingDefaults.Index = value; }
    }

    public string JavascriptColumnType
    {
      get { return _underlyingDefaults.JavascriptColumnType; }
      set { _underlyingDefaults.JavascriptColumnType = value; }
    }

    public string ColumnTitle
    {
      get { return _underlyingDefaults.ColumnTitle; }
      set { _underlyingDefaults.ColumnTitle = value; }
    }

    public string JavascriptProperty
    {
      get { return _underlyingDefaults.JavascriptProperty; }
      set { _underlyingDefaults.JavascriptProperty = value; }
    }

    public string DefaultValue
    {
      get { return _underlyingDefaults.DefaultValue; }
      set { _underlyingDefaults.DefaultValue = value; }
    }

    public string Picker
    {
      get { return _underlyingDefaults.Picker; }
      set { _underlyingDefaults.Picker = value; }
    }
  }

}