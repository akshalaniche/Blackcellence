using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour {

	public string nextScene;
	public void NextScene()
	{
		SceneManager.LoadScene (nextScene, LoadSceneMode.Single);
	}
}
