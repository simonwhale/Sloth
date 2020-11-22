using SlothEnterprise.ProductApplication.V1;

namespace SlothEnterprise.ProductApplication.Applications
{
    public class SellerApplication : ISellerApplication
    {
        public IProduct Product { get; set; }
        public ISellerCompanyData CompanyData { get; set; }
    }
}