using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.data
{

    public class UserRepository : BaseRepository<User>, IUserRepository
    {

        private readonly GestionGastosContext _context;
        public UserRepository(GestionGastosContext context) : base(context)
        {
            _context = context;
        }
        public User? GetByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            return user;
        }

        public User? GetByPhoneNumber(string phoneNumber)
        {
            var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            return user;
        }

    }
}
