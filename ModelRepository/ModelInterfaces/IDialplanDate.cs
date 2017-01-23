using System;

namespace ModelRepository.ModelInterfaces
{
  public interface IDialplanDate : IModel
  {
    int Id { get; }
    DateTime StartDate { get; set; }
    DateTime EndDate { get; set; }
    IDialplan Dialplan { get; set; }
  }
}