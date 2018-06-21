using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour {
	public static int chunkSize = 16;

	public World world;
	public WorldPos pos;

	[HideInInspector]
	public bool update = false;

    // Array of all the blocks of the chunk
    public Block[ , , ] blocks = new Block[chunkSize, chunkSize, chunkSize];

	private MeshFilter filter;
	private MeshCollider coll;
                                                                                     
    // List of blocks with logic
    public List<Block> TickBlocks = new List<Block>();

	// Use this for initialization
	void Start ()
    {
		filter = gameObject.GetComponent<MeshFilter>();
		coll = gameObject.GetComponent<MeshCollider>();		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(update) {
            // Update chuncks
            update = false;
			UpdateChunk();
		}

        // for every block into our List, run it's logic                                                  
        for (int i = 0; i < TickBlocks.Count; i++) {
            TickBlocks[i].Tick(Time.deltaTime);
        }
	}

    // Return the block in the given position
    public Block GetBlock(int x, int y, int z)
    {
		Block retBlock = null;
		if(InRange(x) && InRange(y) && InRange(z)) {
            // If the requested block is in this chunk, return it
            retBlock = blocks[x, y, z];
		} else {
            // Otherwise, get it from the world
            retBlock = world.GetBlock(pos.x + x, pos.y + y, pos.z + z);
		}

		return retBlock;
	}

    // Set the block in the given position
    public void SetBlock(int x, int y, int z, Block block)
    { 
		if(InRange(x) && InRange(y) && InRange(z)) { 
			blocks[x, y, z] = block;
		} else { 
			world.SetBlock(pos.x + x, pos.y + y, pos.z + z, block);
		}
	}

    // Update the chunk mesh data with the block meshdata
    void UpdateChunk()
    { 
		MeshData meshData = new MeshData();

		for (int x = 0; x < chunkSize; x++) {
			for (int y = 0; y < chunkSize; y++) {
				for (int z = 0; z < chunkSize; z++) {
					meshData = blocks[x, y, z].BlockData(this, x, y, z, meshData);
				}
			}
		}

		RenderMesh(meshData);
	}

	// Sets the mesh to be rendered (uvs, materials, normals)
	void RenderMesh(MeshData meshData)
    {
		filter.mesh.Clear();
		filter.mesh.subMeshCount = meshData.triangles.Count;
		filter.mesh.vertices = meshData.vertices.ToArray();

        for (int i=0; i<filter.mesh.subMeshCount; i++) {
			filter.mesh.SetTriangles(meshData.triangles[i].triangles.ToArray(), i);
		}

		filter.mesh.uv = meshData.uv.ToArray();
		filter.mesh.RecalculateNormals();
		coll.sharedMesh = null;

		Mesh mesh = new Mesh();
		mesh.vertices = meshData.colVertices.ToArray();
		mesh.SetTriangles(meshData.colTriangles.ToArray(), 0);
		mesh.RecalculateNormals();

        coll.sharedMesh = mesh;
	}

    // Set all blocks to unchanged
	public void SetBlockUnmodified()
    {
		foreach (Block block in blocks) {
			if (block != null) {
				block.changed = false;
			}
		}
	}
	
	// Check block in chunk
	public static bool InRange(int index)
    {
		if(index < 0 || index >= chunkSize) {
			return false;
		}

		return true;
	}
}
