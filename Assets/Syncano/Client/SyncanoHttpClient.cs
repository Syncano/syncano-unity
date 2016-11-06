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
		public Coroutine PostAsync<T>(T obj, Action<Response<T>> onSuccess, Action<Response<T>> onFailure, string httpMethodOverride = null, string url = null, Dictionary<string, string> payload = null, Action<string> onAfterSuccess = null) where T : SyncanoObject , new() {

			string serializedObject = obj != null ? JsonConvert.SerializeObject(obj, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore}) : string.Empty;
			string id =  (obj != null && obj.Id != 0) ? obj.Id.ToString() : string.Empty;
			url = UrlBuilder(id.ToString(), typeof(T), payload, url);

			return StartCoroutine(SendRequest(new Response<T>(), url, serializedObject, onSuccess, onFailure, httpMethodOverride, onAfterSuccess:onAfterSuccess));
		}

		public Coroutine CreateOrUpdateUser<T>(string url, User<T> obj, Action<Response<User<T>>> onSuccess, Action<Response<User<T>>> onFailure, string httpMethodOverride) where T : Profile , new() {

			WWWForm wwwForm = new WWWForm();

			PropertyInfo[] properties = obj.GetType().GetProperties();

			if(properties != null && properties.Length > 0)
			{
				for (int i = 0; i < properties.Length; i++) {

					PropertyInfo property = properties[i];

					object value = property.GetValue(obj, null);

					if(value != null && value is Profile == false)
					{
						JsonPropertyAttribute[] propertyAttritbues = property.GetCustomAttributes(typeof(JsonPropertyAttribute), true) as JsonPropertyAttribute[];
						string propertyName = property.Name;

						if(propertyAttritbues != null && propertyAttritbues.Length > 0)
						{
							JsonPropertyAttribute attribute = propertyAttritbues[0];
							if(string.IsNullOrEmpty(attribute.PropertyName) == false)
							{
								propertyName = attribute.PropertyName;
							}
						}
					
						wwwForm.AddField(propertyName, value.ToString());
					}
				}
			}

			if(obj.Profile != null)
			{
				properties = obj.Profile.GetType().GetProperties();

				if(properties != null && properties.Length > 0) {
					for (int i = 0; i < properties.Length ; i++) {

						PropertyInfo property = properties[i];

						object value = property.GetValue(obj.Profile, null);

						if(value != null)
						{
							JsonPropertyAttribute[] propertyAttritbues = property.GetCustomAttributes(typeof(JsonPropertyAttribute), true) as JsonPropertyAttribute[];
							string propertyName = property.Name;

							if(propertyAttritbues != null && propertyAttritbues.Length > 0)
							{
								JsonPropertyAttribute attribute = propertyAttritbues[0];
								if(string.IsNullOrEmpty(attribute.PropertyName) == false)
								{
									propertyName = attribute.PropertyName;
								}
							}

							if(value is SyncanoFile)
							{
								SyncanoFile syncanoFile = value as SyncanoFile;

								if(syncanoFile.Data != null)
								{
									wwwForm.AddBinaryData("profile." + propertyName, syncanoFile.Data);	
								}
							}

							else
							{
								wwwForm.AddField("profile." + propertyName, value.ToString());
							}
						}
					}
				}
			}
		
			return StartCoroutine(SendRequest(obj, new Response<User<T>>(), url, wwwForm, onSuccess, onFailure, httpMethodOverride));
		}

		private IEnumerator SendRequest<T>(User<T> obj, Response<User<T>> response, string url, WWWForm form, Delegate onSuccess, Delegate onFailure, string httpMethodOverride)  where T : Profile, new() {

			UnityWebRequest www =  UnityWebRequest.Post(url, form);
			www.method = httpMethodOverride;
			www.SetRequestHeader(Constants.HTTP_HEADER_API_KEY, SyncanoClient.Instance.ApiKey);

			if(string.IsNullOrEmpty(SyncanoClient.Instance.UserKey) == false)
			{
				www.SetRequestHeader("X-USER-KEY", SyncanoClient.Instance.UserKey);
			}

			yield return www.Send();

			ReadWebRequest(response, www);

			if(response.IsSuccess)
			{
				JsonConvert.PopulateObject(www.downloadHandler.text, obj);
			}

			ProcessResponse(response, onSuccess, onFailure, www.method, www.downloadHandler.text);
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
		public Coroutine PostAsync<T>(long id, Action<Response<T>> onSuccess, Action<Response<T>> onFailure, string httpMethodOverride = null, string url = null) where T : SyncanoObject, new() {

			url = UrlBuilder(id.ToString(), typeof(T), url: url);

			return StartCoroutine(SendRequest(new Response<T>(), url, string.Empty, onSuccess, onFailure, httpMethodOverride));
		}

		public Coroutine PostAsync<T>(Dictionary<string, string> payload, Action<Response<T>> onSuccess, Action<Response<T>> onFailure, string httpMethodOverride = null, string url = null) where T : SyncanoObject, new() {

			url = UrlBuilder(string.Empty, typeof(T), null, url);

			return StartCoroutine(SendRequest(new Response<T>(), url, string.Empty, onSuccess, onFailure, httpMethodOverride, payload));
		}

		public Coroutine GetAsync<T>(string channelUrl, Dictionary<string, string> getData, Action<Response<T>> onSuccess, Action<Response<T>> onFailure) where T : SyncanoObject, new() {

			channelUrl = UrlBuilder(channelUrl, typeof(T), getData, url: channelUrl);

			return StartCoroutine(SendRequest(new Response<T>(), channelUrl, string.Empty, onSuccess, onFailure, UnityWebRequest.kHttpVerbGET));
		}

		public Coroutine PostAsync<T>(Action<ResponseGetList<T>> onSuccess, Action<ResponseGetList<T>> onFailure = null, string httpMethodOverride = null, string url = null) where T : SyncanoObject, new() {
			url = UrlBuilder(null, typeof(T), url: url);
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
		private IEnumerator SendRequest<T>(Response<T> response, string url, string serializedObject, Delegate onSuccess, Delegate onFailure, string httpMethodOverride = null, Dictionary<string, string> payload = null, Delegate onAfterSuccess = null)  where T : SyncanoObject, new() {

			UnityWebRequest www = PrepareWebRequest(url, serializedObject, payload, httpMethodOverride);

			yield return www.Send();

			ReadWebRequest(response, www);
			ProcessResponse(response, onSuccess, onFailure, www.method, www.downloadHandler.text, onAfterSuccess);
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
		private void ProcessResponse<T>(Response<T> response, Delegate onSuccess, Delegate onFailure, string method, string json, Delegate onAfterSuccess = null) where T : SyncanoObject, new()
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

						if(onAfterSuccess != null)
						{
							onAfterSuccess.DynamicInvoke(json);
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
			UnityWebRequest www = null;

			if(payload != null && payload.Count > 0)
			{
				postData = new WWWForm();

				foreach(KeyValuePair<string, string> pair in payload)
				{
					postData.AddField(pair.Key, pair.Value);
				}
				www = UnityWebRequest.Post(url, postData);
			}

			else
			{
				www = UnityWebRequest.Get(url);
			}

			yield return www.Send();

			ScriptEndpoint response = JsonConvert.DeserializeObject<ScriptEndpoint>(www.downloadHandler.text, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
			ReadWebRequest(response, www);

			if(response.IsSuccess)
			{
				if(string.IsNullOrEmpty(response.stderr) == false)
				{
					response.IsSuccess = false;
				}

				if(string.IsNullOrEmpty(response.stdout) == false && response.stdout.StartsWith("error { Error"))
				{
					response.IsSuccess = false;
					response.IsSyncanoError = true;
					response.syncanoError = response.stdout;
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
		private UnityWebRequest PrepareWebRequest(string url, string serializedObject, Dictionary<string, string> payload, string httpMethodOverride = null)
		{
			UnityWebRequest www = new UnityWebRequest(url);
			www.SetRequestHeader(Constants.HTTP_HEADER_API_KEY, SyncanoClient.Instance.ApiKey);
			www.SetRequestHeader("Content-Type", "application/json");

			if(string.IsNullOrEmpty(SyncanoClient.Instance.UserKey) == false)
			{
				www.SetRequestHeader("X-USER-KEY", SyncanoClient.Instance.UserKey);
			}

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
		private string UrlBuilder(string id, Type classType, Dictionary<string, string> getData = null, string url = null) {
			
			if(string.IsNullOrEmpty(url))
			{
				StringBuilder sb = new StringBuilder(Constants.PRODUCTION_SERVER_URL);

				string ID = string.IsNullOrEmpty(id) == false ? id.ToString() : string.Empty;

				if(classType.IsGenericTypeDefinition)
				{
					Type t = classType.GetGenericTypeDefinition();
					if(t == typeof(User<>))
					{
						sb.Append(string.Format("/v1.1/instances/{0}/users/", SyncanoClient.Instance.InstanceName));
					}

					else
					{
						sb.Append(string.Format(Constants.OBJECTS_DETAIL_URL, SyncanoClient.Instance.InstanceName, classType.ToString(), ID));
					}
				}

				else
				{
					sb.Append(string.Format(Constants.OBJECTS_DETAIL_URL, SyncanoClient.Instance.InstanceName, classType.ToString(), ID));
				}

				url = sb.ToString();
			}

			StringBuilder urlStringBuilder = new StringBuilder(url);

			if(getData != null)
			{
				urlStringBuilder.Append("?");

				foreach(KeyValuePair<string, string> pair in getData)
				{
					if(string.IsNullOrEmpty(pair.Key) == false && string.IsNullOrEmpty(pair.Value) == false)
					{
						urlStringBuilder.Append(pair.Key).Append("=").Append(pair.Value);
					}

					urlStringBuilder.Append("&");
				}
			}

			if(url.EndsWith("&"))
			{
				url = url.Remove(url.Length);
			}

			return urlStringBuilder.ToString();
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

			case UnityWebRequest.kHttpVerbPUT:
				if(resultCode == HTTP_CODE_SUCCESS)
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