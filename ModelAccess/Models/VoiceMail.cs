using DatabaseAccess;

namespace ModelAccess.Models
{
    public interface IVoiceMail : IModel
    {
        int Id { get; set; }
        string Number { get; set; }
        string Password { get; set; }
        int NumberOfMessages { get; set; }
        int MessageLength { get; set; }
        int HeldNumberOfMessages { get; set; }
        string DefaultEmail { get; set; }
        bool EmailNotificationHasMp3 { get; set; }
        IUnderlyingVoicemail Under { get; }
    }

    public class VoiceMail : IVoiceMail
    {
        public IUnderlyingVoicemail Under { get; private set; }

        public VoiceMail(IUnderlyingVoicemail under)
        {
            Under = under;
        }

        public bool Update()
        {
            return Under.Update();
        }

        public bool Delete()
        {
            return Under.Delete();
        }

        public int Id { get { return Under.Id; } set { Under.Id = value; } }

        public string Number { get { return Under.Mailbox; } set { Under.Mailbox = value; } }

        public string Password { get { return Under.Password; } set { Under.Password = value; } }


        public int NumberOfMessages { get { return Under.MaxMessages; } set { Under.MaxMessages = value; } }

        public int HeldNumberOfMessages { get { return Under.HeldMaxMessages; } set { Under.HeldMaxMessages = value; } }

        public int MessageLength { get { return Under.MaxLength; } set { Under.MaxLength = value; } }


        public string DefaultEmail { get { return Under.Email; } set { Under.Email = value; } }

        public bool EmailNotificationHasMp3 { get { return Under.Attach; } set { Under.Attach = value; } }
    }
}