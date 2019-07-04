using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MooseSoft.Azure.ServiceBus.Tests")]

namespace MooseSoft.Azure.ServiceBus
{
    internal class Constants
    {
        public const string DeferredKey = "DeferredSequenceNumber";
        public const string RetryCountKey = "RetryCount";
    }
}