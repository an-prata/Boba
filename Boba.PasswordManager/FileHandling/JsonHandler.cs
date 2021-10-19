using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Boba.PasswordManager.FileHandling
{
	public class JsonHandler
	{
		public static void SaveToFile(string filePath, object value)
		{
			using FileStream fileStream = new FileStream(filePath, FileMode.Create);
			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions() { WriteIndented = true };
			byte[] bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, value.GetType(), jsonSerializerOptions));
			fileStream.Write(bytes, 0, bytes.Length);
		}

		public static TValue ReadFromFile<TValue>(string filePath)
		{
			string jsonString = File.ReadAllText(filePath);
			return JsonSerializer.Deserialize<TValue>(jsonString);
		}
	}
}
