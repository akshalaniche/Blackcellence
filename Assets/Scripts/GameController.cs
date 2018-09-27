using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameController : MonoBehaviour {
	private bool secondFlip;
	private GameObject cardFlipped1;
	private CardRotater card1;
	[SerializeField] GameObject button;

	[SerializeField]
	private int numberOfPairs;
	private int numCardsLeft;

	public GUIText movesText;
	private int movesLeft;

	public GUIText endText;

	//number of possible cards (prefabs available for cards)
	private static int NUM_DIFF_CARDS = Enum.GetNames (typeof(Face)).Length;
	//Array of prefabs, to allow instantiation
	public GameObject[] typeOfCards = new GameObject[NUM_DIFF_CARDS];

	//number of rows and columns of cards
	public int numberRows, numberColumns;

	//distance between two neighbouring cards in the x direction and the z direction
	public float xDistance, zDistance;

	//position of top left most card
	[SerializeField]
	private Vector3 initialPos;

	// Use this for initialization
	void Start () {
		CardRotater.inputAllowed = true;
		secondFlip = false;
		cardFlipped1 = null;

		movesLeft = 20;
		UpdateMoves ();

		endText.text = "";

		numCardsLeft = 2 * numberOfPairs;

		//Shuffle array of indeces, so we can choose the card pairs randomly
		int[] peopleToSelectFrom = new int[NUM_DIFF_CARDS];
		for (int i = 0; i < NUM_DIFF_CARDS; i++)
			peopleToSelectFrom [i] = i;
		Shuffle (peopleToSelectFrom);

		//Create array that represents a deck of cards composed of the pairs chosen
		int[] deckOfCards = new int[2 * numberOfPairs];
		for (int i = 0; i < numberOfPairs; i++) {
			deckOfCards [2 * i] = peopleToSelectFrom [i];
			deckOfCards [2 * i + 1] = peopleToSelectFrom [i];
		}
			
		//Shuffle deck of cards
		Shuffle (deckOfCards);

		//position of card i
		Vector3 currentPos;
		int counter = 0; //counter of position in deck

		//Instantiate array of cards on screen
		for (int i = 0; i < numberColumns; i++) {
			for (int j = 0; j < numberRows; j++) {
				//compute position at which to instantiate the card
				GameObject card = typeOfCards [deckOfCards [counter]];
				currentPos = initialPos + Vector3.right * xDistance * i - Vector3.forward * zDistance * j;
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (card, currentPos, spawnRotation);
				counter++;
			}
		}
	}

	public IEnumerator CardOpened(GameObject cardFlipped) {
		CardRotater card2 = cardFlipped.GetComponent<CardRotater> ();
		Face person2 = card2.person;

		if (secondFlip) {
			Face person1 = card1.person;

			//This is the second flip, so we compare the two cards to see if they form a pair
			movesLeft = movesLeft - 1;
			UpdateMoves ();

			if (person1 == person2) {
				yield return new WaitForSeconds (0.75f);

				Destroy (cardFlipped1);
				Destroy (cardFlipped);
				numCardsLeft = numCardsLeft - 2;

			//	yield return new WaitForSeconds (1.0f);

				//win
				if (numCardsLeft <= 0) {
					UpdateEndText("Congratulations!\nYou won!");
				}

			} else {
				yield return new WaitForSeconds (0.5f);

				card1.CloseCard ();		
				yield return new WaitForSeconds (0.5f);
				card2.CloseCard ();
			}

			cardFlipped1 = null;
			secondFlip = false;
			CardRotater.inputAllowed = true;

			//Check if game lost
			if (movesLeft == 0) {
				GameObject[] remainingCards = GameObject.FindGameObjectsWithTag ("Card");

				foreach (GameObject card in remainingCards) {
					Destroy (card);
				}

				UpdateEndText("Game over!\nSorry, you lost");
			}
		}

		else {
			yield return new WaitForSeconds (0.5f);
			card1 = card2;
			cardFlipped1 = cardFlipped;
			secondFlip = true;
			CardRotater.inputAllowed = true;
		}
	}

	void UpdateMoves() {
		movesText.text = "Moves left:\n" + movesLeft;
	}

	void UpdateEndText (String message){
		endText.text = message;
		button.SetActive (true);
	}

	//implementation of the Fischer-Yates shuffling algorithm
	void Shuffle (int[] arr) {
		System.Random rnd = new System.Random();
		for (int i = arr.Length; i > 1; i--) {
			int pos = rnd.Next(i);
			var x = arr[i - 1];
			arr[i - 1] = arr[pos];
			arr[pos] = x;
		}
	}
}
