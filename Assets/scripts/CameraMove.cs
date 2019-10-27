using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	int speed = 20;
	float minHeigth = 5f;
	float maxHeigth;
	float k1, k2, k3, k4, b;


	float leftRestriction;
	float rightRestriction;
	float upRestriction;
	float downRestriction;

	public void setRestrictions(float left, float right, float up, float down){
		leftRestriction = left;
		rightRestriction = right;
		upRestriction = up;
		downRestriction = down;

		if (MapInfo.current.gridWidth > MapInfo.current.gridHeigth) {
			maxHeigth = MapInfo.current.gridHeigth * 133/ (upRestriction);
		} else {
			maxHeigth = MapInfo.current.gridWidth * 133/ (leftRestriction);
		}

		k1 = maxHeigth / rightRestriction;
		k2 = maxHeigth / upRestriction;
		k3 = maxHeigth / leftRestriction;
		k4 = maxHeigth / down;

		b = maxHeigth + 10;

	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (leftRestriction + " " + rightRestriction + " " + upRestriction + " " + downRestriction);
		if ((transform.position.z <= leftRestriction) && ((int)Input.mousePosition.x < 2))
			transform.position -= transform.right * Time.deltaTime * speed;
		
		if ((transform.position.z >= rightRestriction) && (int)Input.mousePosition.x > Screen.width - 2)
			transform.position += transform.right * Time.deltaTime * speed;
		
		if ((transform.position.x <= upRestriction) && Input.mousePosition.y > Screen.height - 2)
			transform.position += transform.forward * Time.deltaTime * speed;
		
		if ((transform.position.x >= downRestriction) && Input.mousePosition.y < 2)
			transform.position -= transform.forward * Time.deltaTime * speed;

		checkHeigth ();

		if (transform.position.z > leftRestriction)
			transform.position = new Vector3(transform.position.x,transform.position.y,leftRestriction);

		if (transform.position.z < rightRestriction)
			transform.position = new Vector3(transform.position.x,transform.position.y,rightRestriction);

		if (transform.position.x > upRestriction)
			transform.position = new Vector3(upRestriction,transform.position.y,transform.position.z);
		
		if (transform.position.x < downRestriction)
			transform.position = new Vector3(downRestriction,transform.position.y,transform.position.z);

		if (Input.GetAxis ("Mouse ScrollWheel")>0 && transform.position.y > minHeigth) {
			transform.position = new Vector3(transform.position.x,transform.position.y-3,transform.position.z);
		}

		if (Input.GetAxis ("Mouse ScrollWheel")<0 && transform.position.y < maxHeigth) {
			transform.position = new Vector3(transform.position.x,transform.position.y+3,transform.position.z);
		}

		if (transform.position.y < minHeigth)
			transform.position = new Vector3 (transform.position.x, minHeigth, transform.position.z);

		if (transform.position.y > maxHeigth)
			transform.position = new Vector3 (transform.position.x, maxHeigth, transform.position.z);

	}

	void checkHeigth() {
		if (transform.position.y > k4 * transform.position.x + b)
			transform.position = new Vector3 ((transform.position.y - b) / k4, transform.position.y, transform.position.z);
	
		//if (transform.position.y > k2 * transform.position.x + b)
		//	transform.position = new Vector3 ((transform.position.y - b) / k2, transform.position.y, transform.position.z);

		if (transform.position.y > k3 * transform.position.z + b)
			transform.position = new Vector3 (transform.position.x, transform.position.y, (transform.position.y - b) / k3);

		if (transform.position.y > k1 * transform.position.z + b)
			transform.position = new Vector3 (transform.position.x, transform.position.y, (transform.position.y - b) / k1);
		
		}
}
