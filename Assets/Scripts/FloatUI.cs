using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatUI : MonoBehaviour {

    [SerializeField]
    Transform target;

    [SerializeField]
    Vector2 offset_viewport;

    [SerializeField]
    MaskableGraphic[] UIElements;

    [SerializeField]
    float stayTime;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    Image bubbleImage;

    Vector2 currentOffset;
    string currentParam_anim;

    Animator imageAnim;


	// Use this for initialization
	void Start () {
        imageAnim = bubbleImage.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Activate(ThoughtTrigger zone)
    {
        Debug.Log("Activate");
        SetUIElements(true);
        currentParam_anim = zone.param_anim;
        imageAnim.SetBool(currentParam_anim, true);
        currentOffset = zone.imageOffset;
        Invoke("Deactivate", stayTime);
        StartCoroutine("FollowTarget");
    }

    public void Deactivate()
    {
        SetUIElements(false);
        imageAnim.SetBool(currentParam_anim, false);
        StopCoroutine("FollowTarget");
    }

    

    IEnumerator FollowTarget()
    {
        /*
        if (UIElements[0].canvas.renderMode != RenderMode.WorldSpace)
        {
            throw new UnityException("Canvas should be set to be rendered in world space.");
        }
        */

        if (!target)
        {
            throw new UnityException("Target is null");
        }

        while (true)
        {
            Vector2 pos;
            float width = canvas.GetComponent<RectTransform>().sizeDelta.x;
            float height = canvas.GetComponent<RectTransform>().sizeDelta.y;
            float x = Camera.main.WorldToScreenPoint(target.position).x / Screen.width;
            float y = Camera.main.WorldToScreenPoint(target.position).y / Screen.height;
            pos = new Vector2(width * x - width / 2, y * height - height / 2);
            pos += offset_viewport + currentOffset;
            GetComponent<RectTransform>().anchoredPosition = pos;

            yield return null;
        }
    }

    void SetUIElements(bool status)
    {
        foreach (MaskableGraphic element in UIElements)
        {
            element.enabled = status;
        }
    }

}
