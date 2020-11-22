using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Products;
using SlothEnterprise.ProductApplication.V1;
using System;

namespace SlothEnterprise.ProductApplication
{
    public class ProductApplicationService
    {
        private readonly ISelectInvoiceService _selectInvoiceService;
        private readonly IConfidentialInvoiceService _confidentialInvoiceWebService;
        private readonly IBusinessLoansService _businessLoansService;

        public ProductApplicationService(
            ISelectInvoiceService selectInvoiceService,
            IConfidentialInvoiceService confidentialInvoiceWebService,
            IBusinessLoansService businessLoansService)
        {
            _selectInvoiceService = selectInvoiceService;
            _confidentialInvoiceWebService = confidentialInvoiceWebService;
            _businessLoansService = businessLoansService;
        }

        public int SubmitApplicationFor(ISellerApplication application)
        {
            // Assumed here that it may be possible to send a blank
            // Application to the method, so I have added defensive coding to it
            if (application == null) throw new NullReferenceException();

            switch (application.Product)
            {
                case SelectiveInvoiceDiscount sid:
                    return _selectInvoiceService.SubmitApplicationFor(
                            application.CompanyData.Number.ToString(),
                            sid.InvoiceAmount,
                            sid.AdvancePercentage
                        );

                case ConfidentialInvoiceDiscount cid:
                    return SubmitCompanyDataRequest(application.CompanyData, cid);

                case BusinessLoans loan:
                    return SubmitBusinessLoanRequest(application.CompanyData, loan);

                default:
                    throw new InvalidOperationException();
            }
        }

        private int SubmitBusinessLoanRequest(ISellerCompanyData companyData, BusinessLoans loan)
        {
            var companyDataRequest = CreateCompanyDataRequest(companyData);

            var loanApplication = new LoansRequest
            {
                InterestRatePerAnnum = loan.InterestRatePerAnnum,
                LoanAmount = loan.LoanAmount
            };

            var result = _businessLoansService.SubmitApplicationFor(companyDataRequest, loanApplication);

            if (!result.Success) return -1;

            return result.ApplicationId ?? -1;
        }

        private int SubmitCompanyDataRequest(ISellerCompanyData application, ConfidentialInvoiceDiscount cid)
        {
            var companyDataRequest = CreateCompanyDataRequest(application);

            var result = _confidentialInvoiceWebService.SubmitApplicationFor(
                   companyDataRequest, cid.TotalLedgerNetworth, cid.AdvancePercentage, cid.VatRate);

            if (!result.Success) return -1;

            return result.ApplicationId ?? -1;
        }

        private CompanyDataRequest CreateCompanyDataRequest(ISellerCompanyData companyData) =>
            new CompanyDataRequest()
            {
                CompanyFounded = companyData.Founded,
                CompanyNumber = companyData.Number,
                CompanyName = companyData.Name,
                DirectorName = companyData.DirectorName
            };
    }
}