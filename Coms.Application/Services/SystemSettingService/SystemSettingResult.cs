using System.ComponentModel.DataAnnotations;

namespace Coms.Application.Services.SystemSettingService
{
    public class SystemSettingResult
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Hotline { get; set; }
        public string TaxCode { get; set; }
        public string Email { get; set; }
        public string BankAccount { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string AppPassword { get; set; }
    }
}
