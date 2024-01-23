using EventBus.Entities.Email;

namespace Email.Application.Contracts
{
    public interface IEmailSender
    {
        public void Send(EmailBuilder builder, EmailModelSignUp model);
    }
}
