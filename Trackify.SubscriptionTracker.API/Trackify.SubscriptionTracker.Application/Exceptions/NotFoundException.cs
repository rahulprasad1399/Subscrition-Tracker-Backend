namespace Trackify.SubscriptionTracker.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string name,int key)
            : base($"{name} with ID '{key}' was not found.")
        {
        }
    }
}
