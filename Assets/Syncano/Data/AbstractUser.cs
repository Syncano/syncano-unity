using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Syncano;

namespace Syncano.Data {
	public abstract class AbstractUser<P> : SyncanoObject where P : Profile {

	public const string FIELD_USER_NAME = "username";
	public const string FIELD_PASSWORD = "password";
	public const string FIELD_USER_KEY = "user_key";
	public const string FIELD_PROFILE = "profile";
	public const string FIELD_ACCESS_TOKEN = "access_token";

	/// <summary>
	/// Gets or sets the name of the user.
	/// </summary>
	/// <value>The name of the user.</value>
	[JsonProperty(FIELD_USER_NAME)]
	public string UserName { get; set; }

	/// <summary>
	/// Gets or sets the password.
	/// </summary>
	/// <value>The password.</value>
	[JsonProperty(FIELD_PASSWORD)]
	public string Password { get; set; }

	/// <summary>
	/// Gets or sets the user key.
	/// </summary>
	/// <value>The user key.</value>
	[JsonProperty(FIELD_USER_KEY)]
	public string UserKey { get; set; }

	/// <summary>
	/// Gets or sets the profile.
	/// </summary>
	/// <value>The profile.</value>
	[JsonProperty(FIELD_PROFILE, NullValueHandling = NullValueHandling.Ignore)]
	public P Profile { get; set; }

	/// <summary>
	/// Gets or sets the auth token.
	/// </summary>
	/// <value>The auth token.</value>
	[JsonProperty(FIELD_ACCESS_TOKEN)]
	public string AuthToken { get; set; }

	public AbstractUser() { }

	public AbstractUser(string userName, string password) {
		UserName = userName;
		Password = password;
	}

}
}