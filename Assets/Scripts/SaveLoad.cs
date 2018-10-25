using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public static class SaveLoad {

	public static void LoadFromPersistent<T> (ref T obj, string fileName) where T : new() {
		string filePath = Path.Combine (Application.persistentDataPath, fileName);
		if (File.Exists (filePath)) {
			byte[] jsonBytes = File.ReadAllBytes (filePath);
			string dataAsJson = Encoding.ASCII.GetString (jsonBytes).Trim ();
			obj = JsonHelper.FromJson<T> (dataAsJson);
		} else {
			obj = new T ();
		}
	}

	public static void SaveToPersistent<T> (ref T obj, string fileName) {
		string filePath = Path.Combine (Application.persistentDataPath, fileName);
		string dataAsJson = JsonHelper.ToJson (obj, true);
		byte[] jsonBytes = Encoding.ASCII.GetBytes (dataAsJson);
		File.WriteAllBytes (filePath, jsonBytes);
	}

	public static void SaveToAssets<T> (ref T obj, string fileName) {	//Use only in Editor!
		if (Application.platform == RuntimePlatform.WindowsEditor) {
			string filePath = Path.Combine (Application.streamingAssetsPath, fileName);
			string dataAsJson = JsonHelper.ToJson (obj, true);
			File.WriteAllText (filePath, dataAsJson);
		} else {
			Debug.LogError ("Not in Editor!");
		}
	}

	public static void LoadFromAssets<T> (ref T obj, string fileName) where T : new() {
		string filePath = Path.Combine (Application.streamingAssetsPath, fileName);
		string dataAsJson;

		if (Application.platform == RuntimePlatform.Android) {
			WWW www = new WWW (filePath);
			while (!www.isDone) {
			}
			byte[] wwwBytes = www.bytes;
			dataAsJson = Encoding.ASCII.GetString (wwwBytes).Trim ();
		} else {
			dataAsJson = File.ReadAllText (filePath);
		}

		obj = JsonHelper.FromJson<T> (dataAsJson);
	}

	public static class JsonHelper
	{
		public static T FromJson<T>(string json)
		{
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
			return wrapper.Items;
		}

		public static string ToJson<T>(T item)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = item;
			return JsonUtility.ToJson(wrapper);
		}

		public static string ToJson<T>(T item, bool prettyPrint)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = item;
			return JsonUtility.ToJson(wrapper, prettyPrint);
		}

		[System.Serializable]
		private class Wrapper<T>
		{
			public T Items;
		}
	}
}