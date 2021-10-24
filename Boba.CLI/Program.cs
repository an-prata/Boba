using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using Boba.PasswordManager;
using Boba.PasswordManager.FileHandling;

namespace Boba.CLI
{
	class Program
	{
		public const string Version = "0.0.1";

		static void Main(string[] args)
		{
			EncryptedPasswordLibrary encryptedPasswordLibrary = new EncryptedPasswordLibrary(new RSACryptoServiceProvider(), "untitled", new List<EncryptedPasswordEntry>());
			Console.WriteLine($"Boba version {Version}");

			while (true)
			{
				Console.Write("Boba: ");
				string[] command = Console.ReadLine().Split(' ');

				switch (command[0])
				{
					case "open":
						try
						{
							encryptedPasswordLibrary = JsonHandler.ReadFromFile<EncryptedPasswordLibrary>(command[1]);
							Console.WriteLine($"Opened library: {command[1]}");
						}
						catch (IndexOutOfRangeException)
						{
							Console.WriteLine("Usage: open [library file]");
						}
						break;

					case "add":
						try { encryptedPasswordLibrary.NewEntry(Encoding.UTF8.GetBytes(command[3]), command[1], command[2]); }
						catch (IndexOutOfRangeException) { Console.WriteLine("Usage: add [application/site] [username] [password]"); }
						break;

					case "remove":
						try { encryptedPasswordLibrary.PasswordEntries.Remove(encryptedPasswordLibrary.PasswordEntries[Convert.ToInt32(command[1])]); }
						catch (IndexOutOfRangeException) { Console.WriteLine("Usage: remove [entry number]"); }
						catch { Console.WriteLine("Please enter a valid argument to remove an entry."); }
						break;

					case "rename":
						try { encryptedPasswordLibrary.Name = command[1]; }
						catch (IndexOutOfRangeException) { Console.WriteLine("Usage: rename [new library name]"); }
						break;

					case "new":
						try 
						{
							try 
							{ 
								encryptedPasswordLibrary = new EncryptedPasswordLibrary(new RSACryptoServiceProvider(Convert.ToInt32(command[2])), command[1], new List<EncryptedPasswordEntry>());
								Console.WriteLine($"Created and opened new library with key length {Convert.ToString(encryptedPasswordLibrary.CryptoServiceProvider.KeySize)}: {encryptedPasswordLibrary.Name}");
								break;
							}
							catch 
							{
								Console.WriteLine("Usage: new [key size] [library name]");
							}
						}
						catch (IndexOutOfRangeException) 
						{
							try { encryptedPasswordLibrary = new EncryptedPasswordLibrary(new RSACryptoServiceProvider(), command[1], new List<EncryptedPasswordEntry>()); }
							catch (IndexOutOfRangeException) { Console.WriteLine("Usage: new [key size (2048)] [new library name]"); }
						}
						break;

					case "save":
						try 
						{ 
							JsonHandler.SaveToFile(command[1], encryptedPasswordLibrary);
							XmlHandler.SaveToFile(command[2], encryptedPasswordLibrary.CryptoServiceProvider.ExportParameters(false));
							XmlHandler.SaveToFile(command[3], encryptedPasswordLibrary.CryptoServiceProvider.ExportParameters(true));
						}
						catch (IndexOutOfRangeException)
						{
							try
							{
								JsonHandler.SaveToFile(command[1], encryptedPasswordLibrary);
							}
							catch (IndexOutOfRangeException)
							{
								JsonHandler.SaveToFile($"{encryptedPasswordLibrary.Name}.json", encryptedPasswordLibrary);
								XmlHandler.SaveToFile("publickey.xml", encryptedPasswordLibrary.CryptoServiceProvider.ExportParameters(false));
								XmlHandler.SaveToFile("privatekey.xml", encryptedPasswordLibrary.CryptoServiceProvider.ExportParameters(true));
							}
						}
						catch
						{
							Console.WriteLine("Usage: save [library file] [public key file] [private key file]");
							Console.WriteLine("Can also be used without any arguments other than \"save\" to save files with default names to the current directory.");
						}
						break;

					case "list":
						Console.WriteLine($"\n{encryptedPasswordLibrary.PasswordEntries.Count} Entries in library: {encryptedPasswordLibrary.Name}:");
						Console.WriteLine("---------------------------------------");
						Console.WriteLine($"\tNum:\tName:");
						for (int i = 0; i < encryptedPasswordLibrary.PasswordEntries.Count; i++) 
							Console.WriteLine($"\t{Convert.ToString(i)}\t{encryptedPasswordLibrary.PasswordEntries[i].Application}");
						break;

					case "exit":
						Environment.Exit(0);
						break;
				}
			}
		}
	}
}
