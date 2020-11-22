using SlothEnterprise.ProductApplication.V1;
using System;

namespace SlothEnterprise.ProductApplication.Applications
{
    public class SellerCompanyData : ISellerCompanyData
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string DirectorName { get; set; }
        public DateTime Founded { get; set; }
    }
}