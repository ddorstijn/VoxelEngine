using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class WorldPos {

	public int x, y, z;

	public WorldPos(int xPos, int yPos, int zPos) 
    {
		x = xPos;
		y = yPos;
		z = zPos;
	}

	public override bool Equals(object obj) 
    {
		bool retVal = false;

		if(!(obj is WorldPos)) 
        {
			retVal = false;
		} 
        else
        {
			WorldPos pos = (WorldPos)obj;
			if(pos.x != x || pos.y != y || pos.z != z)
            {
				retVal = false;
			} 
            else
            {
				retVal = true;
			}
		}

		return retVal;
	}

	public override int GetHashCode ()
	{
		int hash = 1;
		hash = hash * 17 + x;
		hash = hash * 31 + y;
		hash = hash * 13 + z;

		return hash;
	}

	public static WorldPos operator +(WorldPos w1, WorldPos w2) 
    {
		return new WorldPos(w1.x + w2.x, w1.y + w2.y, w1.z + w2.z);

	}


	public class WorldPosComparer : IComparer 
    {

		private WorldPos referencePos;

		public WorldPosComparer(WorldPos reference) {
			referencePos = reference;
		}

		public int Compare (object x, object y)
		{
			int retVal = 0;

			WorldPos posX = (WorldPos) x;
			WorldPos posY = (WorldPos) y;

			int distX = GetDistance(referencePos, posX);
			int distY = GetDistance(referencePos, posY);

			retVal = distX - distY;

			return retVal;
		}


		private int GetDistance(WorldPos posA, WorldPos posB) {
			int distX = Mathf.Abs(posA.x - posB.x);
			int distZ = Mathf.Abs(posA.z - posB.z);
			int distY = Mathf.Abs(posA.y - posB.y);
			
			if(distX > distZ) {
				return 14 * distZ + 10* (distX - distZ) + 10 * distY;
			}
			
			return 14 * distX + 10 * (distZ - distX) + 10 * distY;
			
		
		}

	}

	public static int UnWeightedDistance(WorldPos posA, WorldPos posB) 
    {

		int distX = Mathf.Abs(posA.x - posB.x);
		int distZ = Mathf.Abs(posA.z - posB.z);
		int distY = Mathf.Abs(posA.y - posB.y);

		return distX + distY + distZ;
	}
}
