using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace ECommerce.InfraStructure.Services
{
    public class PayPalService
    {
        private PayPalEnvironment _environment;
        private PayPalHttpClient _httpClient;
        public PayPalService(PayPalEnvironment environment, PayPalHttpClient httpClient)
        {
            _environment = environment;
            _httpClient = httpClient;
        }

        public PayPalService(string clientId, string secret)
        {
            _environment = new SandboxEnvironment(clientId, secret);
            _httpClient = new PayPalHttpClient(_environment);
        }

        public async Task<Order> CreateOrderAsync(decimal amount)
        {
            var orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "USD",
                            Value = amount.ToString()
                        }
                    }
                }
            };

            OrdersCreateRequest request = new OrdersCreateRequest();
            request.RequestBody(orderRequest);

            var response = await _httpClient.Execute(request);
            return response.Result<Order>();
        }

        public async Task<Order> CaptureOrder(string orderId)
        {
            OrdersCaptureRequest request = new OrdersCaptureRequest(orderId);
            var response = await _httpClient.Execute(request);
            return response.Result<Order>();
        }
    }
}
