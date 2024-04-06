using System;
using System.Collections.Generic;
using System.Threading;

namespace LegacyApp
{
    public interface IUserCreditService : IDisposable
    {
        public int GetCreditLimit(string lastName, DateTime dateOfBirth);
    }
    
    public class UserCreditService : IUserCreditService
    {
        /// <summary>
        /// Simulating database
        /// </summary>
        private readonly Dictionary<string, int> _database =
            new Dictionary<string, int>()
            {
                {"Kowalski", 200},
                {"Malewski", 20000},
                {"Smith", 10000},
                {"Doe", 3000},
                {"Kwiatkowski", 1000}
            };
        
        public void Dispose()
        {
            //Simulating disposing of resources
        }

        /// <summary>
        /// This method is simulating contact with remote service which is used to get info about someone's credit limit
        /// </summary>
        /// <returns>Client's credit limit</returns>
        public int GetCreditLimit(string lastName, DateTime dateOfBirth)
        {
            int randomWaitingTime = new Random().Next(3000);
            Thread.Sleep(randomWaitingTime);

            if (_database.ContainsKey(lastName))
                return _database[lastName];

            throw new ArgumentException($"Client {lastName} does not exist");
        }
    }
    
    // because UserCreditService is disposable, and we still want to make graceful close in UserCreditService
    // we use factory instead of holding UserCreditService directly in UserService
    public interface IUserCreditServiceFactory
    {
        public IUserCreditService Create();
    }

    internal class DefaultUserCreditServiceFactory : IUserCreditServiceFactory 
    {
        public IUserCreditService Create()
        {
            return new UserCreditService();
        }
    }
}

