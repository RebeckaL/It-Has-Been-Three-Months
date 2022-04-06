using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> dialogueManagers = new List<GameObject>();
    DialogueManager currentDM = null;

    [SerializeField] GameObject notice, clue, CRContent, phoneSize;
    string currentTag;

    [SerializeField] ScreenShotHandler ssh;
    RectTransform rt;

    List<GameObject> objectsOnScreen = new List<GameObject>();
    List<GameObject> capturedObjects = new List<GameObject>();

    [SerializeField] GameObject flash;

    [SerializeField] GameObject circle;
    [SerializeField] Canvas canvas;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    public void TakeScreenshot()
    {
        if (!flash.activeInHierarchy)
        {
            int index = -1;


            if(objectsOnScreen.Count > 0)
            {
                Transform objectTransform = objectsOnScreen[0].transform;
                Vector3 newPos = new Vector3(objectTransform.position.x, objectTransform.position.y, Camera.main.transform.position.z);
                RaycastHit2D hit = Physics2D.Raycast(newPos, Vector2.zero);
                if (hit.collider.gameObject == objectsOnScreen[0])
                {
                    index = objectsOnScreen[0].GetComponent<ClueScript>().Index;
                }

            }

            StartCoroutine(ssh.RecordFrame(index));
            StartCoroutine(TurnOnFlash());
        }
    }

    //a method for opening gates from other objects
    //ideally would have an string parable, but in order to be called by buttons that is not possible
    public void OpenGate(int i)
    {
        foreach(GameObject dm in dialogueManagers)
        {
            if (dm.CompareTag(currentTag))
            {
                currentDM = dm.GetComponent<DialogueManager>();
            }
        }

        if (currentDM.dialogBlocks[currentDM.blockNum + 1].type == DialogueBlock.BlockType.gate)
        {
            if (!currentDM.listOfConditions[i].isTrue)
            {
                currentDM.listOfConditions[i].isTrue = true;

                currentDM.ContinueAfterGate();
            }
        }
    }

    IEnumerator TurnOnFlash()
    {
        yield return new WaitForFixedUpdate();
        flash.SetActive(true);
    }

    public void SetCurrentTag(string tag)
    {
        currentTag = tag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trigger"))
        {
            GameObject trigger = collision.gameObject;
            Vector3 newPos = new Vector3(trigger.transform.position.x, trigger.transform.position.y, Camera.main.transform.position.z);
            Debug.DrawRay(newPos, Vector2.zero, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(newPos, Vector2.zero);
            if (hit.collider.gameObject == collision.gameObject)
            {
                int objectIndex = collision.gameObject.GetComponent<ClueScript>().Index;
                if (dialogueManagers[0].GetComponent<DialogueManager>().currentGateIndex == objectIndex)
                {
                    dialogueManagers[0].GetComponent<DialogueManager>().OpenGate(objectIndex);
                    Debug.Log("OPENED!");
                }
            }
        }
        else if (!capturedObjects.Contains(collision.gameObject))
        {
            objectsOnScreen.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsOnScreen.Remove(collision.gameObject);
    }
}
