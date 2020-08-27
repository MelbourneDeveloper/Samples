# if !NET45
using System;

namespace DelegateLogging
{
    public class DelegateLoggerScope : IDisposable
    {
        protected Action<LogMessage>? Action { get; private set; }
        protected DelegateLogger? DelegateLogger { get; private set; }

        public DelegateLoggerScope(
        Action<LogMessage> action,
        DelegateLogger delegateLogger
        )
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
            DelegateLogger = delegateLogger ?? throw new ArgumentNullException(nameof(delegateLogger));
        }

        public virtual void Dispose()
        {
            Action = null;
            DelegateLogger = null;
        }

    }

    public sealed class DelegateLoggerScope<TState> : DelegateLoggerScope, IDisposable
    {

        public DelegateLoggerScope(
            Action<LogMessage> action,
            DelegateLogger delegateLogger
            ) : base(action, delegateLogger)
        {
            if (delegateLogger == null) throw new ArgumentNullException(nameof(delegateLogger));
        }

        public override void Dispose() =>
            //_DelegateLogger.ReleaseScope<TState>();
            base.Dispose();
    }
}
#endif