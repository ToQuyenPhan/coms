using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.UserFlowDetails
{
    public class UserFlowDetailService : IUserFlowDetailService
    {
        private readonly IUserFlowDetailsRepository _userFlowDetailsRepository;

        public UserFlowDetailService(IUserFlowDetailsRepository userFlowDetailsRepository)
        {
            _userFlowDetailsRepository = userFlowDetailsRepository;
        }

        public async Task<ErrorOr<PagingResult<UserFlowDetailResult>>> GetContractFlowDetails(int contractId, 
                int currentPage, int pageSize)
        {
            var contractFlowDetails = await _userFlowDetailsRepository.GetUserFlowDetailsByContractId(contractId);
            if(contractFlowDetails is not null )
            {
                IList<UserFlowDetailResult> results = new List<UserFlowDetailResult>();
                foreach(var contractFlowDetail in contractFlowDetails)
                {
                    var flowDetailResult = new UserFlowDetailResult()
                    {
                        Id = contractFlowDetail.Id,
                        Status = (int) contractFlowDetail.Status,
                        StatusString = contractFlowDetail.Status.ToString(),
                        UserId = contractFlowDetail.UserId,
                        FullName = contractFlowDetail.User.FullName,
                        ContractId = contractFlowDetail.ContractId,
                        FlowDetailId = contractFlowDetail.FlowDetailId,
                        FlowRole = contractFlowDetail.FlowDetail.FlowRole.ToString(),
                    };
                    if(contractFlowDetail.User.Image is not null)
                    {
                        flowDetailResult.UserImage = contractFlowDetail.User.Image;
                    }
                    results.Add(flowDetailResult);
                }
                int total = results.Count();
                if (currentPage > 0 && pageSize > 0)
                {
                    results = results.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                }
                return new PagingResult<UserFlowDetailResult>(results, total, currentPage, pageSize);
            }
            else
            {
                return Error.NotFound("404", "Not found any flow details!");
            }
        }
    }
}
