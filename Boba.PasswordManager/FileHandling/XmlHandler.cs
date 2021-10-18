using System;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace Boba.PasswordManager.FileHandling
{
    public class XmlHandler
    {
		public static void SaveToFile(string filePath, object value)
		{
			using FileStream fileStream = new FileStream(filePath, FileMode.Create);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(RSAParameters));
			xmlSerializer.Serialize(fileStream, value);
		}

		public static RSAParameters ReadFromFile(string filePath)
		{
			using FileStream fileStream = new FileStream(filePath, FileMode.Open);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(RSAParameters));
			return (RSAParameters)xmlSerializer.Deserialize(fileStream);
		}
	}
}
