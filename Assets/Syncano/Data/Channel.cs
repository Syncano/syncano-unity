﻿using UnityEngine;
using System.Collections;
using Syncano;
using Syncano.Enum;
using Newtonsoft.Json;

namespace Syncano.Data {
	public class Channel : SyncanoObject {

		public const string FIELD_NAME = "name";
		public const string FIELD_DESCRIPTION = "description";
		public const string FIELD_TYPE = "type";
		public const string FIELD_GROUP = "group";
		public const string FIELD_GROUP_PERMISSIONS = "group_permissions";
		public const string FIELD_OTHER_PERMISSIONS = "other_permissions";
		public const string FIELD_CREATED_AT = "created_at";
		public const string FIELD_UPDATED_AT = "updated_at";
		public const string FIELD_CUSTOM_PUBLISH = "custom_publish";

		/// <summary>
		/// Channel name.
		/// </summary>
		[JsonProperty(FIELD_NAME)]
		public string Name { get; set; }

		/// <summary>
		/// Channel description.
		/// </summary>
		[JsonProperty(FIELD_DESCRIPTION)]
		public string Description;

		/// <summary>
		/// Gets or sets the type of the channel.
		/// </summary>
		/// <value>The type of the channel.</value>
		[JsonProperty(FIELD_TYPE)]
		public ChannelType ChannelType { get; set; }

		/// <summary>
		/// Gets or sets the group.
		/// </summary>
		/// <value>The group.</value>
		[JsonProperty(FIELD_GROUP)]
		public Group Group { get; set; }

		/// <summary>
		/// Gets or sets the channel permissions.
		/// </summary>
		/// <value>The channel permissions.</value>
		[JsonProperty(FIELD_GROUP_PERMISSIONS)]
		public ChannelPermissions GroupPermissions { get; set; }

		/// <summary>
		/// Gets or sets the other permissions.
		/// </summary>
		/// <value>The other permissions.</value>
		[JsonProperty(FIELD_OTHER_PERMISSIONS)]
		public ChannelPermissions OtherPermissions { get; set; }

		/// <summary>
		/// Date of channel creation.
		/// </summary>
		[JsonProperty(FIELD_CREATED_AT)]
		public string CreatedAt;

		/// <summary>
		/// Date of last channel update.
		/// </summary>
		[JsonProperty(FIELD_UPDATED_AT)]
		public string UpdatedAt;

		/// <summary>
		/// The custon publish flag.
		/// </summary>
		[JsonProperty(FIELD_CUSTOM_PUBLISH)]
		public bool CustomPublish;

		public Channel() { }

		public Channel (string channelName) { Name = channelName; }
	}
}