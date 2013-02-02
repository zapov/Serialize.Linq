using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Serialize.Linq.Interfaces;
using Newtonsoft.Json;

namespace Serialize.Linq.Serializers
{
    public class NewtonsoftJsonSerializer : TextSerializer, IJsonSerializer
    {
        protected override XmlObjectSerializer CreateXmlObjectSerializer(Type type)
        {
			return null;
        }

		public override string Serialize<T>(T value)
		{
			var declaredType = typeof(T);
			var type = value != null ? value.GetType() : declaredType;
			var settings = new JsonSerializerSettings();
			settings.TypeNameHandling = type != declaredType || !(declaredType.IsClass || declaredType.IsValueType) ? TypeNameHandling.Objects : TypeNameHandling.Auto;
			return JsonConvert.SerializeObject(value, settings);
		}

		public override T Deserialize<T>(string text)
		{
			var settings = new JsonSerializerSettings();
			settings.TypeNameHandling = TypeNameHandling.Auto;
			return JsonConvert.DeserializeObject<T>(text, settings);
		}
    }
}