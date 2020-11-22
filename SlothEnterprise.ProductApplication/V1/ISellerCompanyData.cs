using System;

namespace SlothEnterprise.ProductApplication.V1
{
    public interface ISellerCompanyData
    {
        string Name { get; set; }
        string Number { get; set; }
        string DirectorName { get; set; }
        DateTime Founded { get; set; }
    }
}