using UnityEngine;
using System;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Syncano.Data;
using Syncano.Request;
using Newtonsoft.Json;
using Newtonsoft;

namespace Syncano.Client {
	/// <summary>
	/// Client of Syncano using connection over Http.
	/// Provides functionality of sending end geting http request to and from Syncano.
	/// </summary>
	public class SyncanoHttpClient : SelfInstantiatingSingleton<SyncanoHttpClient> {

		/// <summary>
		/// Everything is ok!
		/// </summary>
		public const int HTTP_CODE_SUCCESS = 200;

		/// <summary>
		/// The request was successful and a resource was created.
		/// </summary>
		public const int HTTP_CODE_CREATED = 201;

		///<summary>
		/// The request was successful but there is no data to
		/// return (usually after successful DELETE request).
		///</summary>
		public const int HTTP_CODE_NO_CONTENT = 204;

		///<summary>
		/// There was no new data to return.
		///</summary>
		public const int HTTP_CODE_NOT_MODIFIED = 304;

		///<summary>
		/// The request was invalid or cannot be otherwise served.
		/// An accompanying error message will explain further.
		///</summary>
		public const int HTTP_CODE_BAD_REUEST = 400;

		///<summary>
		/// Authentication credentials were missing or incorrect.
		///</summary>
		public const int HTTP_CODE_UNAUTHORIZED = 401;

		///<summary>
		/// The request is understood, but it has been refused or
		/// access is not allowed. An accompanying error message will explain why.
		///</summary>
		public const int HTTP_CODE_FORBIDDEN = 403;

		///<summary>
		/// The URI requested is invalid or the resource requested, such as a user, does not exists.
		/// Also returned when the requested format is not supported by the requested method.
		///</summary>
		public const int HTTP_CODE_NOT_FOUND = 404;

		///<summary>
		/// Requested method is not supported for this resource.
		///</summary>
		public const int HTTP_CODE_METHOD_NOT_ALLOWED = 405;

		///<summary>
		/// Something is broken. Please contact Syncano support.
		///</summary>
		public const int HTTP_CODE_INTERNAL_SERVER_ERROR = 500;

		///<summary>
		/// Syncano is down. Contact support.
		///</summary>
		public const int HTTP_CODE_BAD_GATEWAY = 502;

		///<summary>
		/// Syncano request timeout.
		///</summary>
		public const int HTTP_CODE_GATEWAY_TIMEOUT = 504;

		///<summary>
		/// Status code when response is ok.
		///</summary>
		public const int CODE_SUCCESS = 0;

		///<summary>
		/// Status code when Http error appeared.
		///</summary>
		public const int CODE_HTTP_ERROR = 1;

		///<summary>
		/// Client Protocol Exception
		///</summary>
		public const int CODE_CLIENT_PROTOCOL_EXCEPTION = 2;

		///<summary>
		/// Illegal State Exception.
		///</summary>
		public const int CODE_ILLEGAL_STATE_EXCEPTION = 3;

		///<summary>
		/// IOException.
		///</summary>
		public const int CODE_ILLEGAL_IO_EXCEPTION = 4;

		///<summary>
		/// Parsing exception.
		///</summary>
		public const int CODE_PARSING_RESPONSE_EXCEPTION = 5;

		///<summary>
		/// Unknown
		///</summary>
		public const int CODE_UNKNOWN_ERROR = 6;

		/// <summary>
		/// This method starts coroutine for SendRequest and returns it, so it cal be yielded or run asynchronously.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="obj">Object.</param>
		/// <param name="onSuccess">On success.</param>
		/// <param name="onFailure">On failure.</param>
		/// <param name="httpMethodOverride">Http method override.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Coroutine PostAsync<T>(T obj, Action<Response<T>> onSuccess, Action<Response<T>> onFailure, string httpMethodOverride = null) where T :SyncanoObject , new() {

			string serializedObject = obj != null ? JsonUtility.ToJson(obj) : string.Empty;
			string id =  (obj != null && obj.id != 0) ? obj.id.ToString() : string.Empty;
			string url = UrlBuilder(id.ToString(), typeof(T));

			return StartCoroutine(SendRequest(new Response<T>(), url, serializedObject, onSuccess, onFailure, httpMethodOverride));
		}

		/// <summary>
		/// This method starts coroutine for SendRequest and returns it, so it cal be yielded or run asynchronously.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="onSuccess">On success.</param>
		/// <param name="onFailure">On failure.</param>
		/// <param name="httpMethodOverride">Http method override.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Coroutine PostAsync<T>(long id, Action<Response<T>> onSuccess, Action<Response<T>> onFailure, string httpMethodOverride = null) where T : SyncanoObject, new() {

			string url = UrlBuilder(id.ToString(), typeof(T));

			return StartCoroutine(SendRequest(new Response<T>(), url, string.Empty, onSuccess, onFailure, httpMethodOverride));
		}

		public Coroutine PostAsync<T>(Dictionary<string, string> postData, Action<Response<T>> onSuccess, Action<Response<T>> onFailure, string httpMethodOverride = null) where T : SyncanoObject, new() {

			string url = UrlBuilder(string.Empty, typeof(T));

			return StartCoroutine(SendRequest(new Response<T>(), url, string.Empty, onSuccess, onFailure, httpMethodOverride));
		}

		public Coroutine GetAsync<T>(string channelName, Dictionary<string, string> getData, Action<Response<T>> onSuccess, Action<Response<T>> onFailure) where T : SyncanoObject, new() {

			string url = UrlBuilder(channelName, typeof(T), getData);

			return StartCoroutine(SendRequest(new Response<T>(), url, string.Empty, onSuccess, onFailure, UnityWebRequest.kHttpVerbGET));
		}

	 	public Coroutine PostAsync<T>(Action<ResponseGetList<T>> onSuccess, Action<ResponseGetList<T>> onFailure = null, string httpMethodOverride = null) where T : SyncanoObject, new() {
			string url = UrlBuilder(null, typeof(T));
			return StartCoroutine(SendRequest<T>(new ResponseGetList<T>(), url, string.Empty, onSuccess, onFailure, httpMethodOverride));
		}

		/// <summary>
		/// Sends the request.
		/// </summary>
		/// <returns>The request.</returns>
		/// <param name="response">Response.</param>
		/// <param name="url">URL.</param>
		/// <param name="serializedObject">Serialized object.</param>
		/// <param name="onSuccess">On success.</param>
		/// <param name="onFailure">On failure.</param>
		/// <param name="httpMethodOverride">Http method override.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private IEnumerator SendRequest<T>(Response<T> response, string url, string serializedObject, Delegate onSuccess, Delegate onFailure, string httpMethodOverride = null)  where T : SyncanoObject, new() {
			
			UnityWebRequest www = PrepareWebRequest(url, serializedObject, httpMethodOverride);

			yield return www.Send();

			ReadWebRequest(response, www);
			ProcessResponse(response, onSuccess, onFailure, www.method, www.downloadHandler.text);
		}

		/// <summary>
		/// After request was received, check and call available callbacks.
		/// </summary>
		/// <param name="response">Response.</param>
		/// <param name="onSuccess">On success.</param>
		/// <param name="onFailure">On failure.</param>
		/// <param name="method">Method.</param>
		/// <param name="json">Json.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private void ProcessResponse<T>(Response<T> response, Delegate onSuccess, Delegate onFailure, string method, string json) where T : SyncanoObject, new()
		{
			if(onFailure != null)
			{
				if(response.IsSuccess == false)
				{
					if(onFailure != null)
					{
						onFailure.DynamicInvoke(response);
					}
				}

				else
				{
					if(onSuccess != null)
					{
						if(method.Equals(UnityWebRequest.kHttpVerbDELETE) == false)
						{
							response.SetData(json);
						}
						onSuccess.DynamicInvoke(response);
					}
				}
			}

			else
			{
				if(method.Equals(UnityWebRequest.kHttpVerbDELETE) == false)
				{
					response.SetData(json);
				}

				if(onSuccess != null)
				{
					onSuccess.DynamicInvoke(response);
				}
			}
		}

		/// <summary>
		/// Calls the script endpoint.
		/// </summary>
		/// <returns>The script endpoint.</returns>
		/// <param name="endpointId">Endpoint identifier.</param>
		/// <param name="scriptName">Script name.</param>
		/// <param name="callback">Callback.</param>
		public Coroutine RunScriptEndpointUrl(string url, System.Action<ScriptEndpoint> callback, Dictionary<string, string> payload = null) {
			return StartCoroutine(RequestScriptEndPoint(url, callback, payload));
		}

		/// <summary>
		/// Method used for calling script endpoints.
		/// </summary>
		/// <returns>The script end point.</returns>
		/// <param name="endpointId">Endpoint identifier.</param>
		/// <param name="scriptName">Script name.</param>
		/// <param name="callback">Callback.</param>
		private IEnumerator RequestScriptEndPoint(string url, System.Action<ScriptEndpoint> callback, Dictionary<string, string> payload = null) {

			if(Uri.IsWellFormedUriString(url, UriKind.Absolute) == false)
			{
				throw new Exception(url + " is not a valid url");
			}

			WWWForm postData = null;

			if(payload != null && payload.Count > 0)
			{
				postData = new WWWForm();
		
				foreach(KeyValuePair<string, string> pair in payload)
				{
					postData.AddField(pair.Key, pair.Value);
				}
			}

			UnityWebRequest www = UnityWebRequest.Post(url, postData);

			yield return www.Send();

			ScriptEndpoint response = JsonConvert.DeserializeObject<ScriptEndpoint>(www.downloadHandler.text, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
			ReadWebRequest(response, www);

			if(response.IsSuccess)
			{
				if(string.IsNullOrEmpty(response.stderr) == false)
				{
					response.IsSuccess = false;
				}
			}	

			callback(response);
		}

		/// <summary>
		/// Prepares the web request to be sent to Syncano.
		/// </summary>
		/// <returns>The web request.</returns>
		/// <param name="url">URL.</param>
		/// <param name="serializedObject">Serialized object.</param>
		/// <param name="httpMethodOverride">Http method override.</param>
		private UnityWebRequest PrepareWebRequest(string url, string serializedObject, string httpMethodOverride = null)
		{
			UnityWebRequest www = new UnityWebRequest(url);
			www.SetRequestHeader(Constants.HTTP_HEADER_API_KEY, SyncanoClient.Instance.ApiKey);
			www.SetRequestHeader("Content-Type", "application/json");
			UTF8Encoding encoding = new UTF8Encoding();

			www.downloadHandler = new DownloadHandlerBuffer();

			www.method = string.IsNullOrEmpty(httpMethodOverride) ? UnityWebRequest.kHttpVerbPOST : httpMethodOverride;

			if(string.IsNullOrEmpty(serializedObject) == false)
			{
				www.uploadHandler = new UploadHandlerRaw(encoding.GetBytes(serializedObject));
			}

			return www;
		}

		/// <summary>
		/// This method reads UnityWebRequest and sets all response codes, errors etc. to a SyncanoWebRequest object.
		/// </summary>
		/// <param name="webRequest">Web request.</param>
		/// <param name="www">Www.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private void ReadWebRequest(SyncanoWebRequest webRequest, UnityWebRequest www) {

			webRequest.responseCode = www.responseCode;
			webRequest.webError = www.error;
			bool isSyncanoError = CheckIfResponseIfSuccessForMethod(www.method, www.responseCode) != true;
			webRequest.IsSyncanoError = isSyncanoError;
			webRequest.IsSuccess = !www.isError && isSyncanoError == false;
			 
			if(isSyncanoError) {
				webRequest.syncanoError = www.downloadHandler.text;
			}
		}

		/// <summary>
		/// URLs the builder for most of Dataobjects method calls.
		/// </summary>
		/// <returns>The builder.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="classType">Class type.</param>
		private string UrlBuilder(string id, Type classType, Dictionary<string, string> getData = null) {

			string ID = string.IsNullOrEmpty(id) == false ? id.ToString() : string.Empty;
			StringBuilder sb = new StringBuilder(Constants.PRODUCTION_SERVER_URL);

			if(classType.ToString().ToLower().Equals("channel"))
			{
				sb.Append(string.Format(Constants.CHANNELS_LIST_URL, SyncanoClient.Instance.InstanceName));
			}

			else if(classType.ToString().ToLower().Equals("notification"))
			{
				sb.Append(string.Format(Constants.CHANNELS_POLL_URL, SyncanoClient.Instance.InstanceName, id));
			}

			else
			{
				sb.Append(string.Format(Constants.OBJECTS_DETAIL_URL, SyncanoClient.Instance.InstanceName, classType.ToString(), ID));
			}

			if(getData != null)
			{
				sb.Append("?");

				foreach(KeyValuePair<string, string> pair in getData)
				{
					if(string.IsNullOrEmpty(pair.Key) == false && string.IsNullOrEmpty(pair.Value) == false)
					{

					}

					sb.Append("&");
				}
			}

			string url = sb.ToString();

			if(url.EndsWith("&"))
			{
				url.Remove(url.Length);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Checks if response if success for Syncano Request. Some requests like DELETE should return true for codes like 404 and 204.
		/// </summary>
		/// <returns><c>true</c>, if if response if success for method was checked, <c>false</c> otherwise.</returns>
		/// <param name="method">Method.</param>
		/// <param name="resultCode">Result code.</param>
		private bool CheckIfResponseIfSuccessForMethod(string method, long resultCode) {

			switch(method)
			{
			case UnityWebRequest.kHttpVerbGET:
				if(resultCode == HTTP_CODE_SUCCESS)
				{
					return true;
				}
				return false;
			case UnityWebRequest.kHttpVerbPOST:
				if(resultCode == HTTP_CODE_SUCCESS || resultCode == HTTP_CODE_NO_CONTENT || resultCode == HTTP_CODE_CREATED)
				{
					return true;
				}
				return false;
			case UnityWebRequest.kHttpVerbDELETE:
				if(resultCode == HTTP_CODE_NOT_FOUND || resultCode == HTTP_CODE_NO_CONTENT)
				{
					return true;
				}
				return false;
			default:
				return false;
			}
		}
	}
}