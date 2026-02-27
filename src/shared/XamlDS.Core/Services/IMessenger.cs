namespace XamlDS.Services;

/// <summary>
/// Provides a messaging service for loosely coupled communication between components.
/// </summary>
public interface IMessenger
{
    /// <summary>
    /// Sends a message to all registered subscribers.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to send.</typeparam>
    /// <param name="message">The message instance to send.</param>
    void Send<TMessage>(TMessage message);

    /// <summary>
    /// Subscribes to messages of a specific type.
    /// </summary>
    /// <typeparam name="TMessage">The type of message to subscribe to.</typeparam>
    /// <param name="handler">The handler to invoke when a message is received.</param>
    /// <returns>A subscription token that can be used to unsubscribe.</returns>
    IDisposable Subscribe<TMessage>(Action<TMessage> handler);
}
