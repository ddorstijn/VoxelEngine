using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class Serialization
{
	public static string saveFolderName = "saves";

    // Create the directory for the world
    public static string SaveLocation(string worldName)
    {
		string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation)) {
			Directory.CreateDirectory(saveLocation);
		}

		return saveLocation;
	}

    // Create the filename for the chunk based on the chunk's location in the world
    public static string FileName(WorldPos chunkLocation)
    { 
		string fileName = chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
		return fileName;
	}

	public static void SaveChunk(Chunk chunk)
    {
        // Prepare the blocks to be saved
        WorldSave save = new WorldSave(chunk);
        
        // If there are blocks in the chunk
        if (save.blocks.Count > 0) {
			string saveFile = SaveLocation(chunk.world.worldName);
			saveFile += FileName(chunk.pos);

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);
			formatter.Serialize(stream, save);
			stream.Close();
		}
	}

	public static bool Load(Chunk chunk)
    {
		bool retVal = true;

		string saveFile = SaveLocation(chunk.world.worldName);
		saveFile += FileName(chunk.pos);

		if(!File.Exists(saveFile)) {
			retVal = false;
		} else {
			// Loads the worldsave with a binary formater and sets the blocks to the given chunk directly
			IFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(saveFile, FileMode.Open);

			WorldSave save = (WorldSave) formatter.Deserialize(stream);
			foreach(var block in save.blocks) {
				chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
			}

			stream.Close();
		}

		return retVal;
	}
}
