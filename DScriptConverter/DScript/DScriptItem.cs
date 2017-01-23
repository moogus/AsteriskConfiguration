using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DScriptConverter.DScript.Interfaces;

namespace DScriptConverter.DScript
{
  public class DScriptItem : IDScriptItem
  {
    private readonly string _dScript;
    public IDScriptType DScriptType { get; private set; }
    public string DScriptValue { get; private set; }
    
    public DScriptItem(string dScript)
    {
      _dScript = dScript;
      DScriptType = GetDScriptType();
      DScriptValue = dScript.Substring(1, dScript.Length - 1);
    }

    private IDScriptType GetDScriptType()
    {
      IDScriptType idScriptType;
      switch (_dScript.First())
      {
        default:
          idScriptType = new DScriptTypeInValid();
          break;
        case 'E':
          idScriptType = new DScriptTypeExternal();
          break;
        case 'I':
          idScriptType = new DScriptTypeInternal();
          break;

      }
      return idScriptType;
    }


    private class DScriptTypeInValid : IDScriptType
    {
      public string DScriptType { get; private set; }
      public char DScriptTypeValue { get; private set; }

      public DScriptTypeInValid()
      {
        DScriptType = "Invalid";
        DScriptTypeValue = '#';
      }
    }

    private class DScriptTypeExternal : IDScriptType
    {
      public string DScriptType { get; private set; }
      public char DScriptTypeValue { get; private set; }

      public DScriptTypeExternal()
      {
        DScriptType = "External";
        DScriptTypeValue = 'E';
      }
    }
    private class DScriptTypeInternal : IDScriptType
    {
      public string DScriptType { get; private set; }
      public char DScriptTypeValue { get; private set; }

      public DScriptTypeInternal()
      {
        DScriptType = "Internal";
        DScriptTypeValue = 'I';
      }
    }
  }
}