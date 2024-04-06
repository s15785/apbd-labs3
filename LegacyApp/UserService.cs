using System;

namespace LegacyApp
{
    public class UserService(
        IUserCreditServiceFactory userCreditServiceFactory,
        IClientRepository clientRepository,
        IUserDataAccessStrategy userDataAccessStrategy,
        ITimeProvider timeProvider)
    {
        public UserService() : this(
            new DefaultUserCreditServiceFactory(),
            new ClientRepository(),
            new LegacyLibBasedUserDataAccessStrategy(),
            new DefaultTimeProvider()
        )
        {
        }
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            try
            {
                var user = CreateUser(firstName, lastName, email, dateOfBirth, clientId);
                userDataAccessStrategy.AddUser(user);
                return true;
            }
            catch (UserValidationException)
            {
                return false;
            }
        }

        private User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            var client = clientRepository.GetById(clientId);
            var limit = DetermineLimit(lastName, dateOfBirth, client);
            return new User(firstName, lastName, email, dateOfBirth, client, limit, timeProvider);
        }

        private int? DetermineLimit(string lastName, DateTime dateOfBirth, Client client)
        {
            using var userCreditService = userCreditServiceFactory.Create();
            return LimitDeterminer.Determine(userCreditService, client, lastName, dateOfBirth);
        }
    }
}