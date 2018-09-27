using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Face
{Baldwin=0, Beyonce, Burke, Colvin, Cox, Davis, Kaepernick, King, Mandela, Rupaul, Tubman, Woodson };


public class CardRotater : MonoBehaviour {
	private bool isFaceUp;
	public static bool inputAllowed { get; set; }

	private bool isAnimationProcessing;
	private float waitForSeconds;
	private int fps = 60;
	private float rotateDegreePerSecond = 540.0f;
	private const float FLIP_LIMIT = 180.0f;

	public Face person;
	private GameController controller;


	// Use this for initialization
	void Start () {
		//Find GameController for managing number of moves and score
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			controller = gameControllerObject.GetComponent <GameController> ();
		} else {
			Debug.Log ("Cannot find 'GameController' script");
		}

		//Initialize isFaceUp to false because cards are closed at the beginning
		isFaceUp = false;

		waitForSeconds = 1.00f / (float)fps;
	}


	void OnMouseUpAsButton() {
		if (!isFaceUp && !isAnimationProcessing && inputAllowed) {
			inputAllowed = false;
			StartCoroutine (Flip ());
			//StartCoroutine (ClickTimer ());
			StartCoroutine (controller.CardOpened (this.gameObject));
		}
	}


	public void CloseCard() {
		StartCoroutine(Flip ());
	}

	IEnumerator ClickTimer(){
		yield return new WaitForSeconds(1.0f);
	}

	IEnumerator Flip() {
		isAnimationProcessing = true;

		bool done = false;
		do {
			float degreeZ = rotateDegreePerSecond * waitForSeconds;
			if (isFaceUp) {
				degreeZ = -degreeZ;
			}

			transform.Rotate (new Vector3 (0.0f, 0.0f, degreeZ));
			if (this.transform.eulerAngles.z > FLIP_LIMIT) {
				transform.Rotate (new Vector3 (0.0f, 0.0f, -degreeZ));
				done = true;
			}
			yield return new WaitForSeconds (waitForSeconds);
		} while (!done) ;

		isFaceUp = !isFaceUp;
		isAnimationProcessing = false;
	}
}
