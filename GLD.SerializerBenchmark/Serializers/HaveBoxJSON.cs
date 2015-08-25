///
/// See here https://www.nuget.org/packages/HaveBoxJSON/
/// >PM Install-Package HaveBoxJSON
/// 

using System.IO;
using HaveBoxJSON;

namespace GLD.SerializerBenchmark
{
    internal class HaveBoxJSON : ISerDeser
    {
        private static readonly JsonConverter _serializer = new JsonConverter();

        #region ISerDeser Members

        public string Name {get { return "HaveBoxJSON"; } }

        public string Serialize<T>(object person)
        {
            return _serializer.Serialize((T) person);
        }

        public T Deserialize<T>(string serialized)
        {
            return _serializer.Deserialize<T>(serialized);
        }

        public void Serialize<T>(object person, Stream outputStream)
        {
            throw new System.NotImplementedException();
        }

   
        public T Deserialize<T>(Stream inputStream)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}