using UnityEngine;
using System.Collections;

public class KameraControlV2 : MonoBehaviour {

	[System.Serializable]
	public class PositionsEinstellungen
	{
		public Vector3 zielPosAbstand = new Vector3(0, 3.4f, 0);
		public float lookSmooth = 100f;
		public float abstandVomZiel= -8;
		public float weicherZoom = 100;
		public float maxZoom = -2;
		public float minZoom = -15;
	}

	[System.Serializable]
	public class OrbitEinstellungen
	{
		public float xDrehung = -20;
		public float yDrehung = -180;
		public float maxXDrehung = 25;
		public float minXDrehung = -85;
		public float vertikalerWeicherOrbit = 150;
		public float horizontalerWeicherOrbit = 150;
	}

	[System.Serializable]
	public class InputEinstellungen
	{

		public string ORBIT_HORIZONTALER_SNAP = "OrbitHorizontalSnap";
		public string ORBIT_HORIZONTAL = "OrbitHorizontal";
		public string ORBIT_VERTICAL = "OrbitVertical";
		public string ZOOM = "Mouse ScrollWheel";

	}
		
	public PositionsEinstellungen position = new PositionsEinstellungen();
	public OrbitEinstellungen orbit = new OrbitEinstellungen();
	public InputEinstellungen input = new InputEinstellungen();

	Vector3 zielPos = Vector3.zero;
	Vector3 destination = Vector3.zero;
	CharakterController charakter;
	float verticalOrbitInput, horizontalOrbitInput, zoomInput, horizontalerSnapInput;

	public Transform ziel;
	/*public float lookSmooth = 0.09f;
	public float xDrehung;
	public float yDrehung;*/





	void Start ()
	{
		SetCameraTarget (ziel);

		zielPos = ziel.position + position.zielPosAbstand;
		destination = Quaternion.Euler (orbit.xDrehung, orbit.yDrehung + ziel.eulerAngles.y, 0) * Vector3.forward * position.abstandVomZiel;
		destination += zielPos;
		transform.position = destination;
	}
	

	void SetCameraTarget (Transform t) 
	{

		ziel = t;
		charakter = ziel.GetComponent<CharakterController>();

		zielPos = ziel.position + position.zielPosAbstand;
		destination = Quaternion.Euler (orbit.xDrehung, orbit.yDrehung + ziel.eulerAngles.y, 0) * -Vector3.forward * position.abstandVomZiel; 
		destination += zielPos;
		transform.position = destination;

		}

		void GetInput()
		{
		verticalOrbitInput = Input.GetAxis (input.ORBIT_VERTICAL);
		horizontalOrbitInput = Input.GetAxis (input.ORBIT_HORIZONTAL);
		horizontalerSnapInput = Input.GetAxisRaw (input.ORBIT_HORIZONTALER_SNAP);
		zoomInput = Input.GetAxis (input.ZOOM);
		}

		void Update()
		{
			GetInput ();
			OrbitTarget ();
			ZoomInOnTarget ();
		}


	void LateUpdate()
	{
		MoveToTarget();
		LookAtTarget();

		//moving rotating
	}

	void MoveToTarget()
	{
			zielPos = ziel.position + position.zielPosAbstand;
		destination = Quaternion.Euler (orbit.xDrehung, orbit.yDrehung, 0) * Vector3.forward * position.abstandVomZiel;
		destination += zielPos;
		transform.position = destination;
	}

	void LookAtTarget()
	{
		Quaternion zielDrehung = Quaternion.LookRotation (zielPos - transform.position);
		transform.rotation = Quaternion.Lerp (transform.rotation, zielDrehung, position.lookSmooth * Time.deltaTime);
	}

		void OrbitTarget()
		{
		if (horizontalerSnapInput > 0) {
			orbit.yDrehung = -180;
		}
		orbit.xDrehung += -verticalOrbitInput * orbit.vertikalerWeicherOrbit * Time.deltaTime;
		orbit.yDrehung += -horizontalOrbitInput * orbit.horizontalerWeicherOrbit * Time.deltaTime;
			
		if (orbit.xDrehung > orbit.maxXDrehung) 
			{
			orbit.xDrehung = orbit.maxXDrehung;
			}

			if (orbit.xDrehung < orbit.minXDrehung) 
			{
			orbit.xDrehung = orbit.minXDrehung;
			}


		}

		void ZoomInOnTarget()
		{
		position.abstandVomZiel += zoomInput * position.weicherZoom * Time.deltaTime;

		if (position.abstandVomZiel > position.maxZoom) 
		{
			position.abstandVomZiel = position.maxZoom;
		}

		if (position.abstandVomZiel < position.minZoom) 
			{
			position.abstandVomZiel = position.minZoom;
			}
		}
	}
