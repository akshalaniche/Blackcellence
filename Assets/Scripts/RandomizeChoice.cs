using System;
using UnityEngine;
using TMPro;

public class RandomizeChoice : MonoBehaviour {
	
	private static int numberOfCards = (Enum.GetNames(typeof(Face))).Length;
	[SerializeField] private GameObject[] cards = new GameObject[numberOfCards];
	[SerializeField] string[] messages = new string[numberOfCards];

	[SerializeField] TextMeshProUGUI accomplishment;

	// Use this for initialization
	void Start () {
		System.Random rand = new System.Random ();
		int choice = rand.Next (numberOfCards);
		GameObject card = cards [choice];
		String message = messages [choice];

		Instantiate (card, this.transform);
		accomplishment.text = message;
	}
}
