using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.OrderModule;
using Ecommerce.Service.Specification.OrderModuleSpecifications;
using Ecommerce.ServiceAbstractions;
using Ecommerce.Shared.DTOS.BasketDtos;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Ecommerce.Domain.Entities.ProductModule.Product;

namespace Ecommerce.Service
{
    public class PaymentService(IConfiguration _configuration, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork, IMapper _mapper) : IPaymentService
    {
        public  async Task<BasketDto> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var basket = await _basketRepository.GetBasketAsync(basketId) ?? throw new BasketNotFoundException(basketId);

            var productRepo =  _unitOfWork.GetRepository<Product, int>();
            foreach(var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
                
            }
            ArgumentNullException.ThrowIfNull(basket.DeliveryMethodId);
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(basket.DeliveryMethodId.Value) ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;

            var basketAmount = (long)(basket.Items.Sum(i => i.Quantity * i.Price) + deliveryMethod.Price)*100; // Convert to cents

            var _stripePaymentService = new PaymentIntentService();
            if(basket.PaymentIntentId is null)
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = basketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await _stripePaymentService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = basketAmount
                };
                await _stripePaymentService.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<BasketDto>(basket);
        }



        public async Task UpdatePaymentStatus(string jsonRequest, string stripeHeader)
        {
            var stripeEvent = EventUtility.ConstructEvent(
                jsonRequest,
                stripeHeader,
                _configuration["StripeSettings:EndPointSecret"]
            );

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                await UpdatePaymentFailedAsync(paymentIntent.Id);
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                await UpdatePaymentReceivedAsync(paymentIntent.Id);
            }
            else
            {
                // handle other event types
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
        }



        private async Task UpdatePaymentReceivedAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                .GetByIdAsync(new OrderWithPaymentIntentIdSpecification(paymentIntentId));

            order.OrderStatus = OrderStatus.PaymentReceived;

            _unitOfWork.GetRepository<Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task UpdatePaymentFailedAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                .GetByIdAsync(new OrderWithPaymentIntentIdSpecification(paymentIntentId));

            order.OrderStatus = OrderStatus.PaymentFailed;

            _unitOfWork.GetRepository<Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
// StripeSettings
// SecretKey