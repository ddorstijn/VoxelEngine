using UnityEngine;
using System.Collections;

public class FlyCam : MonoBehaviour {

    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    public World world;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
            transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
        } else {
            transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * climbSpeed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.E)) { transform.position -= transform.up * climbSpeed * Time.deltaTime; }

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100)) {
                EditTerrain.SetBlock(hit, new BlockAir());
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 100)) {
                EditTerrain.SetBlock(hit, new BlockGrass(), true);
            }
        }

        if (Input.GetMouseButtonDown(2)) {

            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo)) {

                int x = Mathf.CeilToInt(hitInfo.point.x);
                int y = Mathf.CeilToInt(hitInfo.point.y);
                int z = Mathf.CeilToInt(hitInfo.point.z);
                Block bl = world.GetBlock(x, y, z);
                Block downBl = world.GetBlock(x, y - 1, z);

                //Check to see if we got a water block                                                                          ///---
                if (bl.GetType() == typeof(BlockAir) && downBl.GetType() != typeof(BlockWater)) {
                    //set the new water block
                    world.SetBlock(x, y, z, new BlockWater());
                    //get the block we just placed
                    BlockWater newBlock = world.GetBlock(x, y, z) as BlockWater;
                    newBlock.SetWorld(world, x, y, z);
                    //and add it to register for update
                    newBlock.RegisterForUpdate();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}