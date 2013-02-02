﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Serialize.Linq.Interfaces;

namespace Serialize.Linq.Serializers
{
	public abstract class TextSerializer : DataSerializer, ITextSerializer
	{
		public virtual string Serialize<T>(T obj)
		{
			try
			{
				using (var ms = new MemoryStream())
				{
					this.Serialize(ms, obj);

					ms.Position = 0;
					using (var reader = new StreamReader(ms, Encoding.UTF8))
						return reader.ReadToEnd();
				}
			}
			catch (Exception ex)
			{
				throw new SerializationException("Error converting type: " + ex.Message, ex);
			}
		}

		public virtual T Deserialize<T>(string text)
		{
			using (var ms = new MemoryStream())
			{
				using (var writer = new StreamWriter(ms))
				{
					writer.Write(text);
					writer.Flush();

					ms.Position = 0;
					return this.Deserialize<T>(ms);
				}
			}
		}
	}
}