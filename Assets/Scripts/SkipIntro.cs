using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SkipIntro : MonoBehaviour {

    public string gameSceneName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadSceneAsync(gameSceneName);
        }
    }
}
