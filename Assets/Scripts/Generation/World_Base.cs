using UnityEngine;
using System.Collections;
using SimplexNoise;

public class World_Base : MonoBehaviour
{
    [SerializeField]
    TerrainSettings terrainSettings;
    [System.Serializable]
    public class TerrainSettings
    {
        public float baseNoise = 0.02f; //noise frequency
        public float baseHeight = -5; //the base height we will start adding noise to
        public float baseNoiseHeight = 4; //the second layer of noise we will be adding
        public float elevation = 15;//how far will the elevation of the world go
        public float frequency = 0.005f; //the freguency of an elevation
    }

    //our loop that creates our chunks per collumns
    public Chunk Generate_Chunk(Chunk chunk)
    {
        for (int x = chunk.pos.x; x < chunk.pos.x + Chunk.chunkSize; x++)
        {
            for (int z = chunk.pos.z; z < chunk.pos.z + Chunk.chunkSize; z++)
            {
                chunk = Generate_Chunk_Collumn(chunk, x, z);
            }
        }
        return chunk;
    }

    //This is going to return us the noise from our script
    int GetNoise(int x, int y, int z, float scale, int max)
    {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }

    Chunk Generate_Chunk_Collumn(Chunk chunk, int x, int z)
    {
        //Find our height
        int Height = Mathf.FloorToInt(terrainSettings.baseHeight);

        //Apply elevation based on frequec
        Height += GetNoise(x, 0, z, terrainSettings.frequency, Mathf.FloorToInt(terrainSettings.elevation));

        //Apply noise, this can be skipped for smoother results
        Height += GetNoise(x, 0, z, terrainSettings.baseNoise, Mathf.FloorToInt(terrainSettings.baseNoiseHeight));

        //we take each block from each chunk...
        for (int y = chunk.pos.y; y < chunk.pos.y + Chunk.chunkSize; y++)
        {              
            //and compare it's height
            if (y <= Height)
            {
                //if it's lower than the height we produced in our noise script, then it's grass...
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockGrass());
            }
            else
            {   //if not, then it's air
                chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, new BlockAir());
            }

        }
        return chunk;        
    }
}
