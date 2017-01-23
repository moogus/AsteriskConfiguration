using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAccess;

namespace MessageCleanUp
{
  class Program
  {
    static void Main(string[] args)
    {
      var repository = new Repository();

      var timeToDie =
        repository.GetList<IDefault>().First(d => d.Type == "VoiceMessages" && d.ColumnTitle == "Recycle Time in Days").
          DefaultValue;

      var binnable = repository.GetList<IVoiceMessage>().Where(m => m.Folder == "Deleted" && m.TimeSinceEdited.Days > int.Parse(timeToDie));

      //foreach (var m in binnable)
      //{
      //  Console.WriteLine(m.TimeSinceEdited.TotalMinutes);
      //}
      //Console.ReadKey();

      foreach (var voiceMessage in binnable)
      {
        voiceMessage.Delete();
      }
    }
  }
}
