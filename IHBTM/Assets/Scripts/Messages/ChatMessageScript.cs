using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatMessageScript : MonoBehaviour
{
    public GameObject ghost;
    private float speed = 10;
    private RectTransform rt;
    MessageTypes type;
    Image img;
    TextMeshProUGUI text;
    float xPos;
    DialogueManager dm;
    [SerializeField] private Sprite rSprite, sSprite;
    [SerializeField] private Color rColor, sColor;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        text = transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>();
        dm = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();

        type = GetComponent<MessageScript>().Type;

        //type = dm.dialogBlocks[dm.blockNum].lines[dm.lineNum].type;

        ContinueSetUpBox();
    }

    private void Update()
    {
        FollowGhost();
    }

    void ContinueSetUpBox()
    {
        switch (type)
        {
            case MessageTypes.timestamp:
                break;
            case MessageTypes.recieved:
                xPos = 10;
                TextMeshProUGUI rt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                rt.GetComponent<RectTransform>().anchoredPosition = new Vector2(3, 0);
                GetComponent<Image>().sprite = rSprite;
                rt.color = rColor;
                break;
            case MessageTypes.sent:
                xPos = 600;
                TextMeshProUGUI st = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                st.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5, 0);
                GetComponent<Image>().sprite = sSprite;
                st.color = sColor;
                break;
        }

    }

    void FollowGhost()
    {
        float targetY = ghost.GetComponent<RectTransform>().anchoredPosition.y;
        Vector3 targetPos = new Vector3(xPos, targetY, 0);
        rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, targetPos, Time.deltaTime * speed);
    }

}
