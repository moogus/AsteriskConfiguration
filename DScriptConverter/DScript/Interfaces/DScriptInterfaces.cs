using System;

namespace DScriptConverter.DScript.Interfaces
{
  public interface IDScriptItem
  {
    IDScriptType DScriptType { get; }
    string DScriptValue { get; }
  }

  public interface IDScriptType
  {
    string DScriptType { get; }
    Char DScriptTypeValue { get; }
  }

  public interface IDScriptActions
  {
    bool IsDScriptTypeAllowed();
    bool IsNumberAllowed();
  }
}