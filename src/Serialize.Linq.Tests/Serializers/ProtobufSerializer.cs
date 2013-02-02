using System;
using System.IO;
using System.Runtime.Serialization;
using ProtoBuf.Meta;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Serializers
{
	public class ProtobufSerializer : DataSerializer, IBinarySerializer
	{
		private readonly RuntimeTypeModel Model;

		public ProtobufSerializer()
		{
			Model = RuntimeTypeModel.Create();
			Model.InferTagFromNameDefault = true;
		}

		protected override XmlObjectSerializer CreateXmlObjectSerializer(Type type)
		{
			return null;
		}

		public byte[] Serialize<T>(T obj)
		{
			if (obj == null)
				return new byte[0];
			using (var ms = new MemoryStream())
			{
				Model.Serialize(ms, obj);
				return ms.ToArray();
			}
		}

		public T Deserialize<T>(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)
				return default(T);
			using (var ms = new MemoryStream(bytes))
				return (T)Model.Deserialize(ms, null, typeof(T));
		}
	}
}
