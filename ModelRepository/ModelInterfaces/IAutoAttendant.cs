using System.Collections.Generic;

namespace ModelRepository.ModelInterfaces
{
  public interface IAutoAttendant : IModel
  {
    int Id { get; }
    string Name { get; set; }
    string Announcement { get; set; }
    int Timeout { get; set; }
    IEnumerable<IAutoAttendantRules> Rules { get; }
  }
}