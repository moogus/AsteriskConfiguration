using System;
using DatabaseAccess;

namespace ModelAccess.Models
{
    public interface IDialplanDate : IModel
    {
        int Id { get; }
        DateTime Date { get; set; }
        bool IsRange { get; set; }
        int DateRangeId { get; set; }
        string Dialplan { get; set; }
    }

    public class DialplanDate : IDialplanDate
    {
        private readonly IUnderlyingDialplanDate _underlyingDialplanDate;
        private readonly DatabaseAccessRepository<IUnderlyingDialplan> _dialplanRepository; 

        public DialplanDate(IUnderlyingDialplanDate underlyingDialplanDate)
        {
          _dialplanRepository=new DatabaseAccessRepository<IUnderlyingDialplan>();
            _underlyingDialplanDate = underlyingDialplanDate;
        }

        #region IDialplanDate Members

        public bool Update()
        {
            return _underlyingDialplanDate.Update();
        }

        public bool Delete()
        {
            return _underlyingDialplanDate.Delete();
        }

        public int Id
        {
            get { return _underlyingDialplanDate.Id; }
        }

        public DateTime Date
        {
            get { return _underlyingDialplanDate.Date; }
            set { _underlyingDialplanDate.Date = value; }
        }

      public bool IsRange
      {
        get { return _underlyingDialplanDate.IsRange; }
        set { _underlyingDialplanDate.IsRange = value; }
      }

      public int DateRangeId
      {
        get { return _underlyingDialplanDate.DateRangeId; }
        set { _underlyingDialplanDate.DateRangeId = value; }
      }

      public string Dialplan
        {
            get { return _underlyingDialplanDate.Dialplan.Name; }

            set { _underlyingDialplanDate.Dialplan = _dialplanRepository.GetFromName(value); }
        }

        #endregion
    }

   
}
