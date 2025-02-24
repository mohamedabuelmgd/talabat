using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specification;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;
using Product = Talabat.Core.Entities.Product;

namespace Talabate.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration
            ,IBasketRepository basketRepository
            ,IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;


            var shippingPrice = 0m;
            if(basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
                basket.ShippingPrice = deliveryMethod.Cost;
            }

            foreach (var item in basket.Items)//be sure that the price from front end is the same of database
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != product.Price)
                    item.Price = product.Price;
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;

            if(string.IsNullOrEmpty(basket.PaymentIntentId))//create payment intent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount=(long)basket.Items.Sum(item=> item.Quantity*item.Price*100)+(long) shippingPrice*100,
                    Currency="usd",
                    PaymentMethodTypes=new List<string>() { "card"}

                };
                intent=await service.CreateAsync(options);//intent have intent id and clientsecret key

                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else//update Payment intent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Quantity * item.Price * 100) + (long)shippingPrice * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId,options);
            }


            await _basketRepository.UpdateBasketAsync(basket);//because the chang of payment id and client secret
            return basket;
        }

        public async Task<Talabat.Core.Entities.Order_Aggregate.Order> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool IsSucceeded)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            if (IsSucceeded)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Complete();
            return order;
        }
    }
}
