using UnityEngine;
using System.Collections;

public class CharacterControllerV01 : MonoBehaviour {

	[System.Serializable]
	public class Bewegungseinstellungen
	{
		public float Laufgeschwindigkeit = 12;
		public float Drehgeschwindigkeit = 100;
		public float Springgeschwindikeit = 25;
		public float AbstzuBoden = 0.1f;
		public LayerMask Boden;
	}

	[System.Serializable]
	public class Physikeinstellungen
	{
		public float Erdanziehung = 1.5f;
		public float Fluganziehung = 0.2f;
	}

	[System.Serializable]
	public class Inputeinstellungen
	{
		public string LAUFEN_ACHSE = "Horizontal";
		public string DREHEN_ACHSE = "Vertical";
		public string SPRINGEN_ACHSE = "Jump";
		public string FLIEGEN_ACHSE = "Fly";
	}

	public Bewegungseinstellungen bewegungseinstellungen = new Bewegungseinstellungen ();
	public Physikeinstellungen physikeinstellungen = new Physikeinstellungen ();
	public Inputeinstellungen inputeinstellungen = new Inputeinstellungen ();

	Vector3 Geschwindigkeit = Vector3.zero;
	Quaternion targetRotation;
	Rigidbody Charakter;
	public bool FlugModus;

	float LaufenInput, DrehenInput, SpringenInput, FliegenInput;

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

		LaufenInput = DrehenInput = SpringenInput = FliegenInput = 0;

		FlugModus = false;
	}
	
	void GetInput ()
	{
		if (!FlugModus)
		{
			LaufenInput = Input.GetAxis (inputeinstellungen.LAUFEN_ACHSE);
			DrehenInput = Input.GetAxis (inputeinstellungen.DREHEN_ACHSE);
			SpringenInput = Input.GetAxisRaw (inputeinstellungen.SPRINGEN_ACHSE);
			FliegenInput = Input.GetAxis (inputeinstellungen.FLIEGEN_ACHSE);
		}
	}

	void GetInputFlug ()
	{
		if (FlugModus)
		{
			FliegenInput = Input.GetAxis (inputeinstellungen.FLIEGEN_ACHSE);
			DrehenInput = Input.GetAxis (inputeinstellungen.DREHEN_ACHSE);
		}
	}

	void Update () 
	{
		GetInput ();
		Drehen ();
		GetInputFlug ();
	}

	void FixedUpdate ()
	{
		Rennen ();
		Springen ();
		Charakter.velocity = transform.TransformDirection (Geschwindigkeit);
	}

	void Rennen ()
	{
		if (Mathf.Abs (LaufenInput) > 0) 
		{
			//Bewegen
			Geschwindigkeit.z = bewegungseinstellungen.Laufgeschwindigkeit * LaufenInput;
		}
		else
			//Stehen
			Geschwindigkeit.z = 0;
	}

	void Drehen ()
	{
		targetRotation *= Quaternion.AngleAxis (bewegungseinstellungen.Drehgeschwindigkeit * DrehenInput * Time.deltaTime, Vector3.up);
		transform.rotation = targetRotation;
	}

	void Springen ()
	{
		if (SpringenInput > 0 && Grounded ()) 
		{
			Geschwindigkeit.y = bewegungseinstellungen.Springgeschwindikeit;
		} 

		if (FliegenInput > 0 && LaufenInput > 0 && !Grounded ()) 
		{
			Geschwindigkeit.y = physikeinstellungen.Fluganziehung;
			FlugModus = true;
		}

		/*if (FliegenInput > 0 && !Grounded() && FlugModus)
		{
			Geschwindigkeit.y -= physikeinstellungen.Erdanziehung;
			FlugModus = false;
		}*/

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
			Geschwindigkeit.y -= physikeinstellungen.Erdanziehung;
		}
	}
}
