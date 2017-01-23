using System;
using System.Text.RegularExpressions;

namespace DScriptConverter.DScript
{
  public class DScriptParse
  {
    private readonly Tuple<string, Match> _expression;

    public DScriptParse(string expression)
    {
      _expression = CompileExpression(expression);
    }

    public bool TestScript(string input)
    {
      var firstTest = Regex.Match(input, _expression.Item1);
      var secondTest = false;
      var ranges = _expression.Item2;
      if (firstTest.Success)
      {
        secondTest = true;
        var rangeIndex = 1;
        while (ranges.Success)
        {
          var range = ranges.Groups[0].ToString().Replace("(", "").Replace(")", "");
          var from = int.Parse(range.Substring(0, range.IndexOf('-')));
          var to = int.Parse(range.Substring(range.IndexOf('-') + 1, range.Length - range.IndexOf('-') - 1));
          var valToTest = int.Parse(firstTest.Groups[rangeIndex].ToString());
          if (valToTest < @from | valToTest > to)
          {
            secondTest = false;
          }
          rangeIndex++;
          ranges = ranges.NextMatch();
        }
      }
      return secondTest;
    }

    private Tuple<string, Match> CompileExpression(string exp)
    {
      var ranges = Regex.Match(exp, @"\(\d+-\d+\)");
      exp = Regex.Replace(exp, @"(\d)+-\d+", match =>
      {
        string v = match.ToString();
        var len = v.IndexOf('-');
        return new string('.', len);
      });

      exp = Regex.Replace(exp, @"\(([^)]+,[^)]+)\)", @"(?:$1)");
      exp = Regex.Replace(exp, @",", "|");
      exp = Regex.Replace(exp, @"%", ".*");
      exp = Regex.Replace(exp, @"_", ".");
      exp = Regex.Replace(exp, @"{", ".{");
      return Tuple.Create(exp, ranges);
    }
  }
}
