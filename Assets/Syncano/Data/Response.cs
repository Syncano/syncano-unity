using UnityEngine;
using System.Collections;
using Syncano.Request;
using Newtonsoft.Json;

namespace Syncano.Data {
	
	/// <summary>
	/// Wrapper class for getting response from Syncano.
	/// </summary>
	public class Response<T> : SyncanoWebRequest where T : SyncanoObject, new() {
	
	/// <summary>
	/// Deserialized data.
	/// </summary>
	public T Data { set; get; }

		public virtual void SetData(string json)
		{
			Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
		}
}
}