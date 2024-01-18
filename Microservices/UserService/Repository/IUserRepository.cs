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
        IEnumerable<User> GetAllUsersForCountry(string country);
        IEnumerable<User> GetAllUsersForCity(string city);
        User GetUserByCountry(string country);
        User GetUserByCity(string city);

        void Save();
    }
}
