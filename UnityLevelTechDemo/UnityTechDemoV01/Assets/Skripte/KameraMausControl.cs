using UnityEngine;
using System.Collections;

public class KameraMausControl : MonoBehaviour {

	public const float YMin = 10.0f;
	public const float YMax = 60.0f;
	public Transform lookAt;
	public Transform camTransform;

	Camera cam;
	float distance = 10.0f;
	float currentX = 0.0f;
	float currentY = 0.0f;
	float sensivityX = 4.0f;
	float sensivityY = 1.0f;

	[System.Serializable]
	public class PositionsEinstellungen
	{
	public Vector3 zielPosAbstand = new Vector3(0, 3.4f, 0);
	public float lookSmooth = 100f;
	public float abstandVomZiel= -8;
	public float weicherZoom = 100;
	public float KameraHinterZiel = 0.0f;
	public const float maxZoom = 2;
	public const float minZoom = 100;
	

	}

	[System.Serializable]
	public class InputEinstellungen
	{
	public string ZOOM = "Mouse ScrollWheel";
	}

	public float zoomInput;
	public PositionsEinstellungen position = new PositionsEinstellungen();
	public InputEinstellungen input = new InputEinstellungen ();
	public float KameraHinterZiel = 0.0f;
	public float KameraBonusAbstand = 79f;
	public const float maxZoom = 2;
	public const float minZoom = 100;

			
	void Start () 
	{
		camTransform = transform;
		cam = Camera.main;
	}

	void Update()
	{
		GetInput ();
		currentX += Input.GetAxis ("Mouse X");
		currentY += Input.GetAxis ("Mouse Y");
		currentY = Mathf.Clamp (currentY, YMin, YMax);
		position.abstandVomZiel += zoomInput * position.weicherZoom * Time.deltaTime;
		KameraHinterZiel = Mathf.Clamp (KameraHinterZiel, minZoom, maxZoom); // Funzt noch net 

	}

	void LateUpdate () 
	{
		KameraHinterZiel = distance * position.abstandVomZiel + KameraBonusAbstand ; 
		Vector3 dir = new Vector3 (0, 0, -distance);
		Quaternion rotation = Quaternion.Euler (currentY, currentX, 0);
		camTransform.position = lookAt.position + rotation * dir * -KameraHinterZiel;
		camTransform.LookAt (lookAt.position);
	}

	void GetInput ()
	{
		zoomInput = Input.GetAxisRaw (input.ZOOM);
	}


}

