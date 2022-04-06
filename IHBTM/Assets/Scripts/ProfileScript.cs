using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProfileScript : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 pos;
    private RectTransform rt;

    private InstagramManager im;
    [SerializeField] private Button backBtn;

    [SerializeField] private float speed;

    private bool isStarting;
    [HideInInspector] public bool isClosing;

    [SerializeField] GameObject personalFeed;
    [SerializeField] GameObject content;
    [SerializeField] GameObject post;

    private void Start()
    {
        im = GameObject.Find("Instagram").GetComponent<InstagramManager>();
        backBtn.onClick.AddListener(() => im.GoBackToFeed());

        rt = GetComponent<RectTransform>();

        startPos = rt.anchoredPosition;
        pos = startPos;

        isStarting = true;
    }

    IEnumerator StopSliding()
    {
        yield return new WaitForSeconds(1f);
        isStarting = false;
    }

    private void Update()
    {
        if (isStarting)
        {
            Slide(Vector3.zero);
        }

        if (isClosing)
        {
            Slide(startPos * 2);
        }
    }

    private void Slide(Vector3 targetPos)
    {
        pos = Vector3.Lerp(pos, targetPos, Time.deltaTime * speed);
        rt.anchoredPosition = pos;
    }

    public void OpenPersonalFeed(int index)
    {
        float yPos = post.GetComponent<RectTransform>().sizeDelta.y * index;
        content.GetComponent<RectTransform>().localPosition = new Vector2(0, yPos);
        RectTransform pfRT = personalFeed.GetComponent<RectTransform>();
        Vector3 targetPos = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition - new Vector2(pfRT.sizeDelta.x / 2, 0);        
        pfRT.anchoredPosition = targetPos;
        StartCoroutine(Activate());
    }

    public void ClosePersonalFeed()
    {
        personalFeed.GetComponent<ShowUp2>().isClosing = true;
        StartCoroutine(Disable());
    }

    IEnumerator Activate()
    {
        yield return new WaitForFixedUpdate();
        personalFeed.SetActive(true);
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(0.5f);
        personalFeed.SetActive(false);
    }
}
