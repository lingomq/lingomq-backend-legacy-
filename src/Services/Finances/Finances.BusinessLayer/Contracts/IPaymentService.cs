using Finances.BusinessLayer.Models.YooKassa;

namespace Finances.BusinessLayer.Contracts
{
    public interface IPaymentService
    {
        Task<bool> ConfirmPayment(Guid paymentId);
        Task<bool> ConfirmPaymentData(Guid userId, Guid financeId, YooKassaSuccessResponse response);
    }
}
