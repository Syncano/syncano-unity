using UnityEngine;
using System.Collections;
using Syncano;
using System.Xml;
using Newtonsoft.Json;

public class Notification : SyncanoObject {

	/// <summary>
	/// The payload.
	/// </summary>
	[JsonProperty("payload")]
	public Payload Result { get; set; }

	/// <summary>
	/// The admin.
	/// </summary>
	[JsonProperty("admin")]
	public Author Admin;

	/// <summary>
	/// The action.
	/// </summary>
	[JsonProperty("metadata")]
	public MetaData Action;

	public Notification() { }


	public struct Payload
	{
		[JsonProperty("content")]
		public string Content;
	}

	public struct Author
	{
		[JsonProperty("admin")]
		public int Admin { get; set; }
	}

	public struct MetaData
	{
		[JsonProperty("type")]
		public string Type { get; set; }
	}
}