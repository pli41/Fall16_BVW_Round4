using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayMovie : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MovieTexture m = (MovieTexture)GetComponent<RawImage>().texture;
        m.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
