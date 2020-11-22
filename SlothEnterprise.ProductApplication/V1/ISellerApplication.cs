namespace SlothEnterprise.ProductApplication.V1
{
    public interface ISellerApplication
    {
        IProduct Product { get; set; }
        ISellerCompanyData CompanyData { get; set; }
    }
}