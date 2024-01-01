using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coms.Contracts.Signs
{
    public class UploadRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
