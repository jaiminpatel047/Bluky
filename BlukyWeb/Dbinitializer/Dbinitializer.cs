using BlulkyBook.DataAccess;
using BlulkyBook.Models;
using BlulkyBook.Utility;
using BlulkyBook.Web.Dbinitializer.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;

namespace BlulkyBook.Web.Dbinitializer
{
    public class Dbinitializer : IDbinitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Dbinitializer(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (!_roleManager.RoleExistsAsync(SD.User_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.User_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.User_Employe)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.User_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.User_Company)).GetAwaiter().GetResult();


                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "pateljaimin047@gmail.com",
                    Email = "pateljaimin047@gmail.com",
                    Name = "Jaimin Patel",
                    PhoneNumber = "1234567890",
                    StreetAddres = "Chandkheda",
                    City = "Ahmedabad",
                    State = "Gujarat",
                    PostalCode = "382424",
                }, "patelJAimin047@gmail.com").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUser.FirstOrDefault(x => x.Email == "pateljaimin047@gmail.com");
                _userManager.AddToRoleAsync(user, SD.User_Admin).GetAwaiter().GetResult();
            }

        }
    }
}
