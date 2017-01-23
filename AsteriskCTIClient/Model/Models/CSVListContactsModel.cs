using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using AsteriskCTIClient.Model.ModelInterfaces;

namespace AsteriskCTIClient.Model.Models
{
  public class CSVListContactsModel : IContactListModel
  {
    public List<IContactModel> ContactModels { get; set; }

    public CSVListContactsModel()
    {
      CreateContacts();
    }

    public bool IsKnownNumber(string numberToDial)
    {
      return
        ContactModels.Select(c => c.Extension)
                            .Concat(ContactModels.Select(c => c.Mobile))
                            .Concat(
                              ContactModels.Select(c => c.DDI)).Any(number => number == numberToDial);
    }

    private void CreateContacts()
    {
      string[] contactCsv = File.ReadAllLines(ConfigurationManager.AppSettings.Get("CsvLocation"));
      IEnumerable<IContactModel> allContacts =
        from line in contactCsv
        let contact = line.Split(',')
        select new ContactModel
          {
            UserName = string.Format("{0} {1}", contact[0], contact[1]),
            FirstName = contact[0],
            LastName = contact[1],
            Department = contact[2],
            Position = contact[3],
            Email = contact[4],
            Extension = contact[5],
            DDI = contact[6],
            Mobile = contact[7],
            IsAsteriskContact = Convert.ToBoolean(contact[8]),
          };

      ContactModels = allContacts.ToList();
    }
  }
}
