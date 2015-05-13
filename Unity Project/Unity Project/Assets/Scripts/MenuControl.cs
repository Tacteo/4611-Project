using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {

	public void changeScene(int sceneIndex) {
		Application.LoadLevel(sceneIndex);
	}

	public void quitGame() {
		Application.Quit ();
	}
	
	// Use this for initialization
	void Start () {
	
	}

}
