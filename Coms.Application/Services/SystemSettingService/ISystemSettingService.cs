using ErrorOr;

namespace Coms.Application.Services.SystemSettingService
{
    public interface ISystemSettingService
    {
        Task<ErrorOr<SystemSettingResult>> GetSystemSettings();
        Task<ErrorOr<SystemSettingResult>> EditSystemSettings(string companyName, string address, string phone, string hotline,
                string taxCode, string email, string bankAccount, string bankAccountNumber, string bankName, string appPassword);
    }
}
