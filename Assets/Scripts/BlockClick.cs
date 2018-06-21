using UnityEngine;
using System.Collections;

public class BlockClick : MonoBehaviour
{
    public World world;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo)) {
                int x = Mathf.CeilToInt(hitInfo.point.x);
                int y = Mathf.CeilToInt(hitInfo.point.y) - 1;
                int z = Mathf.CeilToInt(hitInfo.point.z);

                world.SetBlock(x, y, z, new BlockAir());
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo)) {
                int x = Mathf.CeilToInt(hitInfo.point.x);
                int y = Mathf.CeilToInt(hitInfo.point.y);
                int z = Mathf.CeilToInt(hitInfo.point.z);

                world.SetBlock(x, y, z, new BlockGrass());
            }
        }

        if (Input.GetMouseButtonUp(2)) {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo)) {
                int x = Mathf.CeilToInt(hitInfo.point.x);
                int y = Mathf.CeilToInt(hitInfo.point.y);
                int z = Mathf.CeilToInt(hitInfo.point.z);
                Block bl = world.GetBlock(x, y, z);
                Block downBl = world.GetBlock(x, y - 1, z);

                // Check to see if we got a water block                                                                      
                if (bl.GetType() == typeof(BlockAir) && downBl.GetType() != typeof(BlockWater)) {
                    // Set the new water block
                    world.SetBlock(x, y, z, new BlockWater());
                    // Get the block we just placed
                    BlockWater newBlock = world.GetBlock(x, y, z) as BlockWater;
                    newBlock.SetWorld(world, x, y, z);
                    // Add it to register for update
                    newBlock.RegisterForUpdate();
                }
            }
        }
    }
}
