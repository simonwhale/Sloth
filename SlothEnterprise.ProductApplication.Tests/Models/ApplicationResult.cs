using SlothEnterprise.External;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlothEnterprise.ProductApplication.Tests.Models
{
    public class ApplicationResult : IApplicationResult
    {
        public int? ApplicationId { get; set; }
        public bool Success { get; set; }
        public IList<string> Errors { get; set; }
    }
}
