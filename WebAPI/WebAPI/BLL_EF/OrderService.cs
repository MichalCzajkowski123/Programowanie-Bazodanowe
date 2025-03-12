using BLL.DTOModels.BLL.DTOModels;
using BLL.DTOModels;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Model;
using BLL.ServicesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL_EF
{
    public class OrderService : IOrderService
    {
        private readonly WebStoreContext _context;

        public OrderService(WebStoreContext context)
        {
            _context = context;
        }

        public void GenerateOrder(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                throw new ArgumentException("User does not exist.");
            }

            var basketPositions = _context.BasketPositions
                .Include(bp => bp.Product)
                .Where(bp => bp.UserID == userId)
                .ToList();

            if (basketPositions.Count == 0)
            {
                throw new InvalidOperationException("Cannot generate an order with an empty basket.");
            }


            var order = new Order
            {
                UserID = userId,
                Date = DateTime.UtcNow,
                IsPaid = false,
                OrderPositions = new List<OrderPosition>()
            };

            foreach (var basketPosition in basketPositions)
            {
                var orderPosition = new OrderPosition
                {
                    ProductID = basketPosition.ProductID,
                    Amount = basketPosition.Amount,
                    Price = basketPosition.Product.Price
                };

                order.OrderPositions.Add(orderPosition);
            }

            _context.Orders.Add(order);

            _context.BasketPositions.RemoveRange(basketPositions);

            _context.SaveChanges();
        }

        public void PayOrder(int orderId, double amount)
        {
            var order = _context.Orders
                .Include(o => o.OrderPositions)
                .FirstOrDefault(o => o.ID == orderId);

            if (order == null)
            {
                throw new ArgumentException("Order not found.");
            }

            if (order.IsPaid)
            {
                throw new InvalidOperationException("Order is already paid.");
            }

            double totalAmount = order.OrderPositions.Sum(op => op.Price * op.Amount);

            if (amount != totalAmount)
            {
                throw new ArgumentException("Payment amount does not match the order total.");
            }

            order.IsPaid = true;
            _context.SaveChanges();
        }

        public IEnumerable<OrderResponseDTO> GetOrders(bool? paidStatus = null, int? orderId = null, string? sortBy = "Date", bool descending = false)
        {
            var query = _context.Orders
                .Include(o => o.OrderPositions)
                .AsQueryable();

            if (paidStatus.HasValue)
            {
                query = query.Where(o => o.IsPaid == paidStatus.Value);
            }

            if (orderId.HasValue)
            {
                query = query.Where(o => o.ID == orderId.Value);
            }

            switch (sortBy?.ToLower())
            {
                case "date":
                    query = descending ? query.OrderByDescending(o => o.Date) : query.OrderBy(o => o.Date);
                    break;
                case "value":
                    query = descending ? query.OrderByDescending(o => o.OrderPositions.Sum(op => op.Price * op.Amount))
                                      : query.OrderBy(o => o.OrderPositions.Sum(op => op.Price * op.Amount));
                    break;
                case "id":
                    query = descending ? query.OrderByDescending(o => o.ID) : query.OrderBy(o => o.ID);
                    break;
                default:
                    query = query.OrderBy(o => o.Date);
                    break;
            }

            var result = query.Select(o => new OrderResponseDTO
            {
                ID = o.ID,
                Value = o.OrderPositions.Sum(op => op.Price * op.Amount),
                IsPaid = o.IsPaid,
                Date = o.Date
            }).ToList();

            return result;
        }

        public IEnumerable<OrderDetailsResponseDTO> GetOrderDetails(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderPositions)
                .ThenInclude(op => op.Product)
                .FirstOrDefault(o => o.ID == orderId);

            if (order == null)
            {
                throw new ArgumentException("Order not found.");
            }

            var result = order.OrderPositions.Select(op => new OrderDetailsResponseDTO
            {
                ProductName = op.Product != null ? op.Product.Name : null,
                Price = op.Price,
                Amount = op.Amount,
                Total = op.Price * op.Amount
            }).ToList();

            return result;
        }
    }
}
