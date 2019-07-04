namespace MooseSoft.Azure.ServiceBus.BackOffDelayStrategy
{
    /// <summary>
    /// ZeroBackOffDelayStrategy uses <see cref="ConstantBackOffDelayStrategy"/> that is set to constant of zero.
    /// This strategy will always return no delay.
    /// </summary>
    public class ZeroBackOffDelayStrategy : ConstantBackOffDelayStrategy
    {
        /// <summary>
        /// Creates a new instance of the class and sets the constant delay to zero.
        /// </summary>
        public ZeroBackOffDelayStrategy() : base(0)
        {
        }
    }
}