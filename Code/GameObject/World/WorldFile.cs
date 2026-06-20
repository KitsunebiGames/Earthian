using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Earthian.GameObject.Block;
using Microsoft.Xna.Framework;
using EarthianTagFormat;
using Newtonsoft.Json;

namespace Earthian.GameObject.World
{
	public class Moddata
	{
		public string mod { get; set; }

		public string modEntry { get; set; }

		public bool isEnabled { get; set; }
	}

	public class WorldData
	{
		public string world_name { get; set; }

		public string world_seed { get; set; }

		public List<Moddata> moddata { get; set; }

		public WorldData()
		{
			this.moddata = new List<Moddata>();
		}

		public WorldData(string worldname, string worldseed)
		{
			this.world_name = worldname;
			this.world_seed = world_seed;
			this.moddata = new List<Moddata>();
		}
	}

	public class WorldFile
	{
		private string worldName;

        public static string worldPath = "worlds/";

		private WorldData data;

		public WorldData SubData
		{
			get { return data; }
			set { ; }
		}

		public TagCompound GetChunkData(ChunkPos pos)
		{
			string fp = worldPath + String.Format("{2}/worlddata/{0}-{1}.dat", pos.X, pos.Y, this.worldName);
			TagCompound data = (TagCompound)ETF.LoadData(fp);
			return data;
		}

		public WorldFile(string name)
		{
			this.worldName = name;
			if (!Directory.Exists(worldPath)) Directory.CreateDirectory(worldPath);
			if (!Directory.Exists(worldPath + name)) Directory.CreateDirectory(worldPath + name);
			if (!Directory.Exists(worldPath + name + "/worldInfo.lst")) createWorld(name, "SEED");
			this.data = JsonConvert.DeserializeObject<WorldData>(File.ReadAllText(worldPath + name + "/worldInfo.lst"));
		}

		public WorldFile(string folderPath, WorldData headerdata)
		{
            TryCreateDirectory(worldPath);
            TryCreateDirectory(worldPath + folderPath);
			data = headerdata;
			File.WriteAllText(worldPath + folderPath + "/worldInfo.lst", JsonConvert.SerializeObject(data, Formatting.Indented));
		}

		private void createWorld(string name, string seed)
		{
			WorldData wd = new WorldData(name, seed);
			data = wd;
			File.WriteAllText(worldPath + name + "/worldInfo.lst", JsonConvert.SerializeObject(data, Formatting.Indented));
		}

		public bool ChunkExists(ChunkPos pos)
		{
            TryCreateDirectory(worldPath + worldName + "/worlddata");
			return File.Exists(worldPath + String.Format("{2}/worlddata/{0}-{1}.dat", pos.X, pos.Y, this.worldName));
		}

		public void SaveChunkData(ChunkPos pos, TagCompound data)
		{
			string fp = worldPath + String.Format("{2}/worlddata/{0}-{1}.dat", pos.X, pos.Y, this.worldName);
			ETF.SaveData(fp, data);
		}

		public static List<WorldFile> GetWorlds()
		{
			List<WorldFile> worlds = new List<WorldFile>();
			if (!Directory.Exists(worldPath)) Directory.CreateDirectory(worldPath);
			foreach (string dir in Directory.GetDirectories(worldPath))
			{
				worlds.Add(new WorldFile(dir.Replace(worldPath, "")));
			}
			return worlds;
		}

        public static void TryCreateDirectory(string dir)
        {
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
        }
	}
}
