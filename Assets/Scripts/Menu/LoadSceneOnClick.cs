using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public void LoadByIndex(int sceneIndex)
	{
		if(!SceneManager.GetSceneByBuildIndex (sceneIndex).IsValid ())
			SceneManager.LoadScene (sceneIndex, LoadSceneMode.Additive);
		
		SceneManager.SetActiveScene (SceneManager.GetSceneByBuildIndex (sceneIndex));

	}

}
