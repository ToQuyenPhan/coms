using Coms.Application.Common.Intefaces.Persistence;
using Coms.Application.Services.Services;
using Coms.Domain.Entities;
using Coms.Domain.Enum;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.FlowDetails
{
    public class FlowDetailService : IFlowDetailService
    {
        private readonly IFlowDetailRepository _flowDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFlowRepository _flowRepository;
        public FlowDetailService(IFlowDetailRepository flowDetailRepository, IUserRepository userRepository, IFlowRepository flowRepository)
        {
            _flowDetailRepository = flowDetailRepository;
            _userRepository = userRepository;
            _flowRepository = flowRepository;
        }

        //public async Task<ErrorOr<FlowDetailResult>> AddFlowDetail(FlowRole flowRole, int order, int flowId, int userId)
        //{
        //    try { 

        //        if (_userRepository.GetUser(userId).Result is not null && _userRepository.GetUser(userId).Result.Id != userId)
        //        {
        //            return Error.NotFound("User does not exist!");
        //        }
        //        if(_flowRepository.GetFlowById(flowId).Result is not null && _flowRepository.GetFlowById(flowId).Result.Id != flowId)
        //        {
        //            return Error.NotFound("Flow does not exist!");
        //        }

        //        var flowDetail = new FlowDetail
        //        {
        //            FlowRole = flowRole,
        //            Order = order,
        //            FlowID = flowId,
        //            UserId = userId
        //        };

        //        await _flowDetailRepository.AddFlowDetail(flowDetail);
        //        var created = await _flowDetailRepository.GetFlowDetail(flowDetail.FlowID);
        //        //if (created != null) { 
        //            var result = new FlowDetailResult
        //            {
        //                Id = flowDetail.Id,
        //                FlowRole = flowDetail.FlowRole,
        //                Order = flowDetail.Order,
        //                FlowID = flowDetail.FlowID,
        //                UserID = (int)flowDetail.UserId
        //            };
        //            return result;
        //        //}
        //        //else
        //        //{
        //        //    return Error.NotFound("404", "Flow is not found!");
        //        //}
        //    }
        //    catch (Exception ex)
        //        {
        //            return Error.Failure("500", ex.Message);
        //        }
        //}
        public async Task<ErrorOr<FlowDetailResult>> AddFlowDetail(FlowRole flowRole, int order, int flowId, int userId)
        {
            try
            {
                var user = await _userRepository.GetUser(userId);
                if (user is null)
                {
                    return Error.NotFound("User does not exist!");
                }

                var flow = await _flowRepository.GetFlowById(flowId);
                if (flow is null)
                {
                    return Error.NotFound("Flow does not exist!");
                }

                var flowDetail = new FlowDetail
                {
                    FlowRole = flowRole,
                    Order = order,
                    FlowID = flowId,
                    UserId = userId
                };

                await _flowDetailRepository.AddFlowDetail(flowDetail);
                var created = await _flowDetailRepository.GetFlowDetail(flowDetail.FlowID);

                var result = new FlowDetailResult
                {
                    Id = flowDetail.Id,
                    FlowRole = flowDetail.FlowRole,
                    Order = flowDetail.Order,
                    FlowID = flowDetail.FlowID,
                    UserID = (int)flowDetail.UserId
                };

                return result;
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }


        public async Task<ErrorOr<FlowDetailResult>> GetFlowDetail(int id)
        {
            try
            {
                if (_flowDetailRepository.GetFlowDetail(id).Result is not null)
                {
                    var flowDetail = await _flowDetailRepository.GetFlowDetail(id);
                    var result = new FlowDetailResult
                    {
                        Id = flowDetail.Id,
                        FlowRole = flowDetail.FlowRole,
                        Order = flowDetail.Order,
                        FlowID = flowDetail.FlowID
                    };
                    return result;
                }
                else
                {
                    return Error.NotFound("Flow Detail is not found!");
                }
            }
            catch (Exception ex)
            {
                return Error.Failure("500", ex.Message);
            }
        }
    }
}
