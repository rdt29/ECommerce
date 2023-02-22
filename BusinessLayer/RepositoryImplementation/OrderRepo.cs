using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RepositoryImplementation
{
    public class OrderRepo : IOrders
    {
        private readonly EcDbContext _db;

        public OrderRepo(EcDbContext db)
        {
            _db = db;
        }

        public async Task<OrdersTable> order(OrderDTO obj, int userid)
        {
            try
            {
                var totalPrice = await _db.Products.Where(x => obj.ProductID.Contains(x.Id)).SumAsync(x => x.price);

                List<OrderDetails> orderDetails = new List<OrderDetails>();
                foreach (int i in obj.ProductID)
                {
                    OrderDetails orderdetail = new OrderDetails();
                    orderdetail.ProductId = i;
                    orderdetail.CreatedAt = DateTime.Now;
                    orderdetail.CreatedBy = userid;
                    orderDetails.Add(orderdetail);
                };

                OrdersTable table = new OrdersTable()
                {
                    TotalPrice = totalPrice,
                    CustomerID = userid,
                    CreatedAt = DateTime.Now,
                    CreatedBy = userid,
                    OrderDetails = orderDetails
                };
                await _db.OrderTable.AddAsync(table);
                await _db.SaveChangesAsync();

                return (table);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserOrderViewDTO>> ViewOrders(int id)
        {
            try
            {
                var username = await _db.Users.Where(x => x.ID == id).Select(x => x.Name).FirstOrDefaultAsync();

                var user = await _db.OrderTable.Where(x => x.CustomerID == id).Include(X => X.OrderDetails)
                    .ThenInclude(x => x.Product).Select(x => new UserOrderViewDTO()
                    {
                        UserName = username,
                        TotalPrice = x.TotalPrice,
                        UserId = id,
                        Productdet = x.OrderDetails.Select(x => new UserOrderProductsViewDTO()
                        {
                            OrderID = x.OrderId,
                            ProductName = x.Product.ProductName,
                            price = x.Product.price,
                            ProductDesc = x.Product.ProductDescription,
                        }).ToList()
                    }).ToListAsync();

                return (user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Products>> ViewSuppilerOrders(int id)
        {
            try
            {
                //var totalPrice = await _db.Products.Where(x => obj.ProductID.Contains(x.Id)).SumAsync(x => x.price);

                var OrderProductId = await _db.OrderDetails.ToListAsync();
                var ProductAdded = await _db.Products.Where(x => x.UserId == id).ToListAsync();

                var Final = ProductAdded.Where(y => OrderProductId.Any(x => x.ProductId == y.Id)).ToList();

                return Final;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}