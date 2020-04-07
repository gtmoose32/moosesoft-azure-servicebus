using MooseSoft.Azure.ServiceBus.BackOffDelayStrategy;
using System;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    /// <summary>
    /// Provides a holding mechanism for instances of <see cref="IBackOffDelayStrategy"/> to help constructing message pumps.
    /// </summary>
    public interface IBackDelayStrategyHolder
    {
        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IBackOffDelayStrategy"/></typeparam>
        /// <param name="backOffDelayStrategy"><see cref="IBackOffDelayStrategy"/> to use for the use for the message pump builder.</param>
        /// <returns>A message pump builder.</returns>
        IMessagePumpBuilder WithBackOffDelayStrategy<T>(T backOffDelayStrategy)
            where T : IBackOffDelayStrategy;

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <returns>A message pump builder.</returns>
        IMessagePumpBuilder WithBackOffDelayStrategy<T>()
            where T : IBackOffDelayStrategy, new();

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="ExponentialBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <returns>A message pump builder.</returns>
        IMessagePumpBuilder WithExponentialBackOffDelayStrategy();

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="ExponentialBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <param name="maxDelay">Maximum amount of time this strategy will return.</param>
        /// <returns>A message pump builder.</returns>
        IMessagePumpBuilder WithExponentialBackOffDelayStrategy(TimeSpan maxDelay);

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="ConstantBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <returns>A message pump builder.</returns>
        IMessagePumpBuilder WithConstantBackOffDelayStrategy();

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="ConstantBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <param name="delayTime">Fixed amount of time which will be returned.</param>
        /// <returns>A message pump builder.</returns>
        IMessagePumpBuilder WithConstantBackOffDelayStrategy(TimeSpan delayTime);

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="LinearBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <returns>A message pump builder.</returns>
        IMessagePumpBuilder WithLinearBackOffDelayStrategy();

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="LinearBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <param name="delayTime">Amount of time to use with attempt multiplier.</param>
        /// <returns>A message pump builder.</returns>
        IMessagePumpBuilder WithLinearBackOffDelayStrategy(TimeSpan delayTime);

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="ZeroBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <returns>A message pump builder.</returns>
        IMessagePumpBuilder WithZeroBackOffDelayStrategy();
    }
}