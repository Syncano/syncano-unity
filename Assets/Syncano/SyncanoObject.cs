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

	public const string FIELD_ID = "id";
	public const string FIELD_CHANNEL = "channel";
	public const string FIELD_CHANNEL_ROOM = "channel_room";
	public const string FIELD_OWNER_PERMISSIONS = "owner_permissions";
	public const string FIELD_GROUP_PERMISSIONS = "group_permissions";
	public const string FIELD_OTHER_PERMISSIONS = "other_permissions";
	public const string FIELD_GROUP = "group";

	/// <summary>
	/// The identifier of this object from Syncano.
	/// </summary>
	[JsonProperty(FIELD_ID)]
	public long Id;

	/// <summary>
	/// The channel.
	/// </summary>
	[JsonProperty(FIELD_CHANNEL, NullValueHandling = NullValueHandling.Ignore)]
	public string Channel;

	/// <summary>
	/// Channel room.
	/// </summary>
	[JsonProperty(FIELD_CHANNEL_ROOM, NullValueHandling = NullValueHandling.Ignore)]
	public string ChannelRoom { get; set; }

	/// <summary>
	/// Gets or sets the owner permissions.
	/// </summary>
	/// <value>The owner permissions.</value>
	[JsonProperty(FIELD_OWNER_PERMISSIONS, NullValueHandling = NullValueHandling.Ignore)]
	public DataObjectPermissions? OwnerPermissions { get; set; }

	/// <summary>
	/// Gets or sets the group permissions.
	/// </summary>
	/// <value>The group permissions.</value>
	[JsonProperty(FIELD_GROUP_PERMISSIONS, NullValueHandling = NullValueHandling.Ignore)]
	public DataObjectPermissions? GroupPermissions { get; set; }

	/// <summary>
	/// Gets or sets the other permissions.
	/// </summary>
	/// <value>The other permissions.</value>
	[JsonProperty(FIELD_OTHER_PERMISSIONS, NullValueHandling = NullValueHandling.Ignore)]
	public DataObjectPermissions? OtherPermissions { get; set; }

	/// <summary>
	/// Gets or sets the group.
	/// </summary>
	/// <value>The group.</value>
	[JsonProperty(FIELD_GROUP, NullValueHandling = NullValueHandling.Ignore)]
	public int? Group { get; set; }

	/// <summary>
	/// Empty constructor is required when deserializing Syncano objects.
	/// </summary>
	public SyncanoObject() { }
}
}