using MooseSoft.Azure.ServiceBus.FailurePolicy;
using System;

namespace MooseSoft.Azure.ServiceBus.Abstractions
{
    /// <summary>
    /// Provides a holding mechanism for instances of <see cref="IFailurePolicy"/> to help constructing message pumps.
    /// </summary>
    public interface IFailurePolicyHolder
    {
        /// <summary>
        /// Sets the instance of <see cref="IFailurePolicy"/> to <see cref="CloneMessageFailurePolicy"/> for the message pump builder to use.
        /// </summary>
        /// <param name="canHandle">Function providing <see cref="bool"/> for which exceptions should be handled by the <see cref="IFailurePolicy"/>.</param>
        /// <returns><see cref="IBackDelayStrategyHolder"/></returns>
        IBackDelayStrategyHolder WithCloneMessageFailurePolicy(Func<Exception, bool> canHandle = null);

        /// <summary>
        /// Sets the instance of <see cref="IFailurePolicy"/> to <see cref="DeferMessageFailurePolicy"/> for the message pump builder to use.
        /// </summary>
        /// <param name="canHandle">Function providing <see cref="bool"/> for which exceptions should be handled by the <see cref="IFailurePolicy"/>.</param>
        /// <returns><see cref="IBackDelayStrategyHolder"/></returns>
        IBackDelayStrategyHolder WithDeferMessageFailurePolicy(Func<Exception, bool> canHandle = null);

        /// <summary>
        /// Sets the instance of <see cref="IFailurePolicy"/> to <see cref="AbandonMessageFailurePolicy"/> for the message pump builder to use.
        /// </summary>
        /// <returns>A message pump builder</returns>
        IMessagePumpBuilder WithAbandonMessageFailurePolicy();

        /// <summary>
        /// Sets the instance of <see cref="IFailurePolicy"/> for the message pump builder to use.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IFailurePolicy"/></typeparam>
        /// <param name="failurePolicy"><see cref="IFailurePolicy"/> to use for the use for the message pump builder.</param>
        /// <returns>A message pump builder</returns>
        IMessagePumpBuilder WithFailurePolicy<T>(T failurePolicy)
            where T : IFailurePolicy;
    }
}