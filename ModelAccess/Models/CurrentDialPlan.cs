using DatabaseAccess;

namespace ModelAccess.Models
{
    public class CurrentDialPlan
    {
        private IDialplan _dialplan;
        public IDialplan Dialplan
        {
            get { return _dialplan; }
            set
            {
                _dialplan = value;
                _currentDialplan.Dialplan = value.UnderlyingDialplan;
            }
        }

        public bool AutomaticallyChange { get { return _currentDialplan.AutomaticallyChange; } set { _currentDialplan.AutomaticallyChange = value; } }


        private readonly ICurrentDialplan _currentDialplan;

        public CurrentDialPlan()
        {
            _currentDialplan = new FuCurrentDialplanLinked();
            _dialplan = new Dialplan(_currentDialplan.Dialplan);
        }

        public bool Update()
        {
            return _currentDialplan.Update();
        }
    }
}