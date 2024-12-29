using App.Core.Interface;
using Domain.Entity.Appointments;
using Domain.Model;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StripePaymentService : IStripePaymentService
    {
        private readonly IConfiguration _configuration;
        public StripePaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<StripePaymentResponse> CreateStripePayment(Striprequestmodel striprequestmodel)
        {
            try
            {
                StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"]; ; // test api key

                var productOptions = new ProductCreateOptions
                {
                    Name = "StripeDemo",
                };
                var productService = new ProductService();
                Stripe.Product product = productService.Create(productOptions);

                var basePriceOptions = new PriceCreateOptions
                {
                    Nickname = "StripeDemo",
                    UnitAmount = Convert.ToInt64(striprequestmodel.Amount * 100),
                    Currency = "usd",
                    Product = product.Id,
                    BillingScheme = "per_unit",
                };
                var priceService = new PriceService();
                Price basePrice = priceService.Create(basePriceOptions);

                var paymentMethodOptions = new PaymentMethodCreateOptions
                {
                    Type = "card",
                    Card = new PaymentMethodCardOptions
                    {
                        Token = striprequestmodel.SourceToken,
                    },
                };
                var paymentMethodService = new PaymentMethodService();
                var paymentMethod = paymentMethodService.Create(paymentMethodOptions);

                var optionsCustomer = new CustomerCreateOptions
                {
                    Name = striprequestmodel.CustomerName,
                    Email = striprequestmodel.CustomerEmail,
                    PaymentMethod = paymentMethod.Id,
                    InvoiceSettings = new CustomerInvoiceSettingsOptions
                    {
                        DefaultPaymentMethod = paymentMethod.Id,
                    },
                };

                var serviceCustomer = new CustomerService();
                Stripe.Customer customer = serviceCustomer.Create(optionsCustomer);

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(striprequestmodel.Amount * 100), // Convert amount to cents
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" },
                    Description = "StripeDemo Payment",
                    PaymentMethod = paymentMethod.Id,
                    Customer = customer.Id
                };
                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                // Confirm the Payment Intent
                var confirmOptions = new PaymentIntentConfirmOptions { };
                paymentIntent = await service.ConfirmAsync(paymentIntent.Id, confirmOptions);

                return new StripePaymentResponse
                {
                    Id = paymentIntent.Id, // Assign PaymentIntent ID
                    Status = 200,          // HTTP status code
                    Message = "Payment Successful" // Success message
                };


            }
            catch (StripeException ex)
            {
                // Log the exception (ex.Message) and handle it appropriately
                Console.WriteLine($"StripeException: {ex.Message}");
                var model = new PaymentIntent();
                model.Status = ex.Message;
                return new StripePaymentResponse
                {
                               // No payment ID in case of an error
                    Status = 500,           // HTTP status code for server errors
                    Message = ex.Message    // Error message from the exception
                };


            }
            catch (Exception ex)
            {
                // Log the exception (ex.Message) and handle it appropriately
                Console.WriteLine($"Exception: {ex.Message}");
                var model = new PaymentIntent();
                model.Status = ex.Message;
                return new StripePaymentResponse
                {
                             // No payment ID in case of an error
                    Status = 500,           // HTTP status code for server errors
                    Message = ex.Message    // Error message from the exception
                };

            }
        }

       
    }
}
