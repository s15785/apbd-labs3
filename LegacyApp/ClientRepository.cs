using System;
using System.Collections.Generic;
using System.Threading;

namespace LegacyApp
{
    public interface IClientRepository
    {
        public Client GetById(int clientId);
    }

    public class ClientRepository : IClientRepository
    {
        /// <summary>
        /// This collection is used to simulate remote database
        /// </summary>
        private static readonly Dictionary<int, Client> Database = new()
        {
            { 1, new Client("Kowalski", 1, "kowalski@wp.pl", "Warszawa, Złota 12", "NormalClient") },
            { 2, new Client("Malewski", 2, "malewski@gmail.pl", "Warszawa, Koszykowa 86", "VeryImportantClient") },
            { 3, new Client("Smith", 3, "smith@gmail.pl", "Warszawa, Kolorowa 22", "ImportantClient") },
            { 4, new Client("Doe", 4, "doe@gmail.pl", "Warszawa, Koszykowa 32", "ImportantClient") },
            { 5, new Client("Kwiatkowski", 5, "kwiatkowski@wp.pl", "Warszawa, Złota 52", "NormalClient") },
            { 6, new Client("Andrzejewicz", 6, "andrzejewicz@wp.pl", "Warszawa, Koszykowa 52", "NormalClient") }
        };

        /// <summary>
        /// Simulating fetching a client from remote database
        /// </summary>
        /// <returns>Returning client object</returns>
        public Client GetById(int clientId)
        {
            int randomWaitTime = new Random().Next(2000);
            Thread.Sleep(randomWaitTime);

            if (Database.ContainsKey(clientId))
                return Database[clientId];

            throw new ArgumentException($"User with id {clientId} does not exist in database");
        }
    }
}