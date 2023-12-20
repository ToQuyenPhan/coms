using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Common;
using ErrorOr;

namespace Coms.Application.Services.UserFlowDetails
{
    public class UserFlowDetailService : IUserFlowDetailService
    {
        private readonly IUserFlowDetailsRepository _userFlowDetailsRepository;
        private readonly IFlowDetailRepository _flowDetailRepository;

        public UserFlowDetailService(IUserFlowDetailsRepository userFlowDetailsRepository, 
                IFlowDetailRepository flowDetailRepository)
        {
            _userFlowDetailsRepository = userFlowDetailsRepository;
            _flowDetailRepository = flowDetailRepository;
        }

        public async Task<ErrorOr<PagingResult<UserFlowDetailResult>>> GetContractFlowDetails(int contractId, 
                int currentPage, int pageSize)
        {
            var contractFlowDetails = await _userFlowDetailsRepository.GetByContractId(contractId);
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
                        ContractId = contractFlowDetail.ContractId,
                        UserId = (int)contractFlowDetail.FlowDetail.UserId,
                        FlowDetailId = contractFlowDetail.FlowDetailId,
                        FlowRole = contractFlowDetail.FlowDetail.FlowRole.ToString(),
                    };
                    var flowDetail = await _flowDetailRepository.GetFlowDetail(contractFlowDetail.FlowDetailId);
                    flowDetailResult.FullName = flowDetail.User.FullName;
                    if (flowDetail.User.Image is not null)
                    {
                        flowDetailResult.UserImage = flowDetail.User.Image;
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
