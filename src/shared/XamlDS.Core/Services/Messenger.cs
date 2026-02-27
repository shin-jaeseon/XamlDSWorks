namespace XamlDS.Services;

public sealed class Messenger : IMessenger
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();
    private readonly object _lock = new();

    public void Send<TMessage>(TMessage message)
    {
        List<Delegate>? handlers;

        lock (_lock)
        {
            var messageType = typeof(TMessage);
            if (!_subscribers.TryGetValue(messageType, out handlers))
                return;

            // Copy to avoid modification during iteration
            handlers = handlers.ToList();
        }

        foreach (var handler in handlers.Cast<Action<TMessage>>())
        {
            try
            {
                handler(message);
            }
            catch (Exception ex)
            {
                // Log error but continue processing other handlers
                Console.Error.WriteLine($"Error in message handler: {ex.Message}");
            }
        }
    }

    public IDisposable Subscribe<TMessage>(Action<TMessage> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        lock (_lock)
        {
            var messageType = typeof(TMessage);
            if (!_subscribers.ContainsKey(messageType))
            {
                _subscribers[messageType] = new List<Delegate>();
            }
            _subscribers[messageType].Add(handler);
        }

        return new Subscription<TMessage>(this, handler);
    }

    private void Unsubscribe<TMessage>(Action<TMessage> handler)
    {
        lock (_lock)
        {
            var messageType = typeof(TMessage);
            if (_subscribers.TryGetValue(messageType, out var handlers))
            {
                handlers.Remove(handler);
                if (handlers.Count == 0)
                {
                    _subscribers.Remove(messageType);
                }
            }
        }
    }

    private sealed class Subscription<TMessage> : IDisposable
    {
        private readonly Messenger _messenger;
        private readonly Action<TMessage> _handler;
        private bool _disposed;

        public Subscription(Messenger messenger, Action<TMessage> handler)
        {
            _messenger = messenger;
            _handler = handler;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _messenger.Unsubscribe(_handler);
                _disposed = true;
            }
        }
    }
}
