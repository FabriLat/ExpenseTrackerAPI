using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User? GetByEmail(string email);

        User? GetByPhoneNumber(string phoneNumber);
    }
}
