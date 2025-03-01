using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlulkyBook.DataAccess.Repository.Interface
{
    public interface IUnityOfWork
    {
       ICategoryRepository Category { get; }
       IProductRepository Product { get; }
       ICompanyRepository Company { get; }
        void Save();
    }
}
