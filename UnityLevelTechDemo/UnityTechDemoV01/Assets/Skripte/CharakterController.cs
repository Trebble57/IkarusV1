using UnityEngine;
using System.Collections;

public class CharakterController : MonoBehaviour {

	[System.Serializable]
	public class Bewegungseinstellungen
	{
		public float Laufgeschwindigkeit = 12;
		public float Drehgeschwindigkeit = 100;
		public float Springgeschwindigkeit = 50;
		public float AbstzuBoden = 0.5f;
		public LayerMask Boden;

	}

	[System.Serializable]
	public class Physikseinstellungen
	{
		public float Erdanziehung = 1.5f;
		public float Fluganziehung = 0.2f;
	}

	[System.Serializable]
	public class Inputeinstellungen
	{
		public string VORNE_ACHSE = "Vertical";
		public string DREHEN_ACHSE = "Horizontal";
		public string SPRINGEN_ACHSE = "Jump";
		public string FLIEGEN_ACHSE = "Fly";
	}

	public Bewegungseinstellungen bewegungseinstellungen = new Bewegungseinstellungen ();
	public Physikseinstellungen physikseinstellungen = new Physikseinstellungen ();
	public Inputeinstellungen inputeinstellungen = new Inputeinstellungen ();


	Vector3 Geschwindigkeit = Vector3.zero;
	Quaternion targetRotation;
	Rigidbody Charakter;
	public bool FlugModus;

	float LaufenInput, DrehenInput, SpringenInput, FliegenInput;
	//GameObject Rigidbody;
	//public Animator Charakteranimator;
	//bool CharakterIsGrounded;
	//Vector3 CharakterGroundedNormal;



	public Quaternion TargetRotation 

	{
		get 
		{ 
			return targetRotation;
		}
	}

	bool Grounded()
	{
		return Physics.Raycast (transform.position, Vector3.down, bewegungseinstellungen.AbstzuBoden, bewegungseinstellungen.Boden);
	}

	void Start ()
	{
			targetRotation = transform.rotation;
			Charakter = GetComponent<Rigidbody>();
			//Charakteranimator = GetComponent<Animator>();
		LaufenInput = DrehenInput = SpringenInput = FliegenInput = 0;
		FlugModus = false;


	}

	void GetInput()
	{
		if (!FlugModus) 
		{
			LaufenInput = Input.GetAxis (inputeinstellungen.VORNE_ACHSE);
			DrehenInput = Input.GetAxis (inputeinstellungen.DREHEN_ACHSE);
			SpringenInput = Input.GetAxisRaw (inputeinstellungen.SPRINGEN_ACHSE);
			FliegenInput = Input.GetAxis (inputeinstellungen.FLIEGEN_ACHSE);
		}

	}

	void GetInputFlug()

	{
		if (FlugModus) {
			FliegenInput = Input.GetAxis (inputeinstellungen.FLIEGEN_ACHSE);
			DrehenInput = Input.GetAxis (inputeinstellungen.DREHEN_ACHSE);
		}
	}
		
	void Update()
	{
		GetInput ();
		Drehen ();


	}

 	void FixedUpdate()
	{
		Rennen ();
		Springen ();
		Charakter.velocity = transform.TransformDirection (Geschwindigkeit);
	}

	void Rennen()
	{
		if (Mathf.Abs (LaufenInput) > 0) 
		{
			Geschwindigkeit.z = bewegungseinstellungen.Laufgeschwindigkeit * LaufenInput;
		}

		else
		{	
			Geschwindigkeit.z = 0;
		}		
	}
		void Drehen()
	{
		targetRotation *= Quaternion.AngleAxis(bewegungseinstellungen.Drehgeschwindigkeit * DrehenInput * Time.deltaTime, Vector3.up);
		transform.rotation = targetRotation;		
	}
	void Gravitation()
	{
		Physics.gravity = new Vector3 (0, -10.0F, 0);
	}	

	void Springen()
	{
		if (SpringenInput > 0 && Grounded()) 
		{
			Geschwindigkeit.y = bewegungseinstellungen.Springgeschwindigkeit;
		}

		if (FliegenInput > 0 && LaufenInput > 0 && !Grounded())
		{
			Geschwindigkeit.y = physikseinstellungen.Fluganziehung;
			FlugModus = true;
		}

		if (FliegenInput > 0 && !Grounded() && FlugModus) //Hier FLugmodus true ? 
 
		{
			Geschwindigkeit.y -= physikseinstellungen.Erdanziehung;
			FlugModus = false;
		}

		if (Grounded())

		{
			FlugModus = false;
		}


		else if (SpringenInput == 0 && Grounded ())
		{
			Geschwindigkeit.y = 0;
		}

		else 
		{
			Geschwindigkeit.y -= physikseinstellungen.Erdanziehung;
		}
	}

	/*void Flugmodus ()
	{
		if (FlugModus = true) 
		{
			GetInput.enabled = false;
		}
	}*/




	/*void CheckIfGrounded()
	{
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position + (Vector3.up * 0.2F), Vector3.down, out hitInfo, CharakterIsGrounded)) 
		{
			CharakterGroundedNormal = hitInfo.normal;
			CharakterIsGrounded = true;
			Charakteranimator.applyRootMotion = true;
		}

		else
		{
			CharakterIsGrounded = false;
			Charakteranimator.applyRootMotion = false;
			CharakterGroundedNormal = Vector3.up; //Keine Ahnung warum diese Zeile funktioniert
		}

	}

	/*void Parachute()
	{
		if (CharakterIsGrounded = false, hier springenInput) 
		{
			Hier Fallschirm "aktivieren" -meshrenderer aktivieren? -aircontrol activieren -schwerkraft senken
		}
		if
		{

		}
		else
		{
			springen
		}
	} */

	
}
