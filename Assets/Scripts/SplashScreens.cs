using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashScreens : MonoBehaviour
{

    public AnimationCurve curve;
    public Gradient gradient;

    public float delay;

    private Image image;
    private float startTime;

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = gradient.Evaluate(curve.Evaluate(Time.time - startTime - delay));
    }
}
