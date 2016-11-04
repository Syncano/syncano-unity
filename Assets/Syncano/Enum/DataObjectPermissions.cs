using Newtonsoft.Json;

namespace Syncano.Enum
{
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
}