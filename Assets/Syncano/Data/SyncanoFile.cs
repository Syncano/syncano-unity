using UnityEngine;
using System.Collections;
using System.IO;
using Newtonsoft.Json;

namespace Syncano.Data {
	public class SyncanoFile {	

		public byte[] Data { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		public SyncanoFile() { }

		public SyncanoFile(byte[] data) 
		{
			Data = data;
		}
	}
}