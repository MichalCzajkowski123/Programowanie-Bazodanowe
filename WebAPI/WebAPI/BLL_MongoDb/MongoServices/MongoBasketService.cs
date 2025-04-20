using BLL.DTOModels;
using BLL.ServicesInterfaces;
using BLL_MongoDb.MongoModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_MongoDb.MongoServices
{
    public class MongoBasketService : IBasketService
    {
        private readonly IMongoCollection<BasketPosition> _basket;
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Order> _orders;

        public MongoBasketService(IMongoDatabase database)
        {
            _basket = database.GetCollection<BasketPosition>("basket_positions");
            _products = database.GetCollection<Product>("products");
            _users = database.GetCollection<User>("users");
            _orders = database.GetCollection<Order>("orders");
        }

        public void AddToBasket(BasketRequestDTO basketDto)
        {
            var productExists = _products.Find(p => p.Id == basketDto.ProductID).Any();
            var userExists = _users.Find(u => u.Id == basketDto.UserID).Any();

            if (!productExists || !userExists)
                throw new ArgumentException("Product or User does not exist.");

            var existing = _basket.Find(bp => bp.ProductID == basketDto.ProductID && bp.UserID == basketDto.UserID).FirstOrDefault();

            if (existing != null)
            {
                var update = Builders<BasketPosition>.Update.Inc(x => x.Amount, basketDto.Amount);
                _basket.UpdateOne(x => x.Id == existing.Id, update);
            }
            else
            {
                var newItem = new BasketPosition
                {
                    ProductID = basketDto.ProductID,
                    UserID = basketDto.UserID,
                    Amount = basketDto.Amount
                };
                _basket.InsertOne(newItem);
            }
        }

        public void UpdateBasketAmount(int productId, int userId, int newAmount)
        {
            var filter = Builders<BasketPosition>.Filter.Where(bp => bp.ProductID == productId && bp.UserID == userId);
            var update = Builders<BasketPosition>.Update.Set(bp => bp.Amount, newAmount);
            var result = _basket.UpdateOne(filter, update);

            if (result.MatchedCount == 0)
                throw new ArgumentException("Basket position not found. Add product first.");
        }

        public void RemoveFromBasket(int productId, int userId)
        {
            var result = _basket.DeleteOne(bp => bp.ProductID == productId && bp.UserID == userId);

            if (result.DeletedCount == 0)
                throw new ArgumentException("Basket position not found.");
        }

        public IEnumerable<BasketResponseDTO> GetUserBasket(int userId)
        {
            var basketItems = _basket.Find(bp => bp.UserID == userId).ToList();
            var productIds = basketItems.Select(bp => bp.ProductID).ToList();
            var products = _products.Find(p => productIds.Contains(p.Id)).ToList();
            var productMap = products.ToDictionary(p => p.Id, p => p.Name);

            return basketItems.Select(bp => new BasketResponseDTO
            {
                ProductID = bp.ProductID,
                ProductName = productMap.ContainsKey(bp.ProductID) ? productMap[bp.ProductID] : "Unknown",
                Amount = bp.Amount
            });
        }
    }
}
