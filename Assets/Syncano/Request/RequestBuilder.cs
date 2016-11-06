using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Syncano.Client;
using Syncano.Data;

namespace Syncano.Request {
	
	/// <summary>
	/// This class is a gate for making call to syncano between client and the library.
	/// </summary>
	public class RequestBuilder {

		/// <summary>
		/// Sends a Get request for an object with specified id. Takes two callbacks, one when request is successful which is mandatory and second on failure which is optional for error handling.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="onSuccess">On success.</param>
		/// <param name="onFailure">On failure.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Coroutine Get<T>(long id, Action<Response<T>> onSuccess, Action<Response<T>> onFailure) where T : SyncanoObject, new() {
			CheckCallbacks<T>(onSuccess, onFailure);
			return SyncanoHttpClient.Instance.PostAsync<T>(id, onSuccess, onFailure, UnityEngine.Networking.UnityWebRequest.kHttpVerbGET);
		}

		public Coroutine Get<T>(long id, Action<Response<T>> onResponseReturned) where T : SyncanoObject, new() {
			return SyncanoHttpClient.Instance.PostAsync<T>(id, onResponseReturned, null, UnityEngine.Networking.UnityWebRequest.kHttpVerbGET);
		}

		/// <summary>
		/// Get the specified id, onSuccess and onFailure.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="onSuccess">On success.</param>
		/// <param name="onFailure">On failure.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Coroutine Get<T>(Action<ResponseGetList<T>> onSuccess, Action<ResponseGetList<T>> onFailure) where T : SyncanoObject, new() {
			CheckCallbacks<T>(onSuccess, onFailure);
			return SyncanoHttpClient.Instance.PostAsync<T>(onSuccess, onFailure, UnityEngine.Networking.UnityWebRequest.kHttpVerbGET);
		}

		public Coroutine Get<T>(Action<ResponseGetList<T>> onResponseReturned) where T : SyncanoObject, new() {
			return SyncanoHttpClient.Instance.PostAsync<T>(onResponseReturned, null, UnityEngine.Networking.UnityWebRequest.kHttpVerbGET);
		}

		/// <summary>
		/// Get the specified getData, onSuccess and onFailure.
		/// </summary>
		/// <param name="getData">Get data.</param>
		/// <param name="onSuccess">On success.</param>
		/// <param name="onFailure">On failure.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Coroutine Get<T>(string channelUrl, Dictionary<string, string> getData, Action<Response<T>> onSuccess, Action<Response<T>> onFailure) where T : SyncanoObject, new() {
			CheckCallbacks<T>(onSuccess, onFailure);
			return SyncanoHttpClient.Instance.GetAsync<T>(channelUrl, getData, onSuccess, onFailure);
		}

		public Coroutine Get<T>(string channelUrl, Dictionary<string, string> getData, Action<Response<T>> onResponseReturned) where T : SyncanoObject, new() {
			return SyncanoHttpClient.Instance.GetAsync<T>(channelUrl, getData, onResponseReturned, null);
		}

		/// <summary>
		/// Post the specified url, obj, onSuccess and onFailure.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="obj">Object.</param>
		/// <param name="onSuccess">On success.</param>
		/// <param name="onFailure">On failure.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Coroutine Post<T>(string url, T obj, Action<Response<T>> onSuccess, Action<Response<T>> onFailure)  where T : SyncanoObject, new() {
			return SyncanoHttpClient.Instance.PostAsync(obj, onSuccess, onFailure, url:url);
		}

		/// <summary>
		/// Creates the channel.
		/// </summary>
		/// <returns>The channel.</returns>
		/// <param name="channel">Channel.</param>
		/// <param name="onSuccess">On success.</param>
		/// <param name="onFailure">On failure.</param>
		public Coroutine CreateChannel(Channel channel, Action<Response<Channel>> onSuccess, Action<Response<Channel>> onFailure) {
			CheckCallbacks<Channel>(onSuccess, onFailure);
			string url = Constants.PRODUCTION_SERVER_URL + string.Format(Constants.CHANNELS_LIST_URL, SyncanoClient.Instance.InstanceName);
			return SyncanoHttpClient.Instance.PostAsync<Channel>(channel, onSuccess, onFailure, UnityEngine.Networking.UnityWebRequest.kHttpVerbPOST, url:url);
		}

		public Coroutine CreateChannel(Channel channel,  Action<Response<Channel>> onResponseReturned) {
			return SyncanoHttpClient.Instance.PostAsync<Channel>(channel, onResponseReturned, null, UnityEngine.Networking.UnityWebRequest.kHttpVerbPOST);
		}

		/// <summary>
		/// Save the specified obj, onSuccess and onFailure. Sends a Get request for an object with specified id. Takes two callbacks, one when request is successful which is mandatory and second on failure which is optional for error handling.
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <param name="onSuccess">On success.</param>
		/// <param name="onFailure">On failure.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Coroutine Save<T>(T obj, Action<Response<T>> onSuccess, Action<Response<T>> onFailure) where T : SyncanoObject, new() {
			CheckCallbacks<T>(onSuccess, onFailure);
			return SyncanoHttpClient.Instance.PostAsync<T>(obj, onSuccess, onFailure);
		}

		public Coroutine Save<T>(T obj,  Action<Response<T>> onResponseReturned) where T : SyncanoObject, new() {
			return SyncanoHttpClient.Instance.PostAsync<T>(obj, onResponseReturned, null);
		}

		public Coroutine Register<T>(string url, User<T> obj, Action<Response<User<T>>> onSuccess, Action<Response<User<T>>> onFailure, string httpMethodOverride) where T : Profile, new() {
			return SyncanoHttpClient.Instance.CreateOrUpdateUser<T>(url, obj, onSuccess, onFailure, httpMethodOverride);
		}

		/// <summary>
		/// Deletes the specified obj, onSuccess and onFailure. Sends a Get request for an object with specified id. Takes two callbacks, one when request is successful which is mandatory and second on failure which is optional for error handling.
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <param name="onSuccess">On success.</param>
		/// <param name="onFailure">On failure.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Coroutine Delete<T>(T obj, Action<Response<T>> onSuccess, Action<Response<T>> onFailure) where T : SyncanoObject, new() {
			CheckCallbacks<T>(onSuccess, onFailure);
			return SyncanoHttpClient.Instance.PostAsync<T>(obj, onSuccess, onFailure, UnityEngine.Networking.UnityWebRequest.kHttpVerbDELETE);
		}

		public Coroutine Delete<T>(T obj, Action<Response<T>> onResponseReturned) where T : SyncanoObject, new() {
			return SyncanoHttpClient.Instance.PostAsync<T>(obj, onResponseReturned, null, UnityEngine.Networking.UnityWebRequest.kHttpVerbDELETE);
		}

		/// <summary>
		/// Runs the script endpoint URL.
		/// </summary>
		/// <returns>The script endpoint URL.</returns>
		/// <param name="url">URL.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="payload">Payload.</param>
		public Coroutine RunScriptEndpointUrl(string url, Action<ScriptEndpoint> callback, Dictionary<string, string> payload = null) {
			return SyncanoHttpClient.Instance.RunScriptEndpointUrl(url, callback, payload);
		}

		/// <summary>
		/// Runs the script endpoint URL with a given Id. To use this method you must provide an account key to your Syncano instance.
		/// </summary>
		/// <returns>The script endpoint URL.</returns>
		/// <param name="scriptId">Script identifier.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="payload">Payload.</param>
		public Coroutine RunScriptEndpointUrl(int scriptId, Action<ScriptEndpoint> callback, Dictionary<string, string> payload = null) {
			string url = string.Format(Constants.SCRIPT_WITH_ID_URL, SyncanoClient.Instance.InstanceName, scriptId);
			return RunScriptEndpointUrl(url, callback, payload);
		}

		private void CheckCallbacks<T>(Delegate onSuccess, Delegate onFailure) where T : SyncanoObject, new()
		{
			if(onSuccess == null || onFailure == null)
			{
				throw new Exception("Callbacks cannot be null." + "onSuccess == null" + (onSuccess == null).ToString() + " onFailure == " + (onFailure == null).ToString());
			}
		}
	}
}