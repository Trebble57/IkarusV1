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
    bool springen = false;



    float LaufenInput, DrehenInput, SpringenInput;

	/*public Quaternion TargetRotation
	{
		get 
		{
			return targetRotation; 
		}
	}*/

	


	void Start () 
	{
		targetRotation = transform.rotation;
		Charakter = GetComponent<Rigidbody>();

		LaufenInput = DrehenInput = SpringenInput = 0;

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

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, bewegungseinstellungen.AbstzuBoden, bewegungseinstellungen.Boden);
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
        
		
	}

	void FixedUpdate ()
	{
		Rennen ();
        Charakter.velocity = transform.TransformDirection (Geschwindigkeit);  
		NeuesSpringen ();
        sprungtasteDown = (SpringenInput > 0);

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

        if (Grounded())
        {
             if (SpringenInput > 0)
            {
                Geschwindigkeit.y = bewegungseinstellungen.Springgeschwindikeit;
                springen = true;
            }

           

            else if (SpringenInput == 0 && FlugModus)
            {
                Geschwindigkeit.y -= physikeinstellungen.Erdanziehung;
                FlugModus = false;
            }

        }

        if (!Grounded())
        {
            if (springen)
            {
                Geschwindigkeit.y -= Time.deltaTime*100;
            }

            if (springen && Geschwindigkeit.y <= 0)
            {
                Geschwindigkeit.y = -physikeinstellungen.Erdanziehung;
                springen = false;
            }

            if (SpringenInput > 0 && !FlugModus && !sprungtasteDown  )
            {
                Geschwindigkeit.y = -physikeinstellungen.Fluganziehung;
                FlugModus = true;                                              
            }
            else if (SpringenInput > 0 && FlugModus && !sprungtasteDown )
            {
                Geschwindigkeit.y = -physikeinstellungen.Erdanziehung;
                FlugModus = false;                
            }

            /*else
            {
                Geschwindigkeit.y -= physikeinstellungen.Erdanziehung;
            }*/
        }
       
        
	}	
}