using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using Syncano.Request;
using Syncano.Data;
using System;

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
	/// Gets or sets the user key.
	/// </summary>
	/// <value>The user key.</value>
	public string UserKey { get; set; }

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
	
	/// <summary>
	/// Registers the user.
	/// </summary>
	/// <returns>The user.</returns>
	/// <param name="user">User.</param>
	/// <param name="onSuccess">On success.</param>
	/// <param name="onFailure">On failure.</param>
	public Coroutine RegisterUser<T>(string url, User<T> user, Action<Response<User<T>>> onSuccess, Action<Response<User<T>>> onFailure, string httpMethodOverride)  where T : Profile, new()
	{
			return Please().Register(url, user, onSuccess, onFailure, httpMethodOverride);
	}
	
	/// <summary>
	/// Registers the user.
	/// </summary>
	/// <returns>The user.</returns>
	/// <param name="user">User.</param>
	/// <param name="callback">Callback.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public Coroutine RegisterUser<T>(string url, User<T> user, Action<Response<User<T>>> callback, string httpMethodOverride) where T : Profile, new()
	{
		return Please().Register(url, user, callback, null, httpMethodOverride);
	}
	
	/// <summary>
	/// Publish message on the channel.
	/// </summary>
	/// <returns>The on channel.</returns>
	/// <param name="channelName">Channel name.</param>
	/// <param name="notification">Notification.</param>
	public Coroutine PublishOnChannel(string channelName, Notification notification) 
	{
		string url = string.Format(Constants.PRODUCTION_SERVER_URL + Constants.CHANNELS_PUBLISH_URL, SyncanoClient.Instance.InstanceName, channelName);
		return Please().Post(url, notification, null, null);
	}
}
}