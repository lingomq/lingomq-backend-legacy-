using EventBus.Entities.Email;

namespace Email.BusinessLayer.Contracts
{
    public interface IEmailSender
    {
        public void Send(EmailBuilder builder, EmailModelSignUp model);
    }
}
