using System;

namespace LegacyApp
{
    public class User
    {
        public readonly Client Client;
        public readonly DateTime DateOfBirth;
        public readonly string EmailAddress;
        public readonly string FirstName;
        public readonly string LastName;
        public readonly int? CreditLimit;
        public bool HasCreditLimit => CreditLimit != null;

        public User(string firstName, string lastName, string emailAddress, DateTime dateOfBirth, Client client,
            int? creditLimit, ITimeProvider timeProvider)
        {
            CreditLimit = creditLimit;
            EmailAddress = emailAddress;
            DateOfBirth = dateOfBirth;
            Client = client;
            LastName = lastName;
            FirstName = firstName;

            if (string.IsNullOrEmpty(firstName))
            {
                throw new UserValidationException("Invalid first name");
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new UserValidationException("Invalid last name");
            }

            if (!EmailAddress.Contains('@') && !EmailAddress.Contains('.'))
            {
                throw new UserValidationException("Invalid email");
            }

            if (Age(timeProvider) < 21)
            {
                throw new UserValidationException("User with age below 21");
            }

            if (HasCreditLimit && CreditLimit < 500)
            {
                throw new UserValidationException("User with credit limit below 500");
            }
        }

        public int Age(ITimeProvider timeProvider)
        {
            var now = timeProvider.NowDateTime();
            int age = now.Year - DateOfBirth.Year;
            if (now.Month < DateOfBirth.Month || (now.Month == DateOfBirth.Month && now.Day < DateOfBirth.Day)) age--;
            return age;
        }
    }

    public class UserValidationException(string msg) : Exception(msg)
    {
    }

}