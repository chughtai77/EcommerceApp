using ecommerceapp.Data;
using ecommerceapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ecommerceapp.ViewModels.Registration
{
    public class RegistrationViewModel
    {
        private ApplicationDbContext _db;

        public RegistrationViewModel(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public Register GetRegisterByEmail(string email)
        {
            //return _db.Registers.Find(email);
            return _db.Registers.FirstOrDefault(e => e.Email == email);
        }
        public Register GetFindByPhoneno(string phono)
        {
            //return _db.Registers.Find(phono);
            return _db.Registers.FirstOrDefault(e => e.PhoneNumber == phono);
        }

        public async Task AddUserAsync(IdentityUser user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task AddRegisterAsync(Register obj)
        {
            _db.Registers.Add(obj);
            await _db.SaveChangesAsync();
        }
        
        public async Task<Register> GetUserByEmailAsync(string email)
        {
            return await _db.Registers.FirstOrDefaultAsync(r => r.Email.Equals(email));
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
