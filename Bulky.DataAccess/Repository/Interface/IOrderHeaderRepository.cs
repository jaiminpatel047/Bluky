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
        void Update(OrderHeader entity);
        void Save();
    }
}
