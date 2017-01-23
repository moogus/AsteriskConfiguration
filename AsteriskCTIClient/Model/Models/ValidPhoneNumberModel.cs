using System;
using System.Linq;
using AsteriskCTIClient.Model.ModelInterfaces;

namespace AsteriskCTIClient.Model.Models
{
  public class ValidPhoneNumberModel : IValidNumber
  {
    private readonly IAccessListModel _accessListModel;
    private readonly IContactListModel _csvContactListModel;

    public ValidPhoneNumberModel(IAccessListModel accessListModel, IContactListModel csvContactListModel)
    {
      _accessListModel = accessListModel;
      _csvContactListModel = csvContactListModel;
    }

    public string OriginalNumber { get; private set; }
    public string Number { get;  set; }

    public bool IsValidNumber(string number)
    {
      var rtn = false;
      OriginalNumber = number;
      var cleanNumber = RemoveInvalidCharacters(number);

      if (IsPosibleExternalNumber(RemoveInvalidCharacters(cleanNumber)))
      {
        if (_accessListModel.AccessCodes.Count > 0)
        {
          Number = string.Format("{0}{1}", _accessListModel.AccessCodes.First(), cleanNumber);
          rtn = true;
        }
      }
      if (_csvContactListModel.IsKnownNumber(cleanNumber))
      {
        Number = cleanNumber;
        rtn = true;
      }

      if (IsVoiceMail(cleanNumber))
      {
        Number = cleanNumber;
        rtn = true;
      }

      return rtn;
    }

    private static string RemoveInvalidCharacters(string number)
    {
      if (number.StartsWith("+"))
      {
        number = string.Format("00{0}", number.Substring(1));
      }

      return number.Where(Char.IsNumber).Aggregate(string.Empty, (current, n) => string.Format("{0}{1}", current, n));
    }

    private static bool IsPosibleExternalNumber(string numberToDial)
    {
      return numberToDial.All(n => Char.IsNumber(n) || Char.IsWhiteSpace(n)) && numberToDial.Count() > 8;
    }

    private bool IsVoiceMail(string searchString)
    {
      return searchString == "555";
    }


  }
}