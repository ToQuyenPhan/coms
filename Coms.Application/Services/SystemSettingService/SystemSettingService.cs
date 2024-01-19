using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Entities;
using ErrorOr;
using Microsoft.Office.Interop.Word;

namespace Coms.Application.Services.SystemSettingService
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly ISystemSettingsRepository _systemSettingsRepository;

        public SystemSettingService(ISystemSettingsRepository systemSettingsRepository)
        {
            _systemSettingsRepository = systemSettingsRepository;
        }

        public async Task<ErrorOr<SystemSettingResult>> GetSystemSettings()
        {
            var systemSettings = await _systemSettingsRepository.GetSystemSettings();
            if (systemSettings is not null)
            {
                var result = new SystemSettingResult()
                {
                    Id = systemSettings.Id,
                    CompanyName = systemSettings.CompanyName,
                    Address = systemSettings.Address,
                    Phone = systemSettings.Phone,
                    Hotline = systemSettings.Hotline,
                    TaxCode = systemSettings.TaxCode,
                    Email = systemSettings.Email,
                    BankAccount = systemSettings.BankAccount,
                    BankAccountNumber = systemSettings.BankAccountNumber,
                    BankName = systemSettings.BankName,
                    AppPassword = systemSettings.AppPassword
                };
                return result;
            }
            else
            {
                return Error.Failure("500", "Not found any system settings");
            }
        }

        public async Task<ErrorOr<SystemSettingResult>> EditSystemSettings(string companyName, string address, string phone, string hotline, 
                string taxCode, string email, string bankAccount, string bankAccountNumber, string bankName, string appPassword)
        {
            try
            {
                var systemSettings = await _systemSettingsRepository.GetSystemSettings();
                if (systemSettings is not null)
                {
                    systemSettings.CompanyName = companyName;
                    systemSettings.Address = address;
                    systemSettings.Phone = phone;
                    systemSettings.Hotline = hotline;
                    systemSettings.TaxCode = taxCode;
                    systemSettings.Email = email;
                    systemSettings.BankAccount = bankAccount;
                    systemSettings.BankAccountNumber = bankAccountNumber;
                    systemSettings.BankName = bankName;
                    systemSettings.AppPassword = appPassword.Trim();
                    await _systemSettingsRepository.Update(systemSettings);
                    var result = new SystemSettingResult()
                    {
                        Id = systemSettings.Id,
                        CompanyName = systemSettings.CompanyName,
                        Address = systemSettings.Address,
                        Phone = systemSettings.Phone,
                        Hotline = systemSettings.Hotline,
                        TaxCode = systemSettings.TaxCode,
                        Email = systemSettings.Email,
                        BankAccount = systemSettings.BankAccount,
                        BankAccountNumber = systemSettings.BankAccountNumber,
                        BankName = systemSettings.BankName,
                        AppPassword = systemSettings.AppPassword
                    };
                    return result;
                }
                else
                {
                    return Error.Failure("500", "Not found any system settings");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
