using BLL.DTOModels.BLL.DTOModels;
using BLL.DTOModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL_MongoDb.MongoModels;
using BLL.ServicesInterfaces;

namespace BLL_MongoDb.MongoServices
{
    public class MongoOrderService : IOrderService
    {
        private readonly IMongoCollection<BasketPosition> _basket;
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Order> _orders;

        public MongoOrderService(IMongoDatabase database)
        {
            _basket = database.GetCollection<BasketPosition>("basket_positions");
            _products = database.GetCollection<Product>("products");
            _users = database.GetCollection<User>("users");
            _orders = database.GetCollection<Order>("orders");
        }

        public void GenerateOrder(int userId)
        {
            var userExists = _users.Find(u => u.Id == userId.ToString()).Any();
            if (!userExists)
                throw new ArgumentException("User does not exist.");

            var basketItems = _basket.Find(bp => bp.UserID == userId.ToString()).ToList();
            if (basketItems.Count == 0)
                throw new InvalidOperationException("Cannot generate an order with an empty basket.");

            var productIds = basketItems.Select(bp => bp.ProductID).ToList();
            var products = _products.Find(p => productIds.Contains(p.Id)).ToList();
            var productMap = products.ToDictionary(p => p.Id, p => p);

            var orderPositions = basketItems.Select(bp => new OrderPosition
            {
                ProductID = bp.ProductID,
                Amount = bp.Amount,
                Price = productMap.ContainsKey(bp.ProductID) ? productMap[bp.ProductID].Price : 0
            }).ToList();

            var order = new Order
            {
                UserID = userId.ToString(),
                Date = DateTime.UtcNow,
                IsPaid = false,
                OrderPositions = orderPositions
            };

            _orders.InsertOne(order);
            _basket.DeleteMany(bp => bp.UserID == userId.ToString());
        }

        public void PayOrder(int orderId, double amount)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, orderId.ToString());
            var order = _orders.Find(filter).FirstOrDefault();

            if (order == null)
                throw new ArgumentException("Order not found.");

            if (order.IsPaid)
                throw new InvalidOperationException("Order is already paid.");

            var totalAmount = order.OrderPositions.Sum(op => op.Price * op.Amount);

            if (amount != totalAmount)
                throw new ArgumentException("Payment amount does not match the order total.");

            var update = Builders<Order>.Update.Set(o => o.IsPaid, true);
            _orders.UpdateOne(filter, update);
        }

        public IEnumerable<OrderResponseDTO> GetOrders(bool? paidStatus = null, int? orderId = null, string? sortBy = "Date", bool descending = false)
        {
            var filter = Builders<Order>.Filter.Empty;
            var filters = new List<FilterDefinition<Order>>();

            if (paidStatus.HasValue)
                filters.Add(Builders<Order>.Filter.Eq(o => o.IsPaid, paidStatus.Value));
            if (orderId.HasValue)
                filters.Add(Builders<Order>.Filter.Eq(o => o.Id, orderId.Value.ToString()));

            if (filters.Any())
                filter = Builders<Order>.Filter.And(filters);

            var orders = _orders.Find(filter).ToList();

            switch (sortBy?.ToLower())
            {
                case "date":
                    orders = descending ? orders.OrderByDescending(o => o.Date).ToList() : orders.OrderBy(o => o.Date).ToList();
                    break;
                case "value":
                    orders = descending ? orders.OrderByDescending(o => o.OrderPositions.Sum(op => op.Price * op.Amount)).ToList()
                                        : orders.OrderBy(o => o.OrderPositions.Sum(op => op.Price * op.Amount)).ToList();
                    break;
                case "id":
                    orders = descending ? orders.OrderByDescending(o => o.Id).ToList() : orders.OrderBy(o => o.Id).ToList();
                    break;
                default:
                    orders = orders.OrderBy(o => o.Date).ToList();
                    break;
            }

            return orders.Select(o => new OrderResponseDTO
            {
                ID = o.Id,
                Value = o.OrderPositions.Sum(op => op.Price * op.Amount),
                IsPaid = o.IsPaid,
                Date = o.Date
            });
        }

        public IEnumerable<OrderDetailsResponseDTO> GetOrderDetails(int orderId)
        {
            var order = _orders.Find(o => o.Id == orderId.ToString()).FirstOrDefault();
            if (order == null)
                throw new ArgumentException("Order not found.");

            var productIds = order.OrderPositions.Select(op => op.ProductID).ToList();
            var products = _products.Find(p => productIds.Contains(p.Id)).ToList();
            var productMap = products.ToDictionary(p => p.Id, p => p.Name);

            return order.OrderPositions.Select(op => new OrderDetailsResponseDTO
            {
                ProductName = productMap.ContainsKey(op.ProductID) ? productMap[op.ProductID] : null,
                Price = op.Price,
                Amount = op.Amount,
                Total = op.Price * op.Amount
            });
        }
    }
}
