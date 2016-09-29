using UnityEngine;
using System.Collections;
using Syncano;
using Syncano.Enum;

[System.Serializable]
public class Channel : SyncanoObject {

	/// <summary>
	/// Channel name.
	/// </summary>
	public string name;

	/// <summary>
	/// Channel description.
	/// </summary>
	public string description;

	/// <summary>
	/// Date of channel creation.
	/// </summary>
	public string created_at;

	/// <summary>
	/// Date of last channel update.
	/// </summary>
	public string updated_at;

	/// <summary>
	/// The custon publish flag.
	/// </summary>
	public bool custom_publish;

	public Channel() { }

	public Channel (string channelName) { name = channelName; }
}
