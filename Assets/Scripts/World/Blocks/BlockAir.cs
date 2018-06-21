using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockAir : Block { //an empty block without faces and no solid faces
	
	//because this is just air, it returns the meshData as it receives them (no mesh data)
	public override MeshData BlockData (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		return meshData;
	}

	//air is not solid for any face
	public override bool IsSolid (Direction direction)
	{
		return false;
	}
}
