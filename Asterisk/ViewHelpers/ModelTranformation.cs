using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModelRepository.ModelInterfaces;

namespace Asterisk.ViewHelpers
{
  public class ModelTranformation
  {
    public static HtmlString GenerateColumns(IEnumerable<IDefault> defaults)
    {
      return new HtmlString(string.Join(",", defaults.Select((t) =>
        {
          var args = new List<string>();
          if (!string.IsNullOrEmpty(t.JavascriptProperty))
          {
            args.Add(t.JavascriptProperty);
          }
          args.Add('"' + t.DefaultValue + '"');
          return t.JavascriptColumnType + "(" + string.Join(",", args) + ")";
        })));
    }

    public static List<HtmlString> CreatePickers(IEnumerable<IDefault> defaults)
    {
      int pickernameindex = 1;
      var pickerList = new List<HtmlString>();
      foreach (IDefault col in defaults.Where(t => !string.IsNullOrEmpty(t.Picker)))
      {
        var pickeroutput = new HtmlString(string.Format("var picker{0} = {1}();", pickernameindex, col.Picker));
        col.JavascriptProperty = "picker" + pickernameindex++;
        pickerList.Add(pickeroutput);
      }

      return pickerList;
    }
  }
}