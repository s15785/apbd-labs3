
namespace LegacyApp
{
    public interface IUserDataAccessStrategy
    {
        public void AddUser(User user);
    }
    
    public class LegacyLibBasedUserDataAccessStrategy : IUserDataAccessStrategy
    {
        public void AddUser(User user)
        {
            UserDataAccess.AddUser(user);
        }
    }

}

