using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.UserFlowDetails;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using Firebase.Storage;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.Diagnostics.Contracts;
using System.Net.Mail;
using System.Net;
using System.Reflection;

namespace Coms.Application.Services.Signs
{
    public class SignService : ISignService
    {
        private string Bucket = "coms-64e4a.appspot.com";
        private readonly IContractFileRepository _contractFileRepository;
        private readonly IContractAnnexFileRepository _contractAnnexFileRepository;
        private readonly ILiquidationRecordFileRepository _liquidationRecordFileRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IContractAnnexRepository _contractAnnexRepository;
        private readonly ILiquidationRecordRepository _liquidationRecordRepository;
        private readonly IContractFlowDetailsRepository _contractFlowDetailsRepository;
        private readonly IPartnerReviewRepository _partnerReviewRepository;
        private readonly IPartnerSignRepository _partnerSignRepository;
        private readonly ISystemSettingsRepository _systemSettingsRepository;

        public SignService(IContractFileRepository contractFileRepository,
            IContractAnnexFileRepository contractAnnexFileRepository,
            ILiquidationRecordFileRepository liquidationRecordFileRepository,
            IContractRepository contractRepository,
            IContractAnnexRepository contractAnnexRepository,
            ILiquidationRecordRepository liquidationRecordRepository,
            IContractFlowDetailsRepository contractFlowDetailsRepository,
            IPartnerReviewRepository partnerReviewRepository,
            IPartnerSignRepository partnerSignRepository,
            ISystemSettingsRepository systemSettingsRepository)
        {
            _contractFileRepository = contractFileRepository;
            _contractAnnexFileRepository = contractAnnexFileRepository;
            _liquidationRecordFileRepository = liquidationRecordFileRepository;
            _contractRepository = contractRepository;
            _contractAnnexRepository = contractAnnexRepository;
            _liquidationRecordRepository = liquidationRecordRepository;
            _contractFlowDetailsRepository = contractFlowDetailsRepository;
            _partnerReviewRepository = partnerReviewRepository;
            _partnerSignRepository = partnerSignRepository;
            _systemSettingsRepository = systemSettingsRepository;
        }
        public async Task<ErrorOr<ResponseModel>> UploadVersion(Guid fileId, byte[] document)
        {
            try
            {
                if (document != null && document.Length > 0)
                {
                    //contract
                    var contractFile = await _contractFileRepository.GetContractFileById(fileId);
                    if (contractFile is not null)
                    {
                        contractFile.FileData = document;
                        contractFile.UpdatedDate = DateTime.Now;
                        await _contractFileRepository.Update(contractFile);
                        var contract = await _contractRepository.GetContract(contractFile.ContractId);
                        if (contract is not null)
                        {
                            //Update file contract
                            MemoryStream stream = new MemoryStream(contractFile.FileData);
                            string filePath = Path.Combine(Environment.CurrentDirectory, "Contracts", contract.Id + ".pdf");
                            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                            stream.Position = 0;
                            stream.CopyTo(fileStream);

                            //update file in firebase
                            fileStream.Position = 0;
                            var task = new FirebaseStorage(Bucket)
                                .Child("contracts")
                                .Child(contract.Id + ".pdf")
                                .PutAsync(fileStream);
                            string link = "https://firebasestorage.googleapis.com/v0/b/coms-64e4a.appspot.com/o/contracts%2F" + contract.Id
                                + ".pdf?alt=media&token=451cd9c9-b548-48f3-b69c-0129a0c0836c";
                            var downloadUrl = await task;
                            fileStream.Close();

                            var partnerReview = await _partnerReviewRepository.GetByContractId(contract.Id);
                            if (contract.Status == DocumentStatus.Completed)
                            {
                                var partnerSign = new PartnerSign()
                                {
                                    ContractId = contract.Id,
                                    SignedAt = DateTime.Now,
                                    PartnerId = partnerReview.PartnerId
                                };
                                await _partnerSignRepository.AddPartnerSign(partnerSign);
                                contract.Status = DocumentStatus.Finalized;
                                contract.UpdatedDate = DateTime.Now;
                                contract.Link = link;
                            }
                            else
                            {
                                if (partnerReview.IsApproved == true)
                                {
                                    var flowDetails = await _contractFlowDetailsRepository.GetByContractIdForSign(contractFile.ContractId);
                                    var flowDetail = flowDetails.FirstOrDefault(cfd => cfd.FlowDetail.FlowRole.Equals(FlowRole.Signer));
                                    if (flowDetail.Status.Equals(FlowDetailStatus.Signed))
                                    {
                                        return Error.Conflict("409", "You are already " + flowDetail.Status.ToString().ToLower() + "!");
                                    }
                                    else
                                    {
                                        flowDetail.Status = FlowDetailStatus.Signed;
                                        await _contractFlowDetailsRepository.UpdateContractFlowDetail(flowDetail);
                                        contract.Status = DocumentStatus.Completed;
                                        contract.UpdatedDate = DateTime.Now;
                                        contract.Link = link;
                                    }                                  
                                }
                                else
                                {
                                    return Error.Conflict("409", partnerReview.Partner.CompanyName + "is approving!");
                                }                                
                                await SendEmailSignContract(contract.Id);
                            }
                            await _contractRepository.UpdateContract(contract);
                        }
                        return new ResponseModel()
                        {
                            isSuccess = true,
                            code = 200,
                            responseSuccess = Convert.ToBase64String(contractFile.FileData),
                            responseFailed = null
                        };
                    }

                    //contractAnnex
                    var annexFile = await _contractAnnexFileRepository.GetContractAnnexFileById(fileId);
                    if (annexFile != null)
                    {
                        annexFile.FileData = document;
                        annexFile.UpdatedDate = DateTime.Now;
                        await _contractAnnexFileRepository.Update(annexFile);
                        var contractAnnex = _contractAnnexRepository.GetContractAnnexesById(annexFile.ContractAnnexId).Result;
                        if (contractAnnex is not null)
                        {
                            MemoryStream stream = new MemoryStream(annexFile.FileData);
                            string filePath = Path.Combine(Environment.CurrentDirectory, "ContractAnnexes", contractAnnex.Id + ".pdf");
                            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                            stream.Position = 0;
                            stream.CopyTo(fileStream);

                            //update file in firebase
                            fileStream.Position = 0;
                            var task = new FirebaseStorage(Bucket)
                                .Child("contractAnnexes")
                                .Child(contractAnnex.Id + ".pdf")
                                .PutAsync(fileStream);
                            string link = "https://firebasestorage.googleapis.com/v0/b/coms-64e4a.appspot.com/o/contractAnnexes%2F" + contractAnnex.Id
                                + ".pdf?alt=media&token=451cd9c9-b548-48f3-b69c-0129a0c0836c";
                            var downloadAnnexUrl = await task;
                            fileStream.Close();

                            var partnerReview = await _partnerReviewRepository.GetByContractAnnexId(contractAnnex.Id);
                            if (contractAnnex.Status == DocumentStatus.Completed)
                            {

                                var partnerSign = new PartnerSign()
                                {
                                    ContractAnnexId = contractAnnex.Id,
                                    SignedAt = DateTime.Now,
                                    PartnerId = partnerReview.PartnerId
                                };
                                await _partnerSignRepository.AddPartnerSign(partnerSign);
                                contractAnnex.Status = DocumentStatus.Finalized;
                                contractAnnex.UpdatedDate = DateTime.Now;
                                contractAnnex.Link = link;
                            }
                            else
                            {
                                if (partnerReview.IsApproved == true)
                                {
                                    var flowDetails = await _contractFlowDetailsRepository.GetByContractAnnexIdForSign(contractAnnex.Id);
                                    var flowDetail = flowDetails.FirstOrDefault(cfd => cfd.FlowDetail.FlowRole.Equals(FlowRole.Signer));
                                    if (flowDetail.Status.Equals(FlowDetailStatus.Signed))
                                    {
                                        return Error.Conflict("409", "You are already " + flowDetail.Status.ToString().ToLower() + "!");
                                    }
                                    else
                                    {
                                        flowDetail.Status = FlowDetailStatus.Signed;
                                    }
                                    await _contractFlowDetailsRepository.UpdateContractFlowDetail(flowDetail);
                                    contractAnnex.Status = DocumentStatus.Completed;
                                    contractAnnex.UpdatedDate = DateTime.Now;
                                    contractAnnex.Link = link;
                                    await SendEmailSignContractAnnex(contractAnnex.Id);
                                }
                                else
                                {
                                    return Error.Conflict("409", partnerReview.Partner.CompanyName + "is approving!");
                                }
                            }

                            await _contractAnnexRepository.UpdateContractAnnexes(contractAnnex);
                        }
                        return new ResponseModel()
                        {
                            isSuccess = true,
                            code = 200,
                            responseSuccess = Convert.ToBase64String(annexFile.FileData),
                            responseFailed = null
                        };
                    }

                    //liquidationRecord
                    /*var liquidationFile = await _liquidationRecordFileRepository.GetLiquidationRecordFileById(fileId);
                    if (liquidationFile != null)
                    {
                        liquidationFile.FileData = document;
                        liquidationFile.UpdatedDate = DateTime.Now;
                        await _liquidationRecordFileRepository.Update(liquidationFile);
                        var liquidation = _liquidationRecordRepository.GetLiquidationRecordsById(liquidationFile.LiquidationRecordId).Result;
                        if (liquidation is not null)
                        {
                            MemoryStream stream = new MemoryStream(liquidationFile.FileData);
                            string filePath = Path.Combine(Environment.CurrentDirectory, "LiquidationRecords", liquidation.Id + ".pdf");
                            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                            stream.Position = 0;
                            stream.CopyTo(fileStream);

                            //update file in firebase
                            fileStream.Position = 0;
                            var task = new FirebaseStorage(Bucket)
                                .Child("liquidationRecords")
                                .Child(liquidation.Id + ".pdf")
                                .PutAsync(fileStream);
                            string link = "https://firebasestorage.googleapis.com/v0/b/coms-64e4a.appspot.com/o/liquidationRecords%2F" + liquidation.Id
                                + ".pdf?alt=media&token=451cd9c9-b548-48f3-b69c-0129a0c0836c";
                            var downloadLiquidationUrl = await task;
                            fileStream.Close();

                            if (liquidation.Status == DocumentStatus.Completed)
                            {
                                var partnerReview = await _partnerReviewRepository.GetByContractAnnexId(liquidation.Id);
                                var partnerSign = new PartnerSign()
                                {
                                    LiquidationRecordId = liquidation.Id,
                                    SignedAt = DateTime.Now,
                                    PartnerId = partnerReview.PartnerId
                                };
                                await _partnerSignRepository.AddPartnerSign(partnerSign);
                                liquidation.Status = DocumentStatus.Finalized;
                                liquidation.UpdatedDate = DateTime.Now;
                                liquidation.Link = link;
                                var contract = await _contractRepository.GetContract(liquidation.ContractId);
                                if (contract != null)
                                {
                                    contract.Status = DocumentStatus.Liquidated;
                                    contract.UpdatedDate = DateTime.Now;
                                    await _contractRepository.UpdateContract(contract);
                                }
                                else
                                {
                                    return Error.Failure("Quá trình thanh lý gặp vấn đề");
                                }
                            }
                            else
                            {
                                var flowDetails = await _contractFlowDetailsRepository.GetByLiquidationRecordForSign(liquidation.Id);
                                var flowDetail = flowDetails.FirstOrDefault(cfd => cfd.FlowDetail.FlowRole.Equals(FlowRole.Signer));
                                if (flowDetail.Status.Equals(FlowDetailStatus.Signed))
                                {
                                    return Error.Conflict("409", "You are already " + flowDetail.Status.ToString().ToLower() + "!");
                                }
                                else
                                {
                                    flowDetail.Status = FlowDetailStatus.Signed;
                                }
                                await _contractFlowDetailsRepository.UpdateContractFlowDetail(flowDetail);
                                liquidation.Status = DocumentStatus.Completed;
                                liquidation.UpdatedDate = DateTime.Now;
                                liquidation.Link = link;
                            }
                            await _liquidationRecordRepository.UpdateLiquidationRecord(liquidation);
                        }
                        return new ResponseModel()
                        {
                            isSuccess = true,
                            code = 200,
                            responseSuccess = Convert.ToBase64String(liquidationFile.FileData),
                            responseFailed = null

                        };
                    }*/

                    return new ResponseModel()
                    {
                        isSuccess = false,
                        code = 404,
                        responseSuccess = null,
                        responseFailed = "Không tìm thấy file!",
                    };
                }
                else
                {
                    // Xử lý khi document không hợp lệ
                    return Error.Failure("Document không hợp lệ");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }

        private async Task SendEmailSignContract(int contractId)
        {
            var systemSettings = await _systemSettingsRepository.GetSystemSettings();
            var partnerReview = await _partnerReviewRepository.GetByContractId(contractId);
            var contract = await _contractRepository.GetContract(contractId);
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(systemSettings.Email);
            message.To.Add(new MailAddress(partnerReview.Partner.Email));
            string bodyMessage = "<div style='font-family: Arial, sans-serif;'>" +
                "<p style='font-size: 18px;'>Dear " + partnerReview.Partner.CompanyName + ",</p>" +
                "<p style='font-size: 18px;'>You have a new contract to sign. ( Contract code: " + contract?.Code + ")</p>" +
                "<p style='font-size: 18px;'>Here is your code to sign in into our system:</p>" +
                "<p style='font-size: 20px; font-weight: bold;'>Your code: " + partnerReview.Partner.Code + "</p>" +
                "<p style='font-size: 18px;'>Please login to the website: <a href='https://quanlyhopdong.hisoft.vn/partner-code' style='color: blue;'>https://quanlyhopdong.hisoft.vn/partner-code</a> to see details</p>" +
                "</div>";
            message.Subject = "Sign New Contract";
            message.Body = bodyMessage;
            message.IsBodyHtml = true; // This is to notify the MailMessage that the body is in HTML
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(systemSettings.Email, systemSettings.AppPassword);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }

        private async Task SendEmailSignContractAnnex(int contractAnnexId)
        {
            var systemSettings = await _systemSettingsRepository.GetSystemSettings();
            var partnerReview = await _partnerReviewRepository.GetByContractAnnexId(contractAnnexId);
            var contractAnnex = await _contractAnnexRepository.GetContractAnnexesById(contractAnnexId);
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(systemSettings.Email);
            message.To.Add(new MailAddress(partnerReview.Partner.Email));
            string bodyMessage = "<div style='font-family: Arial, sans-serif;'>" +
                "<p style='font-size: 18px;'>Dear " + partnerReview.Partner.CompanyName + ",</p>" +
                "<p style='font-size: 18px;'>You have a new contract annex to sign. ( Contract annex code: " + contractAnnex?.Code + ")</p>" +
                "<p style='font-size: 18px;'>Here is your code to sign in into our system:</p>" +
                "<p style='font-size: 20px; font-weight: bold;'>Your code: " + partnerReview.Partner.Code + "</p>" +
                "<p style='font-size: 18px;'>Please login to the website: <a href='https://quanlyhopdong.hisoft.vn/partner-code' style='color: blue;'>https://quanlyhopdong.hisoft.vn/partner-code</a> to see details</p>" +
                "</div>";
            message.Subject = "Sign New Contract annex";
            message.Body = bodyMessage;
            message.IsBodyHtml = true; // This is to notify the MailMessage that the body is in HTML
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(systemSettings.Email, systemSettings.AppPassword);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }


        private Dictionary<string, string> ToDictionary(object obj)
        {
            var dictionary = obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null).ToString());
            return dictionary;
        }

        private async Task SendNotification(string title, string body, Dictionary<string, string> data, string uid)
        {
            var message = new FirebaseAdmin.Messaging.Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = body
                },
                Data = data,
                Topic = uid
            };
            string fileConfigPath = Path.Combine(Directory.GetCurrentDirectory(), @"coms-64e4a-firebase-adminsdk-rzqca-3869e0f5ce.json");
            //var stream = new FileStream(fileConfigPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            if (FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance is null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(fileConfigPath)
                });
            }
            await FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
