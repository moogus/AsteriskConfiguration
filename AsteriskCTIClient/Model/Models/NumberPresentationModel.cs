using System;
using System.Linq;
using AsteriskCTIClient.Model.ModelInterfaces;

namespace AsteriskCTIClient.Model.Models
{
  public class NumberPresentationModel :INumber 
  {
    private readonly IAccessListModel _acessListModel;
    private string _number;
    public string OriginalNumber { get; private set; }
    public string Number
    {
      get { return _number; }
      set
      {
        _number = IsValidNumber(value);
      }
    }

    public NumberPresentationModel(IAccessListModel acessListModel)
    {
      _acessListModel = acessListModel;
    }

    private string IsValidNumber(string number)
    {
      OriginalNumber = number;

      var cleanNumber = RemoveInvalidCharacters(number);

      if (IsPosibleExternalNumber(cleanNumber))
      {
        foreach (var ac in _acessListModel.AccessCodes.Where(cleanNumber.StartsWith))
        {
          cleanNumber = cleanNumber.Substring(ac.Length);
        }
      }
      return cleanNumber;
    }

    //todo: this is repeated in ValidPhoneNumberModel
    private static string RemoveInvalidCharacters(string number)
    {
      return number.Where(Char.IsNumber).Aggregate(string.Empty, (current, n) => string.Format("{0}{1}", current, n));
    }

    private static bool IsPosibleExternalNumber(string numberToDial)
    {
      return numberToDial.All(n => Char.IsNumber(n) || Char.IsWhiteSpace(n)) && numberToDial.Count() > 8;
    }
  }
}
