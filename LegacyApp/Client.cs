namespace LegacyApp
{
    public class Client(string name, int clientId, string email, string address, string type)
    {
        public string Name { get; } = name;
        public int ClientId { get; } = clientId;
        public string Email { get; } = email;
        public string Address { get; } = address;
        public string Type { get; } = type;
    }
}