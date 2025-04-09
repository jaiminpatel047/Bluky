using BlulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlulkyBook.DataAccess.Repository.Interface
{
    public interface IOrderHeaderRepository : IRepositroy<OrderHeader>
    {
        void UpdateOrderStatus(int Id, string status, string? paymentStatus = null);
        void Update(OrderHeader entity);
        void Save();
    }
}
