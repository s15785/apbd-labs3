using System.Diagnostics.CodeAnalysis;

namespace LegacyAppTests;

using LegacyApp;

public class UserServiceTest
{
    private class MockedUserCreditService(int limit, string expectedLastName, DateTime expectedDateOfBirth): IUserCreditService
    {
        public int GetCreditLimit(string lastName, DateTime dateOfBirth)
        {
            Assert.Equal(expectedLastName, lastName);
            Assert.Equal(expectedDateOfBirth, dateOfBirth);
            return limit;
        }

        public void Dispose()
        {
            //noop
        }
    }
    
    private class MockedUserCreditServiceFactory(IUserCreditService service) : IUserCreditServiceFactory
    {
        public IUserCreditService Create()
        {
            return service;
        }
    }

    private class MockedClientRepository(Client client, int expectedId) : IClientRepository
    {
        public Client GetById(int clientId)
        {
            Assert.Equal(expectedId, clientId);
            return client;
        }
    }

    private class NoopUserDataAccessStrategy : IUserDataAccessStrategy
    {
        public void AddUser(User user)
        {
            //noop
        }
    }

    private class FixedTimeProvider(DateTime fixedDateTime) : ITimeProvider
    {
        public DateTime NowDateTime()
        {
            return fixedDateTime;
        }
    }
        
    [Fact]
    public void SanityTest()
    {
        var currentDateTime = DateTime.Parse("2024-04-06");
        var clientId = 1;
        var dateOfBirth = DateTime.Parse("1982-03-21");
        var creditServiceFactory = new MockedUserCreditServiceFactory(new MockedUserCreditService(3000, "Doe", dateOfBirth));
        //for some reason in Program.cs is invoked with John Doe and clientId = 1 which basing on implementation in ClientRepository is id of Kowalski
        //but I remain it to test original behaviour
        var client = new Client("Doe", 4, "doe@gmail.pl", "Warszawa, Koszykowa 32", "ImportantClient");
        var clientRepository = new MockedClientRepository(client, clientId);
        var userDataAccessStrategy = new NoopUserDataAccessStrategy();
        
        var userService = new UserService(creditServiceFactory, clientRepository, userDataAccessStrategy, new FixedTimeProvider(currentDateTime));

        var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", dateOfBirth, clientId);

        // Assert
        Assert.True(addResult);
    }
}