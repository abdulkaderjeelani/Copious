using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Copious.Infrastructure.Interface;
using Newtonsoft.Json;

namespace Copious.Infrastructure {
    public class JsonTextSerializer : ITextSerializer {
        readonly JsonSerializer _serializer;

        public JsonTextSerializer () {
            _serializer = JsonSerializer.Create (new JsonSerializerSettings {
                // Allows deserializing to the actual runtime type
                TypeNameHandling = TypeNameHandling.All,
                    // In a version resilient way
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            });
        }

        public void Serialize (TextWriter writer, object graph) {
            try {
                var jsonWriter = new JsonTextWriter (writer) {
#if DEBUG
                    Formatting = Formatting.Indented
                };
#endif

                _serializer.Serialize (jsonWriter, graph);

                // We don't close the stream as it's owned by the message.
                writer.Flush ();
            } catch (JsonSerializationException e) {
                // Wrap in a standard .NET exception.
                throw new SerializationException (e.Message, e);
            }
        }

        public string Serialize (object graph) {
            var serializedTextBuilder = new StringBuilder ();

            using (var serializationWriter = new StringWriter (serializedTextBuilder)) {
                Serialize (serializationWriter, graph);
                return serializedTextBuilder.ToString ();
            }
        }

        public object Deserialize (TextReader reader) {
            try {
                using (var jsonReader = new JsonTextReader (reader))
                return _serializer.Deserialize (jsonReader);
            } catch (JsonSerializationException e) {
                // Wrap in a standard .NET exception.
                throw new SerializationException (e.Message, e);
            }
        }

        public T Deserialize<T> (TextReader reader) => (T) Deserialize (reader);

        public object Deserialize (string json) {
            using (var jsonStringReader = new StringReader (json)) {
                return Deserialize (jsonStringReader);
            }
        }

        public T Deserialize<T> (string json) => (T) Deserialize (json);
    }
}