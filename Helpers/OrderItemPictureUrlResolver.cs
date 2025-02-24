﻿using AutoMapper;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        public IConfiguration Configuration { get; }
        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{Configuration["BaseApiUrl"]}{source.Product.PictureUrl}";
            return null;
        }
    }
}
