using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block
{
    // Reference to our world
    protected World world;
    
    // Coordinates
    protected int blockX;
    protected int blockY;
    protected int blockZ;

    const float tileSize = 0.25f;
    const int SelectedSubmesh = 1;

    public int defaultSubmesh = 0;

    // Submesh is used to define which material will be used for each block
    public int submesh = 0;
    public bool isWalkable = true;

    public bool changed = true;

    // Define the tile in the texture
    public struct Tile
    {
        public int x;
        public int y;
    }

    public Block()
    {
        submesh = defaultSubmesh;
    }
    
    // We need to store our world so that we can access it later
    public void SetWorld(World w, int blockX, int blockY, int blockZ)
    {
        world = w;
        this.blockX = blockX;
        this.blockY = blockY;
        this.blockZ = blockZ;
    }

    // The tick that we are going to override only through the water blocks
    public virtual void Tick(float deltaTime)
    {

    }

    // Put the block on a list that we are going to call the Tick from
    public virtual void RegisterForUpdate()
    {
        if (world != null) {
            world.GetChunk(blockX, blockY, blockZ).TickBlocks.Add(this);
        }
    }

    // Remove it from the list
    public virtual void UnRegisterForUpdate()
    {
        if (world != null) {
            world.GetChunk(blockX, blockY, blockZ).TickBlocks.Remove(this);
        }
    }

    // The position of the texture in the atlas, 
    // Direction is used to define the side of the block
    public virtual Tile TexturePosition(Direction direction)
    {                                                                
        Tile tile = new Tile
        {
            x = 0,
            y = 0
        };

        return tile;
    }

    // Get the UV's for the given side
    public virtual Vector2[] FaceUVs(Direction direction)
    { 
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = TexturePosition(direction);

        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize, tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x, tileSize * tilePos.y);

        return UVs;
    }

    // Write all the mesh data of the block
    public virtual MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.useRenderDataForCollision = true;

        // Check the neighbouring blocks if they are solid and if they are not, draws that face
        if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.Down)) {
            meshData = FaceDataUp(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.Up)) {
            meshData = FaceDataDown(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.South)) {
            meshData = FaceDataNorth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.North)) {
            meshData = FaceDataSouth(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.West)) {
            meshData = FaceDataEast(chunk, x, y, z, meshData);
        }

        if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.East)) {
            meshData = FaceDataWest(chunk, x, y, z, meshData);
        }

        return meshData;
    }

    // Check if a block is solid in a given direction
    public virtual bool IsSolid(Direction direction) {
        switch (direction) {
            case Direction.Down:
                return true;

            case Direction.East:
                return true;

            case Direction.North:
                return true;

            case Direction.South:
                return true;

            case Direction.Up:
                return true;

            case Direction.West:
                return true;
        }

        return false;
    }

    // Write the up face
    protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

        meshData.AddQuadTriangles(submesh);
        meshData.uv.AddRange(FaceUVs(Direction.Up));

        return meshData;
    }

    // Write the down face
    protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles(submesh);
        meshData.uv.AddRange(FaceUVs(Direction.Down));

        return meshData;
    }

    // Write the north face
    protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles(submesh);
        meshData.uv.AddRange(FaceUVs(Direction.North));

        return meshData;
    }

    // Draw the east face
    protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));

        meshData.AddQuadTriangles(submesh);
        meshData.uv.AddRange(FaceUVs(Direction.East));

        return meshData;
    }

    // Draw the west face
    protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles(submesh);
        meshData.uv.AddRange(FaceUVs(Direction.West));

        return meshData;
    }

    // Draw the south face
    protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData) {
        meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
        meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

        meshData.AddQuadTriangles(submesh);

        meshData.uv.AddRange(FaceUVs(Direction.South));

        return meshData;
    }

    // All the directions of one voxel
    public enum Direction
    {
        North,
        East,
        South,
        West,
        Up,
        Down
    };

}
