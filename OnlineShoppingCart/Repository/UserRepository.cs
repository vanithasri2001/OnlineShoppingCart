using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingCart.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly ShoppingDbContext shoppingDb;
        public UserRepository(ShoppingDbContext shoppingDb)
        {
            this.shoppingDb = shoppingDb;
        }
        public async Task<UserModel> AddAsync(UserModel user)
        {
            await shoppingDb.AddAsync(user);
            await shoppingDb.SaveChangesAsync();
            return user;
        }
        public async Task<UserModel> DeleteAsync(int id)
        {
            var user = await shoppingDb.UserTable.FirstOrDefaultAsync(x => x.UserId == id);
            if (user == null)
            {
                return null;
            }
            shoppingDb.UserTable.Remove(user);
            await shoppingDb.SaveChangesAsync();
            return user;
        }
        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            var users = await shoppingDb.UserTable.ToListAsync();
            return users;
        }
        public async Task<UserModel> GetAsync(int id)
        {
            return await shoppingDb.UserTable.FirstOrDefaultAsync(x => x.UserId == id);
        }
        public async Task<UserModel> UpdateAsync(int id, UserModel user)
        {
            var update = await shoppingDb.UserTable.FirstOrDefaultAsync(x => x.UserId == id);
            if (update == null)
            {
                return null;
            }
            update.FirstName = user.FirstName;
            update.LastName = user.LastName;
            update.EmailID = user.EmailID;
            update.PhoneNo = user.PhoneNo;
            update.Gender = user.Gender;
            update.City = user.City;
            update.Password = user.Password;
            update.ConfirmPassword = user.ConfirmPassword;

            await shoppingDb.SaveChangesAsync();
            return update;
        }
        public async Task<UserModel> LoginModel(LoginModel loginmodel)
        {
            
            
                var users = await shoppingDb.UserTable.FirstOrDefaultAsync(x => x.EmailID == loginmodel.EmailID && x.Password == loginmodel.Password);
            if (users == null) { return null; }
            return users;
        }
            



    }
}

    

