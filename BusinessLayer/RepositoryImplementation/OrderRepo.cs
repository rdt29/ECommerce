using BusinessLayer.Repository;
using DataAcessLayer.DBContext;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using System.Security.Cryptography;

namespace BusinessLayer.RepositoryImplementation
{
    public class OrderRepo : IOrders
    {
        private readonly EcDbContext _db;

        public OrderRepo(EcDbContext db)
        {
            _db = db;
        }

        public async Task<byte[]> Invoice(int Orderid)
        {
            try
            {
                var order = await _db.OrderTable.Where(x => x.ID == Orderid).Include(x => x.OrderDetails).ThenInclude(x => x.Product).ToListAsync();
                var UserId = order.Select(x => x.CustomerID).FirstOrDefault();
                var UserName = await _db.Users.Where(x => x.ID == UserId).Select(x => x.Name).FirstOrDefaultAsync();
                string html = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Order Detail</title><style type='text/css'>body {	font-family: Arial, sans-serif;		font-size: 14px;			margin: 0;			padding: 0;		}" +
                    ".greeting { color: red; text-align: center;}" +
                    "		h1 {		font-size: 24px;			font-weight: bold;			margin-top: 20px;		margin-bottom: 20px;			text-align: center;		}		" +
                    "table {border-collapse: collapse;" +
                    "margin: 0 auto;width: 100%;}" +
                    "th, td " +
                    "{border: 1px solid #ccc; " +
                    "padding: 10px;		" +
                    "text-align: center;}" +
                    "th {background-color: #f0f0f0;	font-weight: bold;}" +
                    "#left{font-weight: bold;float: left;}" +
                    "</style>" +
                    "</head>" +
                    "<body>" +
                    "<h2  id='left'>RDT-Ecommerce</h2>" +
                    "<h1>Order Detail</h1>" +
                    "<br>" +
                    "<h3>Invoice</h3>" +
                    "<hr>" +
                    "<h2><b>Billed To,</h2>" +
                    "<h4>" + UserName + "</h4>" +

                    "<table>		" +
                    "<thead>" +
                    "<tr>" +
                    "<th>Product Name</th>" +
                    "<th>Quantity</th>" +
                    "<th>Price</th>" +
                    "<th>Total</th>" +
                    "</tr>" +
                    "</thead>" +
                    "<tbody>"
                  ;
                var TotalPrice = 0;
                foreach (var i in order)
                {
                    TotalPrice = i.TotalPrice;
                    foreach (var j in i.OrderDetails)
                    {
                        html += "<tr>" +
                        "<td>" + j.Product.ProductName + "</td>" +
                        "<td>1</td>" +
                        "<td>" + j.Product.price + "</td>" +
                        "<td>" + j.Product.price + "</td>" +
                        "</tr>";
                    }
                }
                html +=

                   "<tr>" +
                    "<td colspan='3' style='text-align:right'>Total:</td>" +
                    "<td>" + TotalPrice + "</td>" +
                    "</tr>" +
                    "</tbody>" +
                    "</table>" +
                   "<h3 class = 'greeting'>Thankyou for choosing us </h3> " +
                    "</body>" +
                    "</html>";

                var document = new PdfDocument();
                string Content = html;
                PdfGenerator.AddPdfPages(document, Content, PageSize.A4);
                byte[] response = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    document.Save(ms);
                    response = ms.ToArray();
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
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
                        OrderID = x.ID,

                        Productdet = x.OrderDetails.Select(x => new UserOrderProductsViewDTO()
                        {
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