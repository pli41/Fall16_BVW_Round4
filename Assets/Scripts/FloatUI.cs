using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatUI : MonoBehaviour {

    [SerializeField]
    Transform target;

    [SerializeField]
    Vector3 offset;

    [SerializeField]
    Vector2 offset_viewport;

    [SerializeField]
    MaskableGraphic[] UIElements;

    [SerializeField]
    Transform camera;

    [SerializeField]
    float stayTime;

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    Image bubbleImgae;

    [SerializeField]
    Sprite[] bubbleImages;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Activate(ThoughtTrigger bubble)
    {

        SetUIElements(true);
        bubbleImgae.sprite = PickImage(bubble.imageName);
        Invoke("Deactivate", stayTime);
        StartCoroutine("FollowTarget");
    }

    public void Deactivate()
    {
        SetUIElements(false);
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
            pos += offset_viewport;
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

    Sprite PickImage(string imageName)
    {
        Sprite sprite = null;
        switch (imageName)
        {
            case "box":
                sprite = bubbleImages[0];
                break;
            case "dumpster":
                sprite = bubbleImages[1];
                break;
            case "ledge":
                sprite = bubbleImages[2];
                break;
            case "onthewire":
                sprite = bubbleImages[3];
                break;
        }
        return sprite;
    }
}
