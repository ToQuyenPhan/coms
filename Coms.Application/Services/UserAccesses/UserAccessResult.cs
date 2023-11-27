using Coms.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.UserAccesses
{
    public class UserAccessResult
    {
        public int? UserId { get; set; }
        //public string UserName { get; set; }
        public int? AccessId { get; set; }
        public string AccessRole { get; set; }
    }
}
