using UserService.Identity;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int user_id);
        void CreateUser(User user);
        void DeleteUser(int user_id);
        void UpdateUser(User user);
        User GetUserByAuth0Id(string authzId);

        void Save();
    }
}
