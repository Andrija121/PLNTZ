using UserService.Identity;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        //User GetUserById(int user_id);
        User GetUserByAuth0Id(string authzId);
        void CreateUser(User user);
        //void DeleteUser(int user_id);
        void DeleteUserByAuth0Id(string authzId);
        void UpdateUser(User user);

        void Save();
    }
}
