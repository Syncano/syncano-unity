using UnityEngine;
using System.Collections;
using Syncano.Request;

namespace Syncano.Data {
/// <summary>
/// Class representing ScriptEndpoint structure.
/// </summary>
public class ScriptEndpoint : SyncanoWebRequest {
	
	/// <summary>
	/// The serialized result.
	/// </summary>
	public Result result;

	/// <summary>
	/// Shortcut for error message.
	/// </summary>
	public string stderr { get { return result.stderr; } }

	/// <summary>
	/// Shortcut for serialized Output in JSON.
	/// </summary>
	public string stdout { get { return result.stdout; } }

	public ScriptEndpoint () { }

	[System.Serializable]
	public class Result
	{
		/// <summary>
		/// Error message.
		/// </summary>
		public string stderr;

		/// <summary>
		/// Serialized Output in JSON.
		/// </summary>
		public string stdout;
	}
}
}