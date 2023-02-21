using DataAcessLayer.DTO;
using DataAcessLayer.Entity;

namespace BusinessLayer.Repository
{
    public interface IOrders
    {
        public Task<OrdersTable> order(OrderDTO obj, int userid);

        public Task<List<UserOrderViewDTO>> ViewOrders(int id);
    }
}