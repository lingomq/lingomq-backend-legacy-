using Email.Application.Models;

namespace Email.Application.Contracts
{
    public abstract class EmailBuilder
    {
        public EmailTemplate? EmailTemplate { get; private set; }
        public void CreateMessage() => EmailTemplate = new EmailTemplate();
        public abstract void SetHeader();
        public abstract void SetText();
        public virtual void SetButton() { }
    }
}
