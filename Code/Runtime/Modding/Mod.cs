using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using IronPython.Hosting;
using EarthianTagFormat;

namespace Earthian.Code.Runtime.Modding
{
	public class ModHeader
	{
		public string ModName { get; set; }

		public List<string> ModAuthors { get; set; }

		public List<string> ModDependancies { get; set; }
	}

	public class Mod
	{
		public ModHeader Header { get; set; }

		public List<ModCode> Code { get; set; }


		private enum CurrentInternAction
		{
			HEADER_DATA_BEGUN,
			CODE_DATA_BEGUN,
			TEXTURE_DATA_BEGUN,
			AUDIO_DATA_BEGUN,
			SHADER_DATA_BEGUN,
			NONE
		}

		public Mod(string AssemblyName)
		{
			Header = new ModHeader();
			Code = new List<ModCode>();
			CurrentInternAction action = CurrentInternAction.NONE;
			string dir = (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Mods/" + AssemblyName);

			try
			{
				TagCompound c = (TagCompound)ETF.LoadData(dir);

				Dictionary<string, Tag> dict = c.GetTagDict();

				foreach (string tag in dict.Keys)
				{
					if (action == CurrentInternAction.HEADER_DATA_BEGUN)
					{
						if (tag == "header_end")
						{
							action = CurrentInternAction.NONE;
						}
						else
						{
							if (tag == "mod_name")
								Header.ModName = (string)dict[tag].GetValue();
							else if (tag == "mod_authors")
							{
								Dictionary<string, Tag> t = ((TagCompound)dict[tag]).GetTagDict();
								foreach (string tagi in t.Keys)
								{
									Header.ModAuthors.Add((string)t[tagi].GetValue());
								}
							}
							else if (tag == "mod_deps")
							{
								Dictionary<string, Tag> t = ((TagCompound)dict[tag]).GetTagDict();
								foreach (string tagi in t.Keys)
								{
									Header.ModAuthors.Add((string)t[tagi].GetValue());
								}
							}
						}
					}
					else if (action == CurrentInternAction.CODE_DATA_BEGUN)
					{
						if (tag == "code_block_end")
						{
							action = CurrentInternAction.NONE;
						}
						else
						{
							Code.Add(new ModCode(tag, (string)dict[tag].GetValue()));
						}
					}
					else
					{
						if (tag == "header_begin")
							action = CurrentInternAction.HEADER_DATA_BEGUN;
						else if (tag == "code_block_begin")
							action = CurrentInternAction.CODE_DATA_BEGUN;
					}
				}
			}
			catch (NullReferenceException)
			{
				Console.WriteLine(DateTime.UtcNow + " | Error, could not load mod " + AssemblyName + "! [Mod data seems to be empty!]...");
			}
			catch
			{
				Console.WriteLine(DateTime.UtcNow + " | Error, could not load mod " + AssemblyName + "!...");
			}
		}
	}

	public class ModCode
	{
		public ModCode(string RefName, string Data)
		{
			this.RefName = RefName;
			this.Data = Data;
		}

		public string Data { get; set; }

		public string RefName { get; set; }
	}


}
