using System.Linq;
using System.Text.RegularExpressions;

namespace DScriptConverter.DScript
{
  public class ScriptInstruction
  {
    public int StartFromPosition { get; private set; }
    public bool HasRange { get; private set; }
    public int Position { get; private set; }
    public int Length { get; private set; }
    public bool VariableLength { get; private set; }
    public string Value { get; private set; }

    public ScriptInstruction(string dscipt, bool hasRange, int position, int startFromPosition)
    {
      StartFromPosition = startFromPosition;
      HasRange = hasRange;
      Position = position;
      Value = GetValue(dscipt);
      Length = HasRange ? GetLengthRange(dscipt) : GetLengthNoRange(dscipt);
      VariableLength = false;
    }

    private static int GetLengthRange(string dscipt)
    {
      var vals = dscipt.Split('-');
      return vals[0].Trim().Length;
    }

    private int GetLengthNoRange(string dscipt)
    {
      var length = 0;
      if (dscipt.Contains(","))
      {
        var vals = dscipt.Split(',');

        for (var i = 0; i < vals.Count(); i++)
        {
          if (i == 0)
          {
            length = vals[i].Length;
          }
          else
          {
            if (vals[i].Length > length)
            {
              length = vals[i].Length;
              VariableLength = true;
            }
          }
        }
      }
      return length;
    }

    private static string GetValue(string dscipt)
    {
      if (dscipt.Contains(","))
      {
        var vals = dscipt.Split(',');
        dscipt = (vals.Count() == 2 && vals[0] == "") ? vals[1] : Regex.Replace(dscipt, ",", "|");
      }

      dscipt = Regex.Replace(dscipt, "%", ".*");
      dscipt = Regex.Replace(dscipt, "{", ".{");
      return dscipt;
    }
  }
}