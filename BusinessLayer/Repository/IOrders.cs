using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Repository
{
    public interface IOrders
    {
        public Task<OrdersTable> order(OrderDTO obj, int userid);

        public Task<List<UserOrderViewDTO>> ViewOrders(int id);

        public Task<List<Products>> ViewSuppilerOrders(int id);

        public Task<string> SendMail(int orderId, string ToMail);

        public Task<byte[]> Invoice(int id);
    }
}