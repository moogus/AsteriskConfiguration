using System.Linq;
using DScriptConverter.DScript.Interfaces;

namespace DScriptConverter.DScript
{
  public class DScriptActions : IDScriptActions
  {
    private readonly string _valueToTest;
    private readonly IDScriptItem _dScriptItem;
    private readonly DScriptParse _dScriptParse;
    public DScriptActions(string valueToTest, IDScriptItem dScriptItem)
    {
      _valueToTest = valueToTest;
      _dScriptItem = dScriptItem;
      _dScriptParse = new DScriptParse(_dScriptItem.DScriptValue);
    }
    public bool IsDScriptTypeAllowed()
    {
      return _valueToTest.First() == _dScriptItem.DScriptType.DScriptTypeValue && _dScriptItem.DScriptType.DScriptTypeValue != '#';
    }

    public bool IsNumberAllowed()
    {
     return _dScriptParse.TestScript(_valueToTest.Substring(1));
    }
  }
}