using System.Collections.Generic;

namespace AsteriskCTIClient.Model.ModelInterfaces
{
  public interface IContactListModel
  {
    List<IContactModel> ContactModels { get; set; }
    bool IsKnownNumber(string numberToDial);
  }
}