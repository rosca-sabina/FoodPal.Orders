namespace FoodPal.Orders.BackgroundServices.Handlers.Contracts
{
    public interface IMessageHandlerFactory
    {
        IMessageHandler GetHandler(string messageType);
    }
}
