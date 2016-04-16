using UnityEngine;
using System.Collections;

public class KameraControl : MonoBehaviour {

	public Transform ziel;
	public float lookSmooth = 0.09f;
	public Vector3 offsetFromTarget = new Vector3(0,-6,-8);
	public float xTilt = 10;

	Vector3 destination = Vector3.zero;
	CharakterController Charakter;
	float rotateVel = 0;

	void Start () {
		SetCameraTarget (ziel);
	}
	

	void SetCameraTarget (Transform t) {

		ziel = t;
		Charakter = ziel.GetComponent<CharakterController>();

		}


	void LateUpdate(){
		MoveToTarget();
		LookAtTarget();

		//moving rotating
	}

	void MoveToTarget()
	{
		destination = Charakter.TargetRotation * offsetFromTarget;
		destination += ziel.position;
		transform.position = destination;
	}

	void LookAtTarget()
	{
		float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, ziel.eulerAngles.y, ref rotateVel, lookSmooth);
		transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, 0);
	}
}
