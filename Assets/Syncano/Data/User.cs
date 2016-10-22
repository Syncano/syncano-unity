using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using Syncano.Client;

/// <summary>
/// Default user class.
/// Its profile class contains default DataObject firlds.
/// If you want to have a custom user, you need to override AbstractUser and provide custom Profile class.
/// </summary>
namespace Syncano.Data {
	public class User<T> : AbstractUser<T> where T : Profile, new() {

	public User() {
	}

	public User(string userName, string password) : base(userName, password) {
			
	}
	
	public Coroutine Register(Action<Response<User<T>>> callback)
	{
			return SyncanoClient.Instance.RegisterUser(string.Format("https://api.syncano.io/v1.1/instances/{0}/users/", SyncanoClient.Instance.InstanceName), this, callback, UnityEngine.Networking.UnityWebRequest.kHttpVerbPOST);
	}

	public Coroutine Register(Action<Response<User<T>>> onSuccess, Action<Response<User<T>>> onFailure)
	{
			return SyncanoClient.Instance.RegisterUser(string.Format("https://api.syncano.io/v1.1/instances/{0}/users/", SyncanoClient.Instance.InstanceName), this, onSuccess, onFailure, UnityEngine.Networking.UnityWebRequest.kHttpVerbPOST);
	}

	public Coroutine UpdateCustomUser(Action<Response<User<T>>> onSuccess, Action<Response<User<T>>> onFailure)
	{
		string url = string.Format(Constants.PRODUCTION_SERVER_URL + "/v1.1/instances/{0}/users/{1}/", SyncanoClient.Instance.InstanceName, Id);
		return SyncanoHttpClient.Instance.CreateOrUpdateUser(url, this, onSuccess, onFailure, UnityEngine.Networking.UnityWebRequest.kHttpVerbPUT);
	}

	public Coroutine Login(Action<Response<User<T>>> onSuccess, Action<Response<User<T>>> onFailure)
	{
		string url = string.Format(Constants.PRODUCTION_SERVER_URL +"/v1.1/instances/{0}/user/auth/", SyncanoClient.Instance.InstanceName);
		return SyncanoHttpClient.Instance.PostAsync(this, onSuccess, onFailure, null, url, onAfterSuccess:OnAfterSuccess);
	}

	private void OnAfterSuccess(string response)
	{
		JsonConvert.PopulateObject(response, this);
		SyncanoClient.Instance.UserKey = UserKey;
	}
}
}