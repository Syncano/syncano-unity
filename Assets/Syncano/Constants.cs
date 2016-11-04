using UnityEngine;
using System.Collections;

namespace Syncano {
/// <summary>
/// Constants class containing mostly strings for URLs.
/// </summary>
public class Constants {

	/// <summary>
	/// Server URL
	/// </summary>
	public const string PRODUCTION_SERVER_URL = "https://api.syncano.io";

	public const string HTTP_HEADER_API_KEY = "X-API-KEY";

	#region objects
	public const string OBJECTS_DETAIL_URL = "/v1.1/instances/{0}/classes/{1}/objects/{2}/";
	#endregion objects

	#region scritps
	public const string SCRIPT_WITH_ID_URL = "https://api.syncano.io/v1.1/instances/{0}/snippets/scripts/{1}/run/";
	#endregion scripts
	
	#region channel 
	public const string CHANNELS_LIST_URL = "/v1.1/instances/{0}/channels/";
	public const string CHANNELS_POLL_URL = "/v1.1/instances/{0}/channels/{1}/poll/";
	public const string CHANNELS_PUBLISH_URL = "/v1.1/instances/{0}/channels/{1}/publish/";
	#endregion channel

}
}