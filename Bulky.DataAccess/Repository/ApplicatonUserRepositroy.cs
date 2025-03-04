using BlulkyBook.DataAccess.Repository.Interface;
using BlulkyBook.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlulkyBook.DataAccess.Repository
{
    public class ApplicatonUserRepositroy : Repository<ApplicationUser>, IApplicatonUserRepositroy
    {
        private readonly ApplicationDbContext _db;
        public ApplicatonUserRepositroy(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser user)
        {
            if (user != null)
            {
                _db.ApplicationUser.Update(user);
            }
        }
    }
}
