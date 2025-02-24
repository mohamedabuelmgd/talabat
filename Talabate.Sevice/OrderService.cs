using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

namespace Talabate.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository
            , IUnitOfWork unitOfWork
            ,IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1.get basket by basket id
            var basket = await _basketRepository.GetBasketAsync(basketId);
            //2.get the item that iwant to put in the basket
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productItemOrdered = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                orderItems.Add(orderItem);
            }
            //3. sum the total of the price 

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);
            //4. get delivery methods from delivery repo by unite of work
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            //befor create order check if there is an order have the same payment intent that i hav
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            //if ther are order that have the same payment intent delete it and update the payment intent
            if(existingOrder!=null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }


            //5.create order
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal,basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().CreateAsync(order);
            //6.save change in database
            var result =await _unitOfWork.Complete();//return number of order that create 
            if (result<=0) return null;//not create order
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec =new OrderWithItemsAndDeliveryMethodSpecifications(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods;
        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpecifications(buyerEmail,orderId);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            return order;
        }
    }
}
