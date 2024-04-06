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
            //todo: extract validate method
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return false;
            }

            if (!email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            var now = timeProvider.NowDateTime();
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            // todo: extract ClientLimitDeterminer
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var userCreditService = userCreditServiceFactory.Create())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = userCreditServiceFactory.Create())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            userDataAccessStrategy.AddUser(user);
            return true;
        }
    }
}