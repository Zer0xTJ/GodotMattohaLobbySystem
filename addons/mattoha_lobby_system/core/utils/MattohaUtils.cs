using Godot;
using Godot.Collections;
using Mattoha.Core.Utils;
using System;
using System.Collections;

namespace Mattoha.Core;

public class MattohaUtils
{

	/// <summary>
	/// Return Godot Dictionary without Props that exists in "PrivateProps" list property, used when sending player data to other players.
	/// </summary>
	/// <param name="dict">dictionary to copy from.</param>
	/// <returns>New dictionary without PrivateProps properties.</returns>
	public static Dictionary<string, Variant> ToSecuredDict(Dictionary<string, Variant> dict)
	{
		Dictionary<string, Variant> newObj = new();
		var privateProps = GetPrivateProps(dict);
		foreach (var kvp in dict)
		{
			if (kvp.Value.Obj == null)
			{
				continue;
			}
			if (IsPrimitive(kvp.Value) && !privateProps.Contains(kvp.Key))
			{
				newObj.Add(kvp.Key, kvp.Value);
			}
			else if (IsEnumerable(kvp.Value) && !privateProps.Contains(kvp.Key))
			{
				Array<Variant> items = new();
				foreach (var item in kvp.Value.AsGodotArray())
				{
					if (item.Obj != null)
					{
						if (IsPrimitive(item))
						{
							items.Add(item);
						}
						else
						{
							items.Add(ToSecuredDict(item.AsGodotDictionary<string, Variant>()));
						}
					}
				}
				if (items.Count > 0)
				{
					newObj.Add(kvp.Key, items);
				}
			}
			// its a dictionary
			else if (!privateProps.Contains(kvp.Key))
			{
				Dictionary<string, Variant> val = ToSecuredDict(kvp.Value.AsGodotDictionary<string, Variant>());
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
	/// Convert GodotDictionary to a  new object that has only ChatProps fields.
	/// </summary>
	/// <param name="dict">object to copy from.</param>
	/// <returns>GodotDictionary with ChatProps only.</returns>
	public static Dictionary<string, Variant> ToChatDict(Dictionary<string, Variant> dict)
	{
		if(dict == null)
			return null;

		Dictionary<string, Variant> newObj = new();
		var chatProps = GetChatProps(dict);

		foreach (var kvp in dict)
		{
			if (kvp.Value.Obj == null)
			{
				continue;
			}

			if (IsPrimitive(kvp.Value) && chatProps.Contains(kvp.Key))
			{
				newObj[kvp.Key] = kvp.Value;
			}
			else if (IsEnumerable(kvp.Value))
			{
				bool shouldAddItems = true;
				Array<Variant> items = new();
				foreach (var item in kvp.Value.AsGodotArray())
				{
					if (item.Obj != null)
					{
						if (IsPrimitive(item) && chatProps.Contains(kvp.Key))
						{
							shouldAddItems = false;
							break;
						}
						else if (IsPrimitive(item) && chatProps.Contains(kvp.Key))
						{
							items.Add(item);
						}
						else if (!IsPrimitive(item))
						{
							items.Add(ToChatDict(item.AsGodotDictionary<string, Variant>()));
						}
					}
				}
				if (shouldAddItems && items.Count > 0)
				{
					newObj.Add(kvp.Key, items);
				}
			}
			else if (!IsPrimitive(kvp.Value))
			{
				var val = ToChatDict(kvp.Value.AsGodotDictionary<string, Variant>());
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
	/// Get A PrivateProps property value as List<string> from GodotDictionary.
	/// </summary>
	/// <param name="dict">GodotDictionary to check if it has PrivateProps.</param>
	/// <returns>List of private properties names.</returns>
	public static Array<string> GetPrivateProps(Dictionary<string, Variant> dict)
	{
		if(dict == null)
			return null;
		Array<string> privateProps = new() { nameof(MattohaPlayerKeys.ChatProps), nameof(MattohaPlayerKeys.PrivateProps) };
		if (dict.ContainsKey("PrivateProps"))
		{
			var node = dict["PrivateProps"];
			if (node.Obj != null)
			{
				foreach (var item in node.AsGodotArray())
				{
					if (item.Obj != null && !privateProps.Contains($"{item.AsString()}"))
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
	/// Get A ChatProps property value as List<string> from GodotDictionary.
	/// </summary>
	/// <param name="dict">GodotDictionary to check if it has ChatProps.</param>
	/// <returns>List of chat properties names.</returns>
	public static Array<String> GetChatProps(Dictionary<string, Variant> dict)
	{
		Array<string> chatProps = new();
		if (dict.ContainsKey("ChatProps"))
		{
			var node = dict["ChatProps"];
			if (node.Obj != null)
			{
				foreach (var item in node.AsGodotArray())
				{
					if (item.Obj != null && !chatProps.Contains($"{item.AsString()}"))
					{
						chatProps.Add(item.ToString());
					}
				}
			}
		}
		return chatProps;
	}


	/// <summary>
	/// Check if a Variant object is primitive.
	/// </summary>
	public static bool IsPrimitive(Variant obj)
	{
		return obj.Obj.GetType().IsPrimitive ||
			   obj.Obj.GetType() == typeof(string) ||
			   obj.Obj.GetType() == typeof(decimal);
	}


	/// <summary>
	/// Check if a Variant object is enumerbale.
	/// </summary>
	public static bool IsEnumerable(Variant obj)
	{
		return obj.Obj is IEnumerable;
	}
}
