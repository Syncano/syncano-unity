using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using System;
using Syncano.Data;
using Newtonsoft.Json;
using Syncano.Enum;

namespace Syncano {
/// <summary>
/// Class representing basic data structure for all DataObjects returned from Syncano. Every class must override it, otherwise it won't deserialize properly.
/// </summary>
public class SyncanoObject {
	/// <summary>
	/// The identifier of this object from Syncano.
	/// </summary>
	[JsonProperty("id")]
	public long Id;

	/// <summary>
	/// The channel.
	/// </summary>
	[JsonProperty("channel", NullValueHandling = NullValueHandling.Ignore)]
	public string Channel;

	/// <summary>
	/// Channel room.
	/// </summary>
	[JsonProperty("channelroom", NullValueHandling = NullValueHandling.Ignore)]
	public string ChannelRoom { get; set; }

	/// <summary>
	/// Gets or sets the owner permissions.
	/// </summary>
	/// <value>The owner permissions.</value>
	[JsonProperty("ownerPermissions", NullValueHandling = NullValueHandling.Ignore)]
	public DataObjectPermissions? OwnerPermissions { get; set; }

	/// <summary>
	/// Gets or sets the group permissions.
	/// </summary>
	/// <value>The group permissions.</value>
	[JsonProperty("groupPermissions", NullValueHandling = NullValueHandling.Ignore)]
	public DataObjectPermissions? GroupPermissions { get; set; }

	/// <summary>
	/// Gets or sets the other permissions.
	/// </summary>
	/// <value>The other permissions.</value>
	[JsonProperty("otherPermissions", NullValueHandling = NullValueHandling.Ignore)]
	public DataObjectPermissions? OtherPermissions { get; set; }

	/// <summary>
	/// Gets or sets the group.
	/// </summary>
	/// <value>The group.</value>
	[JsonProperty("group", NullValueHandling = NullValueHandling.Ignore)]
	public int? Group { get; set; }

	/// <summary>
	/// Empty constructor is required when deserializing Syncano objects.
	/// </summary>
	public SyncanoObject() { }
}
}