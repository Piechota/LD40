using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if ( Input.GetButtonUp("Jump"))
        {
            SceneManager.LoadScene("main", LoadSceneMode.Single);
        }
	}
}
