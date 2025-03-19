using BLL.DTOModels;
using BLL.ServicesInterfaces;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;

namespace BLL_EF
{
    public class BasketService : IBasketService
    {
        private readonly WebStoreContext _context;

        public BasketService(WebStoreContext context)
        {
            _context = context;
        }

        public void AddToBasket(BasketRequestDTO basketDto)
        {
            var product = _context.Products.Find(basketDto.ProductID);
            var user = _context.Users.Find(basketDto.UserID);

            if (product == null || user == null)
            {
                throw new ArgumentException("Product or User does not exist.");
            }

            var existingBasketPosition = _context.BasketPositions
                .FirstOrDefault(bp => bp.ProductID == basketDto.ProductID && bp.UserID == basketDto.UserID);

            if (existingBasketPosition != null)
            {
                existingBasketPosition.Amount += basketDto.Amount;
            }
            else
            {
                var basketPosition = new BasketPosition
                {
                    ProductID = basketDto.ProductID,
                    UserID = basketDto.UserID,
                    Amount = basketDto.Amount
                };

                _context.BasketPositions.Add(basketPosition);
            }

            _context.SaveChanges();
        }



        public void UpdateBasketAmount(int productId, int userId, int newAmount)
        {
            var basketPosition = _context.BasketPositions
                .FirstOrDefault(bp => bp.ProductID == productId && bp.UserID == userId);

            if (basketPosition == null)
            {
                throw new ArgumentException("Basket position not found. Add product first.");
            }

            basketPosition.Amount = newAmount;
            _context.SaveChanges();
        }

        public void RemoveFromBasket(int productId, int userId)
        {
            var basketPosition = _context.BasketPositions
                .FirstOrDefault(bp => bp.ProductID == productId && bp.UserID == userId);

            if (basketPosition == null)
            {
                throw new ArgumentException("Basket position not found.");
            }

            _context.BasketPositions.Remove(basketPosition);
            _context.SaveChanges();
        }
        public IEnumerable<BasketResponseDTO> GetUserBasket(int userId)
        {
            var query = _context.BasketPositions
                .Include(bp => bp.Product)
                .Where(bp => bp.UserID == userId);

            var result = query.Select(bp => new BasketResponseDTO
            {
                ProductID = bp.ProductID,
                ProductName = bp.Product != null ? bp.Product.Name : "Unknown",
                Amount = bp.Amount
            }).ToList();

            return result;
        }
    }
}
