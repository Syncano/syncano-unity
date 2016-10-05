using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public enum DataObjectPermissions {
	
	[JsonProperty("none")]
	NONE,

	[JsonProperty("read")]
	READ,

	[JsonProperty("write")]
	WRITE,

	[JsonProperty("full")]
	FULL
}