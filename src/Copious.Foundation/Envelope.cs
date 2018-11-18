using System;

namespace Copious.Foundation {
    /// <summary>
    /// Static factory class for <see cref="Envelope{T}"/>.
    /// </summary>
    public abstract class Envelope {
        protected Envelope () { }

        /// <summary>
        /// Creates an envelope for the given body.
        /// </summary>
        /// <param name="body"></param>
        public static Envelope<T> Create<T> (T body) => new Envelope<T> (body);
    }

    /// <summary>
    /// Provides the envelope for an object that will be sent to a bus.
    /// </summary>
    public class Envelope<T> : Envelope {
        /// <summary>
        /// Initializes a new instance of the <see cref="Envelope{T}"/> class.
        /// </summary>
        public Envelope (T body) {
            Body = body;
        }

        /// <summary>
        /// Gets the body.
        /// </summary>
        public T Body { get; private set; }

        /// <summary>
        /// Gets or sets the delay for sending, enqueing or processing the body.
        /// </summary>
        public TimeSpan Delay { get; set; }

        /// <summary>
        /// Gets or sets the time to live for the message in the queue.
        /// </summary>
        public TimeSpan TimeToLive { get; set; }

        /// <summary>
        /// Gets the correlation id.
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets the correlation id.
        /// </summary>
        public string MessageId { get; set; }

        public static implicit operator Envelope<T> (T body) => Envelope.Create (body);
    }
}