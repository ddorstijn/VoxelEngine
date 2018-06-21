using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {

	public float moveSpeed = 10f;
	public float rotationSpeed = 10;

	void Start () {

	}
	
	
	void Update () {
		Quaternion rotationY = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
		Vector3 forwardVector = rotationY * Vector3.forward;
		Vector3 rightVector = rotationY * Vector3.right;


		if ( Input.GetMouseButton( 1 ) ) {
			transform.Translate(rightVector * Time.deltaTime * moveSpeed * (Input.mousePosition.x - Screen.width * 0.5f) / (Screen.width * 0.5f), Space.World);
			transform.Translate(forwardVector * Time.deltaTime * moveSpeed * (Input.mousePosition.y - Screen.height * 0.5f) / (Screen.height * 0.5f), Space.World);
		}

		if(Input.GetKey(KeyCode.W)) {
			transform.Translate(forwardVector * Time.deltaTime * moveSpeed, Space.World);
		}

		if(Input.GetKey(KeyCode.S)) {
			transform.Translate(forwardVector * Time.deltaTime * -moveSpeed, Space.World);
		}

		if(Input.GetKey(KeyCode.D)) {
			transform.Translate(rightVector * Time.deltaTime * moveSpeed, Space.World);
		}

		if(Input.GetKey(KeyCode.A)) {
			transform.Translate(rightVector * Time.deltaTime * -moveSpeed, Space.World);
		}

		if(Input.GetKey(KeyCode.Q)) {
			transform.Rotate(Vector3.up * Time.deltaTime * -rotationSpeed, Space.World);
		}

		if(Input.GetKey(KeyCode.E)) {
			transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
		}
		

	}
}
