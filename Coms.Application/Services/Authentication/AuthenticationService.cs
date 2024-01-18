using Coms.Application.Common.Intefaces.Authentication;
using Coms.Application.Common.Intefaces.Persistence;
using Coms.Domain.Common.Errors;
using Coms.Domain.Entities;
using ErrorOr;
using System.Net.Mail;
using System.Net;

namespace Coms.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly ISystemSettingsRepository _systemSettingsRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepository, IPartnerRepository partnerRepository, 
            ISystemSettingsRepository systemSettingsRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _partnerRepository = partnerRepository;
            _systemSettingsRepository = systemSettingsRepository;
        }

        public ErrorOr<AuthenticationResult> Login(string username, string password)
        {
            // Validate the user exist
            if (_userRepository.GetUserByUsername(username).Result is not User user)
            {
                return Errors.User.IncorrectUsername;
            }
            // Validate the password is correct
            if(user.Password != password)
            {
                return Errors.User.IncorrectPassword;
            }
            //Create JWT Token
            var token = _jwtTokenGenerator.GenerateToken(user);
            return new AuthenticationResult(token);
        }

        public ErrorOr<AuthenticationResult> EnterCode(string code)
        {
            // Validate the partner code exist
            if (_partnerRepository.GetPartnerByCode(code).Result is not Partner partner)
            {
                return Errors.Partner.IncorrectPartnerCode;
            }
            //Create JWT Token
            var token = _jwtTokenGenerator.GeneratePartnerToken(partner);
            return new AuthenticationResult(token);
        }

        public async Task<ErrorOr<string>> SendEmail(int partnerId)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var code = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var systemSettings = await _systemSettingsRepository.GetSystemSettings();
            var partner = await _partnerRepository.GetPartner(partnerId);
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(systemSettings.Email);
            message.To.Add(new MailAddress(partner.Email));
            string bodyMessage = "<div style='font-family: Arial, sans-serif;'>" +
                "<p style='font-size: 18px;'>Dear " + partner.CompanyName + ",</p>" +
                "<p style='font-size: 18px;'>To secure the system, we will provide you with a confirmation code.</p>" +
                "<p style='font-size: 18px;'>Here is your confirmation code to sign in into our system:</p>" +
                "<p style='font-size: 20px; font-weight: bold;'>Your code: " + code + "</p></div>";
            message.Subject = "Email Confirmation";
            message.Body = bodyMessage;
            message.IsBodyHtml = true; 
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(systemSettings.Email, systemSettings.AppPassword);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Send(message);
            return code;
        }
    }
}
