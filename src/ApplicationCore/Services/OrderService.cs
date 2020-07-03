﻿using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;

namespace Microsoft.eShopWeb.ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IAsyncRepository<CatalogItem> _itemRepository;

        /// <summary>
        /// Récupère les informations dans des variables en lecture seule pour le traitement.
        /// </summary>
        /// <param name="basketRepository"></param>
        /// <param name="itemRepository"></param>
        /// <param name="orderRepository"></param>
        public OrderService(IAsyncRepository<Basket> basketRepository,
            IAsyncRepository<CatalogItem> itemRepository,
            IAsyncRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
            _basketRepository = basketRepository;
            _itemRepository = itemRepository;
        }

        /// <summary>
        /// récupère les produits du panier pour en faire une commande.
        /// </summary>
        /// <param name="basketId"></param>
        /// <param name="shippingAddress"></param>
        public async Task CreateOrderAsync(int basketId, Address shippingAddress)
        {
            var basket = await _basketRepository.GetByIdAsync(basketId);
            Guard.Against.NullBasket(basketId, basket); //Empêche un panier vide.
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var catalogItem = await _itemRepository.GetByIdAsync(item.CatalogItemId);
                var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, catalogItem.PictureUri);
                var orderItem = new OrderItem(itemOrdered, item.UnitPrice, item.Quantity);
                items.Add(orderItem);
            }
            var order = new Order(basket.BuyerId, shippingAddress, items);

            await _orderRepository.AddAsync(order);
        }
    }
}
