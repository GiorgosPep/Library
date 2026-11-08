using LibraryAppModelFirst.Models;

namespace LibraryAppModelFirst.Repositories
{
    public interface IUserRepository
    {
        Task<User?> FindByUserName(string userName);
    }
}
