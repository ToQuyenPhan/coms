﻿using Coms.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Application.Services.ActionHistories
{
    public class ActionHistoryResult
    {
        public int Id { get; set; }
        public string ActionType { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? UserId { get; set; }
        public string UserName { get; set; }

        public int ContractId { get; set; }
        public string ContractName { get; set; }
    }
}
