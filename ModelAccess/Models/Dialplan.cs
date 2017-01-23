using DatabaseAccess;

namespace ModelAccess.Models
{
    public interface IDialplan : IModel
    {
        int Id { get; }
        string Name { get; set; }
        IUnderlyingDialplan UnderlyingDialplan { get; }
    }

    public class Dialplan : IDialplan
    {
        public IUnderlyingDialplan UnderlyingDialplan { get; private set; }

        public Dialplan(IUnderlyingDialplan underlyingDialplan)
        {
            UnderlyingDialplan = underlyingDialplan;
        }

        #region IDialplan Members

        public bool Update()
        {
            return UnderlyingDialplan.Update();
        }

        public bool Delete()
        {
            return UnderlyingDialplan.Delete();
        }

        public int Id
        {
            get { return UnderlyingDialplan.Id; }
        }

        public string Name
        {
            get { return UnderlyingDialplan.Name; }
            set { UnderlyingDialplan.Name = value; }
        }

        #endregion
    }

   
}