using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Syncano;
using Syncano.Data;
using Newtonsoft.Json;

namespace Syncano.Data {
	public class ResponseGetList<T> : Response<T>  where T : SyncanoObject, new() {

		[JsonProperty("prev")]
		public string Prev { get; set; }

		[JsonProperty("next")]
		public string Next { get; set; }

		[JsonProperty("objects")]
		public List<T> Objects { get; set; }

		public ResponseGetList() { }

		public override void SetData (string json)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
			Newtonsoft.Json.JsonConvert.PopulateObject(json, this, settings);
		}
	}
}