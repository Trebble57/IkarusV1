using UnityEngine;
using System.Collections;

public class CharacterControllerV03 : MonoBehaviour {

	[System.Serializable]
	public class Bewegungseinstellungen
	{
		public float Laufgeschwindigkeit = 12;
		public float Drehgeschwindigkeit = 100;
		public float Springgeschwindikeit = 25;
		public float AbstzuBoden = 1.1f;
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
	bool sprungtasteDown = false;

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

		}
	}

	void GetInputFlug ()
	{
		if (FlugModus)
		{
			
			DrehenInput = Input.GetAxis (inputeinstellungen.DREHEN_ACHSE);
			SpringenInput = Input.GetAxisRaw (inputeinstellungen.SPRINGEN_ACHSE);
		}
	}

	void Update () 
	{
		GetInput ();
		Drehen ();
		GetInputFlug ();
		sprungtasteDown = (SpringenInput > 0);
	}

	void FixedUpdate ()
	{
		Rennen ();
		Charakter.velocity = transform.TransformDirection (Geschwindigkeit);
		NeuesSpringen ();
	}

	void Rennen ()
	{
		if (Mathf.Abs (LaufenInput) > 0) 
		{
			
			Geschwindigkeit.z = bewegungseinstellungen.Laufgeschwindigkeit * LaufenInput;
		}
		else
			
			Geschwindigkeit.z = 0;
	}

	void Drehen ()
	{
		targetRotation *= Quaternion.AngleAxis (bewegungseinstellungen.Drehgeschwindigkeit * DrehenInput * Time.deltaTime, Vector3.up);
		transform.rotation = targetRotation;
	}


		void NeuesSpringen()
	{
		//Flugmodus = Fluganziehung aktiv , Erdanziehung inaktiv, nur linkrechtssteuerung und sprungtaste aktiv
		if (FlugModus) 
		{
			Geschwindigkeit.y -= physikeinstellungen.Fluganziehung;
		}
		//wenn nichtGrounded und sprungtaste und nichtFlugmodus
		// dann Flugmodus an
		if (!Grounded() && SpringenInput > 0 && !FlugModus && !sprungtasteDown) 
		{
			FlugModus = true;
		}

		// wenn Grounded und sprungtaste 
		//dann   Geschwindigkeit.y = bewegungseinstellungen.Springgeschwindikeit;
		if (SpringenInput > 0 && Grounded ()) 
		{
			Geschwindigkeit.y = bewegungseinstellungen.Springgeschwindikeit;
		} 
	
		//Wenn Grounded und SringenNein && FlugmodusJA
		//dann FlugmodusNein
		if (Grounded() && SpringenInput == 0 && FlugModus && !sprungtasteDown) 
		{
			FlugModus = false;
		}

		//wenn nichtGounded und Sprungtaste und Flugmodus
		//dann Flugmodus aus
		if (!Grounded() && SpringenInput > 0 && FlugModus) 
		{
			FlugModus = false;
		}
		// in allen anderen fällen
		//Erdanziehung aktiv
		else
		{
			Geschwindigkeit.y -= physikeinstellungen.Erdanziehung;
		}


	}

	
}
