using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Syncano;
using Syncano.Data;
using System;

public class ChannelConnection {

	/// <summary>
	/// This method is raised when the channel some message.
	/// </summary>
	private Action<Response<Notification>> onNotification;
	public Action<Response<Notification>> OnNotification {
		get {
			return onNotification;
		}
		set {
			onNotification = value;
		}
	}

	/// <summary>
	/// This method is raised when the channel returned an error.
	/// </summary>
	private Action<Response<Notification>> onError;
	public Action<Response<Notification>> OnError {
		get {
			return onError;
		}
		set {
			onError = value;
		}
	}

	/// <summary>
	/// In normal case, when channel connection returns an error it stops the connection.
	/// When set this to true, it will try to reconnect after 5 second. It may result in
	/// messaging about an error every 5 seconds when for example internet connection is lost.
	/// </summary>
	private bool autoReonnect;
	public bool AutoConnect {
		get {
			return autoReonnect;
		}
		set {
			autoReonnect = value;
		}
	}

	/// <summary>
	/// Flag which indicates if channel is opened or not.
	/// </summary>
	private bool isRunning;

	/// <summary>
	/// The received error flag.
	/// </summary>
	private bool receivedError;

	/// <summary>
	/// The opened channel by this object.
	/// </summary>
	private IEnumerator pollRequestLoop;

	/// <summary>
	/// The context the channel with use to start polling.
	/// </summary>
	private MonoBehaviour context;

	private ChannelConnection() { }

	public ChannelConnection(MonoBehaviour context, Action<Response<Notification>> onNotification, Action<Response<Notification>> onError) 
	{ 
		OnNotification = onNotification; 
		OnError = onError;
		this.context = context;

		if(context == null)
		{
			throw new System.ArgumentException("Context cannot be null!");
		}
	}

	/// <summary>
	/// Start polling messages from a channel;
	/// </summary>
	/// <param name="context">Context.</param>
	/// <param name="channel">Channel.</param>
	public Coroutine Start(string channel) 
	{
		isRunning = true;

		if(pollRequestLoop != null)
		{
			context.StopCoroutine(pollRequestLoop);
		}

		pollRequestLoop = PollRequestLoop(channel, context);

		return context.StartCoroutine(pollRequestLoop);
	}

	/// <summary>
	/// Stop polling messages and set channel to null.
	/// </summary>
	/// <param name="context">Context.</param>
	public void Stop()
	{
		if(pollRequestLoop != null)
		{
			context.StopCoroutine(pollRequestLoop);
			pollRequestLoop = null;
		}
	}

	/// <summary>
	/// This method is invoked only when channel successfully returned response.
	/// </summary>
	/// <param name="response">Response.</param>
	private void HandleSuccess(Response<Notification> response)
	{
		if(response.Data != null)
		{
			//TODO
		}

		if (onNotification != null)
		{
			onNotification(response);
		}
	}

	/// <summary>
	/// This method is invoked only when channel returned an error. Please take a note that it will not handle error if there was a connection timeout. 
	/// Instead it will connect to Syncano again (long polling).
	/// </summary>
	/// <param name="response">Response.</param>
	private void HandleError(Response<Notification> response)
	{
		//long polling timeout, just continue the loop.
		if(response.responseCode == Syncano.Client.SyncanoHttpClient.HTTP_CODE_NO_CONTENT || response.responseCode == Syncano.Client.SyncanoHttpClient.HTTP_CODE_GATEWAY_TIMEOUT)
		{
			return;
		}

		else 
		{
			receivedError = true;

			if(onError != null)
			{
				onError(response);
			}
		}
	}

	/// <summary>
	/// Polls the request in an infinite loop.
	/// </summary>
	/// <returns>The request loop enumerator.</returns>
	/// <param name="channelName">Channel name.</param>
	/// <param name="context">Context.</param>
	private IEnumerator PollRequestLoop(string channelName, MonoBehaviour context)
	{
		string channelUrl = string.Format(Constants.PRODUCTION_SERVER_URL + Constants.CHANNELS_POLL_URL, SyncanoClient.Instance.InstanceName, channelName);

		while(isRunning)
		{
			if(!isRunning)
			{
				break;
			}

			yield return SyncanoClient.Instance.Please().Get<Notification>(channelUrl, null, HandleSuccess, HandleError);

			if(receivedError)
			{
				receivedError = false;

				if(autoReonnect)
				{
					yield return new WaitForSeconds(5);
				}

				else
				{
					break;
				}
			}
		}
	}
}