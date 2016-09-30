using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using Syncano.Request;
using Syncano.Data;

namespace Syncano {
	/// <summary>
	/// Create Syncano object. When using this constructor most functions will not work, because api key and instance name are usually required.
	/// </summary>
	public class SyncanoClient : SelfInstantiatingSingleton<SyncanoClient> {
	/// <summary>
	/// The name of the instance.
	/// </summary>
	public string InstanceName { get; private set; }

	/// <summary>
	/// The API key.
	/// </summary>
	public string ApiKey { get; private set; }

	/// <summary>
	/// This flag checks if Syncano client was initialized.
	/// </summary>
	private bool isInitialized;

	/// <summary>
	/// This method must be called before making any call to Syncano.
	/// </summary>
	/// <param name="apiKey">API key.</param>
	/// <param name="instanceName">Instance name.</param>
	public SyncanoClient Init(string apiKey, string instanceName)
	{
		isInitialized = true;
		InstanceName = instanceName;
		ApiKey = apiKey;

		return this;
	}
	
	/// <summary>
	/// Returns builder object that makes it easy to configure a request in one line.
	/// </summary>
	public RequestBuilder Please()
	{	
		if(isInitialized == false)
		{
			Debug.LogError("Syncano has not been initialized. Please call Init before making any requests.");
			return null;
		}
		return new RequestBuilder();
	}
}
}