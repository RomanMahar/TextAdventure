using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TextController : MonoBehaviour {



	// Fetched gameobjects
	public Text MText;
	public Text AnswerText;
	public InputField AnswerInputField;
	public Text ControlText;
	public Image Spiral;

	// puzzle answer
	public string puzzleAnswer;
	public string FixAnswer;

	// winning condition threshold
	private bool northVisits;
	private bool eastVisits;
	private bool westVisits;
	private bool southVisits;
	private bool allVisits;

	// enumeration of states and singleStates
	public enum States {intro_0, intro_1, intro_2, chamber, north_wall, east_wall, south_wall, west_wall, door, escape_1, doom, wrong_answer};
	public States myState;

	// Used for dev & testing
//	private string ThisState;


	// visit counters
	int this_visit;
	int north_wall_visit;
	int south_wall_visit;
	int east_wall_visit;
	int west_wall_visit;
	int pillar_wall_visit;
	int door_wall_visit;
	int visits;

	

	// Use this for initialization
	void Start () {
		
		// Fetched GameObjects // in future, investigate how to use direct references
		MText = GameObject.Find("MText").GetComponent<Text>();
		AnswerText = GameObject.Find("AnswerText").GetComponent<Text>();
		ControlText = GameObject.Find("ControlText").GetComponent<Text>();
		AnswerInputField = GameObject.Find("InputField").GetComponent<InputField>();
		Spiral = GameObject.Find("Spiral").GetComponent<Image>();

		// init state
		myState = States.intro_0;

		// visit counters
		this_visit = 0;
		north_wall_visit = 0;
		south_wall_visit = 0;
		east_wall_visit = 0;
		west_wall_visit = 0;

		// winning visit requirements
		northVisits = false;
		westVisits = false;
		eastVisits = false;
		southVisits = false;
		allVisits = false;

		// image visibility
		Spiral.enabled = false;

		// in response to an unresolved bug. Field stays active after replay even if cleared
		clearAll();

		puzzleAnswer = "chamber";
	}
	 
	// Update is called once per frame
	void Update () {
			
		if 		(myState == States.intro_0) 		{intro_0();} 
		else if (myState == States.intro_1) 		{intro_1();} 
		else if (myState == States.intro_2) 		{intro_2();}
		else if (myState == States.chamber) 		{chamber();}
		else if (myState == States.north_wall) 		{north_wall();}
		else if (myState == States.east_wall) 		{east_wall();}
		else if (myState == States.south_wall) 		{south_wall();}
		else if (myState == States.west_wall) 		{west_wall();}
		else if (myState == States.door) 			{door();}
		else if (myState == States.escape_1) 		{escape_1();}
		else if (myState == States.wrong_answer) 	{wrong_answer();}
		else if (myState == States.doom) 			{doom();}

		// used for dev & testing
//		ThisState = myState.ToString();
	}

	// methods
	void checkNorth () 	{if (north_wall_visit >= 2) {northVisits = true;}}
	void checkEast () 	{if (east_wall_visit >= 2) 	{eastVisits = true;}}
	void checkSouth () 	{if (south_wall_visit >= 2) {southVisits = true;}}
	void checkWest () 	{if (west_wall_visit >= 2) 	{westVisits = true;}}
	void checkTotals () {checkNorth (); checkEast(); checkSouth(); checkWest(); 
			if (northVisits == true && eastVisits == true && westVisits == true && southVisits == true) {allVisits = true;}}
	
	void increment_state(ref int x) {;
		// integer. increment by 1. can also be written as: this_visit++;
		this_visit = this_visit + 1;
		x = this_visit;
//		OLD CODE. VERY VERBOSE
//		if 		(ThisState == "north_wall") {north_wall_visit = this_visit;}
//		else if (ThisState == "east_wall") {east_wall_visit = this_visit;}
//		else if (ThisState == "south_wall") {south_wall_visit = this_visit;}
//		else if (ThisState == "west_wall") {west_wall_visit = this_visit;}
		checkTotals();
	}

	public void checkAnswer (string Ans) {
		AnswerText = GameObject.Find("AnswerText").GetComponent<Text>();
		Ans = AnswerText.text;
		FixAnswer = Ans.ToLower();
		AnswerInputField.DeactivateInputField();
		if (FixAnswer == puzzleAnswer && allVisits == true) {myState = States.escape_1;}
		else if (FixAnswer == puzzleAnswer) {myState = States.doom;}
		else {myState = States.wrong_answer;}
	}

	void clearAll () {
		AnswerInputField.text = "";
		AnswerInputField.DeactivateInputField();
	}

	// state methods

	void intro_0 () {
		MText.text = "You awaken on cold stone floor. The side of your face is throbbing in pain. " +
					"You must have fallen on it. The coldness of stone soothes your burning cheek. " +
					"You pull your palms toward you sliding them across fragmented rock, " +
					"its grooves and edges scracthing against your skin, as you press yourself off the floor. " + 
					"With great effort you manage to stand. You're weak, hungry, thirsty. Your vision is blurry, " +
					"but slowly comes into focus...";
		ControlText.text = "Press Space key to continue";
		clearAll ();
		if (Input.GetKeyDown (KeyCode.Space)) {myState = States.intro_1;}
	}
	
	void intro_1 () {
		MText.text = "You stand near the centre of a large four-walled stone chamber. Close to you – at the the " +
					"very center – is a giant black pillar. Four smooth and almost reflective flat faces and extend " +
					"from floor to ceiling. One of its faces has a doorway. \n" +
					"Three of the chamber walls are fixed with stone tablets, each bearing an inscription. " +
					"The wall directly across from the pillar door has a crude drawing in place of a tablet.";

		ControlText.text = "Press Space key to continue";
		clearAll ();
		if (Input.GetKeyDown(KeyCode.Space)) {myState = States.intro_2;}
	}

	void intro_2 () {
		MText.text = "You stand near the center of the chamber. What would you like to do?";
		ControlText.text = "To examine the: \n" + 
							"\tdrawing wall, press '1'\n" +
							"\twall the the right of it, press '2'\n" +		
							"\twall the the left of it, press '3'\n" +
							"\tback wall, press '4'\n" +
							"\tdoor, press '5'\n";
		clearAll ();
		if 		(Input.GetKeyDown(KeyCode.Alpha1)) 	{myState = States.north_wall;}
		else if (Input.GetKeyDown(KeyCode.Alpha2)) 	{myState = States.east_wall;} 
		else if (Input.GetKeyDown(KeyCode.Alpha3)) 	{myState = States.west_wall;}
		else if (Input.GetKeyDown(KeyCode.Alpha4)) 	{myState = States.south_wall;}
		else if (Input.GetKeyDown(KeyCode.Alpha5)) 	{myState = States.door;} 
	}

	void chamber () {
		MText.text = "You stand near the center of the chamber. What would you like to do?";

		ControlText.text = "To examine the: \n" + 
							"\tdrawing wall, press '1'\n" +
							"\twall the the right of it, press '2'\n" +		
							"\twall the the left of it, press '3'\n" +
							"\tback wall, press '4'\n" +
							"\tdoor, press '5'\n";
		clearAll ();
		if 		(Input.GetKeyDown(KeyCode.Alpha1)) 	{myState = States.north_wall;}
		else if (Input.GetKeyDown(KeyCode.Alpha2)) 	{myState = States.east_wall;} 
		else if (Input.GetKeyDown(KeyCode.Alpha3)) 	{myState = States.west_wall;}
		else if (Input.GetKeyDown(KeyCode.Alpha4)) 	{myState = States.south_wall;}
		else if (Input.GetKeyDown(KeyCode.Alpha5)) 	{myState = States.door;} 
	}

	public void north_wall () {
		MText.text = "At the center of the bare stone wall is a two-ringed spiral, painted in blue. It encircles " +
			"a black square. At the end of the inner spiral, above the square is a glyph resembling a pair of legs with feet. " +
			"Above the drawing is an upwards pointing diamond shaped arrow superimposed over circle shape and cross. A compass perhaps?";
		// set this state visit value to whatever northwall visit is;
		this_visit = north_wall_visit;
		ControlText.text = "To go back, press 0\n " +
							"To show/hide the mural, press Tab";
		clearAll ();
		if (Input.GetKeyDown(KeyCode.Alpha0)) {myState = States.chamber; increment_state(ref north_wall_visit); Spiral.enabled = false;}
		else if (Spiral.enabled == false && Input.GetKeyDown(KeyCode.Tab)) {Spiral.enabled = true;}
		else if (Spiral.enabled == true && Input.GetKeyDown(KeyCode.Tab)) {Spiral.enabled = false;}
	}

	void east_wall () {
		MText.text = "A stone tablet reads: \n " +
						"You cannot fool me, but you can play me.";
		this_visit = east_wall_visit;
		ControlText.text = "To go back, press 0";
		if (Input.GetKeyDown(KeyCode.Alpha0)) {myState = States.chamber; increment_state(ref east_wall_visit);}}

	void west_wall () {
		MText.text = "A stone tablet reads: \n " +
						"You have seen me before and you will see me again";
		this_visit = west_wall_visit;
		ControlText.text = "To go back to chamber, press 0";
		if (Input.GetKeyDown(KeyCode.Alpha0)) {myState = States.chamber; increment_state(ref west_wall_visit);}	
	}

	void south_wall () {
		MText.text = "Seek control in the west; you may find escape or doom";
		this_visit = south_wall_visit;
		ControlText.text = "To go back, press 0";
		if (Input.GetKeyDown(KeyCode.Alpha0)) {myState = States.chamber; increment_state(ref south_wall_visit);}		
	}

	void door () {
		MText.text = "You see a plaque next to the door. \n It reads: Identify me.";
		ControlText.text = "To input the answer, press Tab\n" +
							"To submit the answer, press Return\n" +
							"To go back, press 0";
		if (Input.GetKeyDown(KeyCode.Tab)) {AnswerInputField.ActivateInputField(); AnswerInputField.Select();}
		else if (Input.GetKeyDown(KeyCode.Return)) {checkAnswer(FixAnswer);}
		else if (Input.GetKeyDown(KeyCode.Alpha0)) {myState = States.chamber; clearAll();}
	}

	void wrong_answer () {
		MText.text = "Nothing happens";
		ControlText.text = "To go back, press 0";
		if (Input.GetKeyDown(KeyCode.Alpha0)) {myState = States.door; clearAll ();}	
	}

	void doom () {
		MText.text = "You hear a click. The stone doors slide open revealing a small chamber that appears to be a " +
			"lift. You step inside. The door shuts. The lift rumbles.  \n" +
			"You hear a loud click. The floor under you shifts. \n " +
			"You fall to your doom.";
		
		ControlText.text = "To play again, press space.";
		clearAll();
		if (Input.GetKeyDown(KeyCode.Space)) {clearAll(); Start ();}		
	}

	// now escape_1 'corridor'
	void escape_1 () {
		MText.text = "You hear a click. The stone doors slide open revealing a small chamber that appears to be a " +
			"lift. You step inside. The door shuts. The lift rumbles.  \n " +
			"You feel an extra towards the ground as the lift shoots up. " +
			"\n \n " +
			"The door opens to a green grass field. \n " +
			"You have escaped.";

		
		ControlText.text = "To play again, press space.";
		if (Input.GetKeyDown(KeyCode.Space)) {clearAll(); Start ();}
		
	}
	
}
