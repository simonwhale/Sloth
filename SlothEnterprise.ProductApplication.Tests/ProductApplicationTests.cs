using Moq;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;
using SlothEnterprise.ProductApplication.Tests.Models;
using SlothEnterprise.ProductApplication.V1;
using System;
using Xunit;

namespace SlothEnterprise.ProductApplication.Tests
{
    
    public class ProductApplicationTests
    {
        private ISellerCompanyData companyData = new SellerCompanyData() { Name = "My test Company", DirectorName = "Sloth Junior", Founded = DateTime.Now, Number = "1234" };
        
        private readonly Mock<ISelectInvoiceService> _selectInvoiceService = new Mock<ISelectInvoiceService>();
        private readonly Mock<IConfidentialInvoiceService> _confidentialInvoiceService = new Mock<IConfidentialInvoiceService>();
        private readonly Mock<IBusinessLoansService> _businessLoanService = new Mock<IBusinessLoansService>();

        [Fact]
        public void Should_Throw_Error_if_application_is_Null()
        {
            ISellerApplication sellerApplciation = null;
            var productApplciationservice = new ProductApplicationService(_selectInvoiceService.Object, _confidentialInvoiceService.Object, _businessLoanService.Object);

            Assert.Throws<NullReferenceException>(() => productApplciationservice.SubmitApplicationFor(sellerApplciation));
        }

        [Fact]
        public void Should_Throw_Error_as_no_product()
        {
            ISellerApplication sellerApplication = new SellerApplication()
            {
                CompanyData = companyData
            };

            var productApplciationservice = new ProductApplicationService(_selectInvoiceService.Object, _confidentialInvoiceService.Object, _businessLoanService.Object);

            Assert.Throws<InvalidOperationException>(() => productApplciationservice.SubmitApplicationFor(sellerApplication));
        }

        [Fact]
        public void Submit_with_Business_Loan()
        {
            ISellerApplication sellerApplication = new SellerApplication()
            {
                CompanyData = companyData,
                Product = new BusinessLoans()
                {
                    Id = 1,
                    LoanAmount = 100,
                    InterestRatePerAnnum = 5
                }
            };

            _businessLoanService.Setup(x => x.SubmitApplicationFor(It.IsAny<CompanyDataRequest>(), It.IsAny<LoansRequest>())).Returns(new ApplicationResult() { ApplicationId = 1, Success = true });

            var productApplciationservice = new ProductApplicationService(_selectInvoiceService.Object, _confidentialInvoiceService.Object, _businessLoanService.Object);

            var result = productApplciationservice.SubmitApplicationFor(sellerApplication);
            Assert.Equal(1, result);
        }
    }
}