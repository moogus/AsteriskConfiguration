using DatabaseAccess;

namespace ModelAccess.Models
{
    public interface IDDI : IModel
    {
        int Id { get; }
        string DDINumber { get; set; }
        string Used { get; }
    }


    public class DDI : IDDI
    {
        private readonly IUnderlyingDDI _underlyingDDI;
        public DDI(IUnderlyingDDI underlyingDDI)
        {
            _underlyingDDI = underlyingDDI;
            if (_underlyingDDI.Id > 0)
            {
                SetDefaultInformation();
            }
        }
        
        public bool Update()
        {
            return _underlyingDDI.Update();
        }

        public bool Delete()
        {
            _underlyingDDI.Delete();
            return true;
        }
        
        private void SetDefaultInformation()
        {
            
        }

        public int Id
        {
            get { return _underlyingDDI.Id; }
        }

        public string DDINumber
        {
            get { return _underlyingDDI.DDI; }
            set { _underlyingDDI.DDI = value; }
        }

        public string Used
        {
            get { return _underlyingDDI.Used; }
        }
    }

    
}