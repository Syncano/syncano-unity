using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

namespace Syncano.Data{
public class Group {

	public const string FIELD_ID = "id";
	public const string FIELD_LABEL = "label";
	public const string FIELD_DESCRIPTION = "description";

	/// <summary>
	/// Gets or sets the identifier.
	/// </summary>
	/// <value>The identifier.</value>
	[JsonProperty(FIELD_ID)]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the label.
	/// </summary>
	/// <value>The label.</value>
	[JsonProperty(FIELD_LABEL)]
	public string Label { get; set; }

	/// <summary>
	/// Gets or sets the description.
	/// </summary>
	/// <value>The description.</value>
	[JsonProperty(FIELD_DESCRIPTION)]
	public string Description { get; set; }

	public Group() { }
}
}