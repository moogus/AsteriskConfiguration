using DatabaseAccess;

namespace ModelAccess.Models
{
    public interface IDialplanRange : IModel
    {
        int Id { get; }
        string DaysOfWeek { get; set; }
        string TimeRange { get; set; }
        int Priority { get; set; }
        string Dialplan { get; set; }
    }

    public class DialplanRange : IDialplanRange
    {
        private readonly IUnderlyingDialplanRange _underlyingDialplanRange;
        private readonly DatabaseAccessRepository<IUnderlyingDialplan> _dialplanRepository; 

        public DialplanRange(IUnderlyingDialplanRange underlyingDialplanRange)
        {
            _dialplanRepository = new DatabaseAccessRepository<IUnderlyingDialplan>();
            _underlyingDialplanRange = underlyingDialplanRange;
        }

        public bool Update()
        {
            return _underlyingDialplanRange.Update();
        }

        public bool Delete()
        {
            return _underlyingDialplanRange.Delete();
        }

        public int Id
        {
            get { return _underlyingDialplanRange.Id; }
        }

        public string DaysOfWeek
        {
            get { return _underlyingDialplanRange.DaysOfWeek; }
            set { _underlyingDialplanRange.DaysOfWeek = value; }
        }

        public string TimeRange
        {
            get { return _underlyingDialplanRange.TimeRange; }
            set { _underlyingDialplanRange.TimeRange = value; }
        }

        public int Priority
        {
            get { return _underlyingDialplanRange.Priority; }
            set { _underlyingDialplanRange.Priority = value; }
        }

        public string Dialplan
        {
            get { return _underlyingDialplanRange.Dialplan.Name; }
            set { _underlyingDialplanRange.Dialplan = _dialplanRepository.GetFromName(value); }
        }
    }

    
}