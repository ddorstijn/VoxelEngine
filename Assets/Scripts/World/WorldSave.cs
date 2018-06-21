using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class WorldSave {

	//creates a dictionary with the world position of each block which will then be passed to a binary formatter to be saved

	public Dictionary<WorldPos, Block> blocks = new Dictionary<WorldPos, Block>();


	public WorldSave(Chunk chunk) {

		for(int x = 0; x< Chunk.chunkSize; x++) 
        {
			for(int y = 0; y< Chunk.chunkSize; y++)
            {
				for(int z = 0; z< Chunk.chunkSize; z++) 
                {
					if(chunk.blocks[x, y, z].changed) 
                    {
						WorldPos pos = new WorldPos(x, y, z);
						blocks.Add(pos, chunk.blocks[x, y, z]);
					}
				}

			}

		}

	}
}
