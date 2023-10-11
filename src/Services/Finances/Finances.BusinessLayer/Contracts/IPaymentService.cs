namespace Finances.BusinessLayer.Contracts
{
    public interface IPaymentService
    {
        Task<bool> ConfirmPayment();
    }
}
