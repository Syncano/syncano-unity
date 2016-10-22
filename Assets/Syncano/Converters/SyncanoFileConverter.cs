using UnityEngine;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json;


public class SyncanoFileConverter : JsonConverter {
	
	public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
	{
		SyncanoFile syncanoFile = (SyncanoFile) value;









		/*
		object[] attributes =  typeof(SyncanoFile).GetCustomAttributes(typeof( JsonProperty));

		for (int i = 0; i < properties.Length; i++)
		{
			if(properties[i].
		}
		*/


	}

	public override object ReadJson (JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
	{
		SyncanoFile syncanoFile = new SyncanoFile();




		return syncanoFile;
	}

	public override bool CanConvert (System.Type objectType)
	{
		return objectType == typeof(SyncanoFile);
	}

}
