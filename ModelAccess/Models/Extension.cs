using System;
using DatabaseAccess;

namespace ModelAccess.Models
{
    public interface IExtension : IModel
    {
        string IpAddress { get; }
        string Model { get; }
        int Id { get; }
        string Status { get; }
        string Password { get; set; }
        string Notes { get; set; }
        string Number { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Department { get; set; }
        string Email { get; set; }
        string JobTitle { get; set; }
        string DDINumber { get; set; }
        IVoiceMail VoiceMail { get; set; }
        int VoicemailDelay { get; set; }
    }

    public class Extension : IExtension
    {
        private readonly IUnderlyingExtension _underlyingExtension;


        public Extension(IUnderlyingExtension underlyingExtension)
        {
            _underlyingExtension = underlyingExtension;

            if (_underlyingExtension.Voicemail!=null)
                _voiceMail=new VoiceMail(_underlyingExtension.Voicemail);
            
        }

        #region READONLY FIELDS

        public string IpAddress
        {
            get { return _underlyingExtension.IpAddress; }
        }

        public string Model
        {
            get { return _underlyingExtension.Model; }
        }

        public int Id
        {
            get { return _underlyingExtension.Id; }
        }

        public string Status
        {
            get
            {
                if (_underlyingExtension.StatusTime < 1)
                    return "never used";
                return ConvertFromUnixToDateTime(_underlyingExtension.StatusTime) > DateTime.UtcNow
                           ? "active"
                           : "not active";
            }
        }

        #endregion

        #region IExtension Members

        public string Password
        {
            get { return _underlyingExtension.Password; }
            set { _underlyingExtension.Password = value; }
        }

        public string Notes
        {
            get { return _underlyingExtension.Notes; }
            set { _underlyingExtension.Notes = value; }
        }

        public string Number
        {
            get { return _underlyingExtension.Number; }
            set
            {
                _underlyingExtension.Number = value;
            }
        }

        public string FirstName
        {
            get { return _underlyingExtension.FirstName; }
            set { _underlyingExtension.FirstName = value; }
        }

        public string LastName
        {
            get { return _underlyingExtension.LastName; }
            set { _underlyingExtension.LastName = value; }
        }

        public string Department
        {
            get { return _underlyingExtension.Department; }
            set { _underlyingExtension.Department = value; }
        }

        public string Email
        {
            get { return _underlyingExtension.Email; }
            set { _underlyingExtension.Email = value; }
        }

        public string JobTitle
        {
            get { return _underlyingExtension.JobTitle; }
            set { _underlyingExtension.JobTitle = value; }
        }

        public string DDINumber
        {
            get { return _underlyingExtension.DDINumber; }
            set { _underlyingExtension.DDINumber = value; }
        }

        private IVoiceMail _voiceMail;
        public IVoiceMail VoiceMail
        {
            get { return _voiceMail; }
            set {
              _voiceMail = value;
             
              _underlyingExtension.Voicemail = value==null ? null : value.Under;
            }
        }
 
        public int VoicemailDelay
        {
            get { return _underlyingExtension.VoicemailDelay; }
            set { _underlyingExtension.VoicemailDelay = value; }
        }

        public bool Update()
        {
            _underlyingExtension.Update();
            //TODO make this return depending on success
            return true;
        }

        public bool Delete()
        {
            _underlyingExtension.Delete();


            return true;
        }

        #endregion

        private static DateTime ConvertFromUnixToDateTime(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }

    
}