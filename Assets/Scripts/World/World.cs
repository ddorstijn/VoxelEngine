using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//this class now derives from the Generate Terrain class
public class World : World_Base {

    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
    public string worldName = "world";
    public GameObject chunkPrefab;

    [HideInInspector]
    public int newChunkX;
    [HideInInspector]
    public int newChunkY;
    [HideInInspector]
    public int newChunkZ;

    // Use this for initialization
    [ContextMenu("Generate level")]
    void Start()
    {
        for (int x = -8; x < 8; x++) 
            for (int y = -4; y < 4; y++) 
                for (int z = -8; z < 8; z++) 
                    CreateChunk(x * 16, y * 16, z * 16);
    }

    public void SaveAllChunks()
    {
        foreach (Chunk ch in chunks.Values) {
            Serialization.SaveChunk(ch);
        }
    }

    // Create a chunk from the prefab
    public void CreateChunk(int x, int y, int z)
    {
        WorldPos worldPos = new WorldPos(x, y, z);

        GameObject newChunkObject = (GameObject)Instantiate(chunkPrefab, new Vector3(x, y, z), Quaternion.identity);
        Chunk newChunk = newChunkObject.GetComponent<Chunk>();

        newChunk.pos = worldPos;
        newChunk.world = this;

        chunks.Add(worldPos, newChunk);

        newChunk = Generate_Chunk(newChunk);

        newChunk.SetBlockUnmodified();
        Serialization.Load(newChunk);
    }

    //Gets a chunk from the world based on a block's position
    public Chunk GetChunk(int x, int y, int z)
    {
        float multiple = Chunk.chunkSize;
        WorldPos pos = new WorldPos(0, 0, 0)
        {
            x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize,
            y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize,
            z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize
        };

        Chunk containerChunk = null;
        chunks.TryGetValue(pos, out containerChunk);

        return containerChunk;
    }

    //Destroys a chunk
    public void DestroyChunk(int x, int y, int z)
    {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk)) {
            Serialization.SaveChunk(chunk);
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }

    //Gets a block at the given world position
    public Block GetBlock(int x, int y, int z)
    {
        Block retBlock = null;
        Chunk containerChunk = GetChunk(x, y, z);

        //if no chunk is found, the block is assumed to be air
        if (containerChunk != null) {
            Block block = containerChunk.GetBlock(
                x - containerChunk.pos.x,
                y - containerChunk.pos.y,
                z - containerChunk.pos.z);

            retBlock = block;
        } else {
            retBlock = new BlockAir();
        }

        return retBlock;
    }

    //Sets a block to the given world position
    public void SetBlock(int x, int y, int z, Block block)
    {
        Chunk chunk = GetChunk(x, y, z);

        if (chunk != null) {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            if (block.GetType() == typeof(BlockWater))
                block.RegisterForUpdate();
            chunk.update = true;

            //update the neighbor chunks
            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));

        }
    }

    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2) {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk.update = true;
        }
    }

}
