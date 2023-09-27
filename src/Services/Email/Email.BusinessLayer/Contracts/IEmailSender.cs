using Email.BusinessLayer.Models;

namespace Email.BusinessLayer.Contracts
{
    public interface IEmailSender
    {
        public void Send(EmailBuilder builder, EmailModel model);
    }
}
