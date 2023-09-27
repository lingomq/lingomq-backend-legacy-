using Email.BusinessLayer.Contracts;

namespace Email.BusinessLayer.Services
{
    public class SignInEmailBuilder : EmailBuilder
    {
        public override void SetHeader() =>
            EmailTemplate!.Header = "Вход в аккаунт";

        public override void SetText() =>
            EmailTemplate!.Text = $"В ваш аккаунт был выполнен вход." +
            $"Если это были не вы, рекомендуем защитить свой аккаунт";
    }
}
