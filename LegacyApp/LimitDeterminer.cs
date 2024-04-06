using System;

namespace LegacyApp;

public class LimitDeterminer()
{
    public static int? Determine(IUserCreditService userCreditService, Client client, string lastName, DateTime dateOfBirth)
    {
        switch (client.Type)
        {
            case "VeryImportantClient":
                return null;
            case "ImportantClient":
            {
                int creditLimit = userCreditService.GetCreditLimit(lastName, dateOfBirth);
                creditLimit *= 2;
                return creditLimit;
            }
            default:
            {
                int creditLimit = userCreditService.GetCreditLimit(lastName, dateOfBirth);
                return creditLimit;
            }
        }
    }
}