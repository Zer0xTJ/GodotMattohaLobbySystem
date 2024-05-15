using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MattohaLobbySystem.Core.Nodes;

public class MattohaUtils
{

	/// <summary>
	/// Return JsonObject without Props that exists in "PrivateProps" list property, used when sending player data to other players.
	/// </summary>
	/// <param name="obj">object to copy from.</param>
	/// <returns>new object without PrivateProps properties.</returns>
	public static JsonObject ToHiddenPrivatePropsObject(JsonObject obj)
	{
		JsonObject newObj = new();
		var privateProps = GetPrivateProps(obj);
		foreach (var kvp in obj)
		{
			if (kvp.Value == null)
			{
				continue;
			}
			if (IsPrimitive(kvp.Value) && !privateProps.Contains(kvp.Key))
			{
				newObj.Add(kvp.Key, ToJsonNode(kvp.Value));
			}
			else if (IsEnumerable(kvp.Value) && !privateProps.Contains(kvp.Key))
			{
				List<object> items = new();
				foreach (var item in kvp.Value.AsArray())
				{
					if (item != null)
					{
						if (IsPrimitive(item))
						{
							items.Add(ToJsonNode(item)!);
						}
						else
						{
							items.Add(ToHiddenPrivatePropsObject(ToJsonObject(item)));
						}
					}
				}
				if (items.Count > 0)
				{
					newObj.Add(kvp.Key, ToJsonNode(items));
				}
			}
			// its an object
			else if (!privateProps.Contains(kvp.Key))
			{
				JsonObject val = ToHiddenPrivatePropsObject(ToJsonObject(kvp.Value));
				if (val != null)
				{
					if (val.Count > 0)
					{
						newObj[kvp.Key] = val;
					}
				}
			}
		}
		return newObj;
	}


	/// <summary>
	/// Convert JsonObject to a  new object that has only ChatProps fields.
	/// </summary>
	/// <param name="obj">object to copy from.</param>
	/// <returns>JsonObject with ChatProps only.</returns>
	public static JsonObject ToChatObject(JsonObject obj)
	{

		JsonObject newObj = new();
		var chatProps = GetChatProps(obj);

		foreach (var kvp in obj)
		{
			if (kvp.Value == null)
			{
				continue;
			}

			if (IsPrimitive(kvp.Value) && chatProps.Contains(kvp.Key))
			{
				newObj[kvp.Key] = ToJsonNode(kvp.Value);
			}
			else if (IsEnumerable(kvp.Value))
			{
				bool shouldAddItems = true;
				List<object> items = new();
				foreach (var item in kvp.Value.AsArray())
				{
					if (item != null)
					{
						if (IsPrimitive(item) && chatProps.Contains(kvp.Key))
						{
							shouldAddItems = false;
							break;
						}
						else if (IsPrimitive(item) && chatProps.Contains(kvp.Key))
						{
							items.Add(ToJsonNode(item)!);
						}
						else if (!IsPrimitive(item))
						{
							items.Add(ToChatObject(ToJsonObject(item)));
						}
					}
				}
				if (shouldAddItems && items.Count > 0)
				{
					newObj.Add(kvp.Key, ToJsonNode(items));
				}
			}
			else if (!IsPrimitive(kvp.Value))
			{
				JsonObject val = ToChatObject(ToJsonObject(kvp.Value));
				if (val != null)
				{
					if (val.Count > 0)
					{
						newObj.Add(kvp.Key, val);
					}
				}
			}
		}

		return newObj;
	}


	/// <summary>
	/// Get A PrivateProps property value as List<string> from JsonObject.
	/// </summary>
	/// <param name="obj">JsonObject to check if it has PrivateProps.</param>
	/// <returns>List of private properties names.</returns>
	public static List<String> GetPrivateProps(JsonObject obj)
	{
		List<string> privateProps = new() { "ChatProps", "PrivateProps" };
		if (obj.ContainsKey("PrivateProps"))
		{
			var node = obj["PrivateProps"];
			if (node != null)
			{
				foreach (var item in node.AsArray())
				{
					if (item != null && !privateProps.Contains($"{item}"))
					{
						privateProps.Add(item.ToString());
					}
				}
			}
		}
		privateProps.Remove("Id");
		return privateProps;
	}


	/// <summary>
	/// Get A ChatProps property value as List<string> from JsonObject.
	/// </summary>
	/// <param name="obj">JsonObject to check if it has ChatProps.</param>
	/// <returns>List of chat properties names.</returns>
	public static List<String> GetChatProps(JsonObject obj)
	{
		List<string> chatProps = new();
		if (obj.ContainsKey("ChatProps"))
		{
			var node = obj["ChatProps"];
			if (node != null)
			{
				foreach (var item in node.AsArray())
				{
					if (item != null && !chatProps.Contains($"{item}"))
					{
						chatProps.Add(item.ToString());
					}
				}
			}
		}
		return chatProps;
	}


	/// <summary>
	/// Check if an object is primitive.
	/// </summary>
	public static bool IsPrimitive(object obj)
	{
		return obj.GetType().IsPrimitive ||
			   obj.GetType() == typeof(string) ||
			   obj.GetType() == typeof(decimal) ||
			   obj is JsonValue;
	}


	/// <summary>
	/// Check if an object is enumerbale.
	/// </summary>
	public static bool IsEnumerable(object obj)
	{
		return obj is IEnumerable && obj is not JsonObject;
	}


	/// <summary>
	/// Used to convert any object to JsonObject.
	/// </summary>
	/// <param name="obj">Object to converts.</param>
	/// <returns>JsonObject</returns>
	public static JsonObject ToJsonObject(dynamic obj)
	{
		return JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(obj));
	}


	/// <summary>
	/// Used to convert any json string to a JsonObject.
	/// </summary>
	/// <param name="jsonString">json string to converts.</param>
	/// <returns>JsonObject</returns>
	public static JsonObject? ToJsonObject(string jsonString)
	{
		return JsonSerializer.Deserialize<JsonObject>(jsonString);
	}


	/// <summary>
	/// Convert any object to a JsonNode object.
	/// </summary>
	/// <param name="obj">object to convert.</param>
	/// <returns>JsonNode</returns>
	public static JsonNode? ToJsonNode(object obj)
	{
		return JsonSerializer.Deserialize<JsonNode>(JsonSerializer.Serialize(obj));
	}


	/// <summary>
	/// Used to convert any json string to concrete object, usefull for working with custom datatypes in your project.
	/// </summary>
	/// <typeparam name="T">Object type</typeparam>
	/// <param name="json">json string to convert</param>
	/// <returns>new Object of Type T</returns>
	public static T? Deserialize<T>(string json)
	{
		return JsonSerializer.Deserialize<T>(json);
	}


	/// <summary>
	/// Used to convert any JsonObject to concrete object, usefull for working with custom datatypes in your project.
	/// </summary>
	/// <typeparam name="T">Object type</typeparam>
	/// <param name="jsonObject">json string to convert</param>
	/// <returns>new Object of Type T</returns>
	public static T? Deserialize<T>(JsonObject jsonObject)
	{
		return JsonSerializer.Deserialize<T>(jsonObject);
	}

	/// <summary>
	/// Serialize any object to json string.
	/// </summary>
	/// <param name="obj">object to serialize</param>
	/// <returns>object in json string format</returns>
	public static string Serialize(object obj)
	{
		return JsonSerializer.Serialize(obj);
	}
}
