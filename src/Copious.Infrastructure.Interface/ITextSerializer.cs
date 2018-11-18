using System.IO;

namespace Copious.Infrastructure.Interface {
    /// <summary>
    /// Interface for serializers that can read/write an object graph to a stream.
    /// </summary>
    public interface ITextSerializer {
        /// <summary>
        /// Serializes an object graph to a text reader.
        /// </summary>
        void Serialize (TextWriter writer, object graph);

        string Serialize (object graph);

        /// <summary>
        /// Deserializes an object graph from the specified text reader.
        /// </summary>
        object Deserialize (TextReader reader);

        T Deserialize<T> (TextReader reader);

        object Deserialize (string json);

        T Deserialize<T> (string json);
    }
}