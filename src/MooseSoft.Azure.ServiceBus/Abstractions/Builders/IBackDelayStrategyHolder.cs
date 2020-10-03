using Moosesoft.Azure.ServiceBus.BackOffDelayStrategy;
using System;

namespace Moosesoft.Azure.ServiceBus.Abstractions.Builders
{
    /// <summary>
    /// Provides a holding mechanism for instances of <see cref="IBackOffDelayStrategy"/> to help constructing message pumps.
    /// </summary>
    public interface IBackDelayStrategyHolder<out TBuilder>
        where TBuilder : IBuilder
    {
        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <typeparam name="TStrategy">Type of <see cref="IBackOffDelayStrategy"/></typeparam>
        /// <param name="backOffDelayStrategy"><see cref="IBackOffDelayStrategy"/> to use for the use for the message pump builder.</param>
        /// <returns>A message pump builder.</returns>
        TBuilder WithBackOffDelayStrategy<TStrategy>(TStrategy backOffDelayStrategy)
            where TStrategy : IBackOffDelayStrategy;

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <returns>A message pump builder.</returns>
        TBuilder WithBackOffDelayStrategy<TStrategy>()
            where TStrategy : IBackOffDelayStrategy, new();

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="ExponentialBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <returns>A message pump builder.</returns>
        TBuilder WithExponentialBackOffDelayStrategy();

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="ExponentialBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <param name="maxDelay">Maximum amount of time this strategy will return.</param>
        /// <returns>A message pump builder.</returns>
        TBuilder WithExponentialBackOffDelayStrategy(TimeSpan maxDelay);

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="ConstantBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <returns>A message pump builder.</returns>
        TBuilder WithConstantBackOffDelayStrategy();

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="ConstantBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <param name="delayTime">Fixed amount of time which will be returned.</param>
        /// <returns>A message pump builder.</returns>
        TBuilder WithConstantBackOffDelayStrategy(TimeSpan delayTime);

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="LinearBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <returns>A message pump builder.</returns>
        TBuilder WithLinearBackOffDelayStrategy();

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="LinearBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <param name="delayTime">Amount of time to use with attempt multiplier.</param>
        /// <returns>A message pump builder.</returns>
        TBuilder WithLinearBackOffDelayStrategy(TimeSpan delayTime);

        /// <summary>
        /// Sets the instance of <see cref="IBackOffDelayStrategy"/> to <see cref="ZeroBackOffDelayStrategy"/> for the message pump builder to use.
        /// </summary>
        /// <returns>A message pump builder.</returns>
        TBuilder WithZeroBackOffDelayStrategy();
    }
}