
namespace Ordering.Application.Exceptions
{
    public class OrderNotFountException : ApplicationException
    {
        public OrderNotFountException(string Name, Object key):base($"Entity {Name} - {key} is not found.")
        {
            
        }
    }
}
