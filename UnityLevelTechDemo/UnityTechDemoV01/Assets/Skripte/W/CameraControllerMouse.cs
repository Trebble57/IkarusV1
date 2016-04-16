using UnityEngine;
using System.Collections;

public class CameraControllerMouse : MonoBehaviour {

	public const float Y_ANGLE_MIN = 10.0f;
	public const float Y_ANGLE_MAX = 50.0f;

	public Transform lookAt;
	public Transform camTransform;

	Camera cam;

	/*[System.Serializable]
	public class PositionsEinstellungen
	{
	public float lookSmooth = 100f;
	public float abstandVomZiel = -8;
	public float weicherZoom = 100;
	}

	[System.Serializable]
	public class InputEinstellungen
	{
		public string ZOOM = "Mouse ScrollWheel";
	}*/

	/*public PositionsEinstellungen position = new PositionsEinstellungen();
	public InputEinstellungen input = new InputEinstellungen();
	float zoomInput;
	public float KameraHinterZiel;
	public float KameraBonusAbstand = 79f;

	public float ZoomMin = -10;
	public float ZoomMax = 50;*/

	float distance = 15.0f;
	float currentX = 0.0f;
	float currentY = 0.0f;
	float sensivityX = 4.0f;
	float sensivityY = 1.0f;

	float zoomSpeed = 2.0f;

	void Start()
	{
		camTransform = transform;
		cam = Camera.main;
	}
		
	/*void GetInput()
	{
		zoomInput = Input.GetAxisRaw (input.ZOOM);

	}*/

	void Update()
	{
		currentX += Input.GetAxis ("Mouse X");
		currentY -= Input.GetAxis ("Mouse Y");

		currentY = Mathf.Clamp (currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		transform.Translate (0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);
		//GetInput ();
		//ZoomInOnTarget ();
	}

	void LateUpdate()
	{
		//KameraHinterZiel = distance * position.abstandVomZiel + KameraBonusAbstand;
		Vector3 dir = new Vector3 (0, 0, -distance);
		Quaternion rotation = Quaternion.Euler (currentY, currentX, 0);
		camTransform.position = lookAt.position + rotation * dir /* KameraHinterZiel*/ ;
		camTransform.LookAt (lookAt.position);
	}

	/*void ZoomInOnTarget()
	{
		position.abstandVomZiel += zoomInput * position.weicherZoom * Time.deltaTime;
	}*/
}

