using UnityEngine;
using System.Collections;
using UsefulThings;

public class MusicController : MonoBehaviour {

    static int GameState;

	// Use this for initialization
	void Start () {
        GameState = 0;
        SfxManager.PlayLoop(3);
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        StartCoroutine(FadeLoops(3 + GameState, 4 + GameState, 15));
        GameState++;
    }

    private IEnumerator FadeLoops(int a, int b, float time)
    {
        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            float t = (Time.time - startTime) / time;
            SfxManager.PlayLoop(a, Mathf.Lerp(1, 0, t));
            SfxManager.PlayLoop(b, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        SfxManager.PlayLoop(b);
        SfxManager.StopLoop(a);
    }
}
