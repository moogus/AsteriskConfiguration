using System;
using System.Collections.Generic;
using System.Linq;
using DScriptConverter.DScript;
using DScriptConverter.DScript.Interfaces;
using DatabaseAccess;

namespace DScriptCheck
{
  class Program
  {
    private static List<IDScriptItem> _dataList;
    private static string _callerId;
    private static string _valueToTest;
    private static string _callerType;
    private static IRepository _repository;
    private static bool _isValid;
    #region CONSTANTS

    private const string Extension = "extension";
    private const string Trunk = "trunk";
    private const string AddCode = "addcodeCFT";
    private const string Forward = "forwardCFT";

    #endregion

    static void Main(string[] args)
    {
      _repository = new Repository();
#if DEBUG
      var vals = args[0].Split(',');
      _callerType = vals[1];
      _callerId = vals[2];
      _valueToTest = string.Format(vals[0].Equals("internal") ? "I{0}" : "E{0}", vals[3]);
#else
      _callerType = args[1];
      _callerId = args[2];
      _valueToTest = string.Format(args[0].Equals("internal") ? "I{0}" : "E{0}", args[3]);
#endif

     _dataList= GetDScriptsForUserOnThisDialPlan();

       _isValid = true;

      if (_dataList.Any())
      {
        _isValid = false;

        foreach (var r in _dataList)
        {
          IDScriptActions checker = new DScriptActions(_valueToTest, r);
          if (checker.IsDScriptTypeAllowed() && checker.IsNumberAllowed())
          {
            _isValid = true;
          }
        }
      }
   
      Console.WriteLine("SET VARIABLE CANDIAL {0}", _isValid ? "1" : "0");

      Console.ReadKey();
    }

    private static List<IDScriptItem> GetDScriptsForUserOnThisDialPlan()
    {
      var rtn  = new List<IDScriptItem>();

      var permissions = GetPermissionClassApplicationInternalDialer().
        PermissionClassMemebers.Where(
          pc => pc.Dialplan.Id == _repository.GetList<ICurrentDialPlan>().First().Dialplan.Id).ToList();

      if (!permissions.Any()) return rtn;
      rtn.AddRange(permissions.Select(member => new DScriptItem(member.Pattern.Pattern)));

      return rtn;

    }

    private static IPermisionClass GetPermissionClassApplicationInternalDialer()
    {
      switch (_callerType)
      {
        case Extension:
          return _repository.GetFromName<IExtension>(_callerId).PermisionClass;
        
        case Trunk:
          return _repository.GetFromName<IPermisionClass>("Default");

        case AddCode:
          return _repository.GetFromName<IPermisionClass>("Default");

        case Forward:
          return _repository.GetFromName<IPermisionClass>("Default");
      }
      return null;
    }
  }
}
