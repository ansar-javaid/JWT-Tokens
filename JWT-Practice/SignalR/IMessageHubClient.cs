namespace JWT_Practice.SignalR
{
    public interface IMessageHubClient
    {
        Task SendOffersToUser(string message);
    }
}
