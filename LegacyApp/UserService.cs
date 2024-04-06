using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IUserCreditServiceFactory _userCreditServiceFactory;
        private readonly IClientRepository _clientRepository;
        
        public UserService()
        {
            _userCreditServiceFactory = new DefaultUserCreditServiceFactory();
            _clientRepository = new ClientRepository();
        }
        
        internal UserService(IUserCreditServiceFactory userCreditServiceFactory, IClientRepository clientRepository)
        {
            _userCreditServiceFactory = userCreditServiceFactory;
            _clientRepository = clientRepository;
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

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            var client = _clientRepository.GetById(clientId);

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
                using (var userCreditService = _userCreditServiceFactory.Create())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = _userCreditServiceFactory.Create())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            //todo: inject some different interface for testing
            UserDataAccess.AddUser(user);
            return true;
        }
    }
}
