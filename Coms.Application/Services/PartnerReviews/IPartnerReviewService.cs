using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coms.Application.Services.Templates;
using ErrorOr;

namespace Coms.Application.Services.PartnerReviews
{
    public interface IPartnerReviewService 
    {
        Task<ErrorOr<PartnerReviewResult>> AddPartnerReview(int partnerId,int userId, int contractId);
    }
}
