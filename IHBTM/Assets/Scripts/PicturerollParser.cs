using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicturerollParser : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private DialogueManager dm;
    [SerializeField] private GameObject originalPictureroll;
    private List<GameObject> pictures = new List<GameObject>();
    private GameObject content;

    private GameObject originalContent;
    [SerializeField] private List<GameObject> originalPictures = new List<GameObject>();

    [Header("DIALOGUE")]
    [SerializeField] private List<TemporaryDialogue> pictureResponses = new List<TemporaryDialogue>();
    [SerializeField] private List<TemporaryDialogue> seenResponses = new List<TemporaryDialogue>();
    [SerializeField] private List<TemporaryDialogue> defaultResponses = new List<TemporaryDialogue>();

    private void OnEnable()
    {
        CleanUp();
        GameObject clone = Instantiate(originalPictureroll, canvas.transform);
        clone.transform.SetParent(this.transform);


        FindOriginalPictures();
        StartCoroutine(SetPictureData());
    }

    IEnumerator SetPictureData()
    {
        yield return new WaitForEndOfFrame();
        FindPictures();

        for (int i = 0; i < pictures.Count; i++)
        {
            int index = i;
            Button photoBtn = pictures[index].transform.GetChild(0).GetComponent<Button>();
            photoBtn.onClick.RemoveAllListeners();

            PictureInfo originalInfo = originalPictures[index].GetComponent<PictureInfo>();
            PictureInfo info = pictures[index].GetComponent<PictureInfo>();

            info.HasBeenSent = originalInfo.HasBeenSent;
            info.HasExpired = originalInfo.HasExpired;

            TemporaryDialogue photoDialogue = null;

            if (info.HasBeenSent)
            {
                photoDialogue = seenResponses[Random.Range(0, seenResponses.Count)];
                photoDialogue.ListOfBlocks[0].lines[0].sprite = photoBtn.image.sprite;
            }

            else if (info.HasExpired || info.Index == -1)
            {
                photoDialogue = defaultResponses[Random.Range(0, defaultResponses.Count)];
                photoDialogue.ListOfBlocks[0].lines[0].sprite = photoBtn.image.sprite;
            }

            else
                photoDialogue = pictureResponses[info.Index];

            info.SetDialogue(photoDialogue);

            photoBtn.onClick.AddListener(() => dm.SendBlocks(info.GetDialogue()));
            photoBtn.onClick.AddListener(() => OpenGate(info));
            photoBtn.onClick.AddListener(() => SetPictureStatus(index));
            photoBtn.onClick.AddListener(() => CleanUp());
            photoBtn.onClick.AddListener(() => HomeScreenManager.CurrentScreen = transform.parent.gameObject);
            photoBtn.onClick.AddListener(() => gameObject.GetComponent<Transition>().Exit());
        }
    }

    private void SetPictureStatus(int index)
    {
        PictureInfo info = originalPictures[index].GetComponent<PictureInfo>();
        if (!info.HasBeenSent)
        {
            info.HasBeenSent = true;
        }
    }

    private void OpenGate(PictureInfo info)
    {
        switch (info.Index)
        {
            case 2:
                if (dm.currentGateIndex != 0)
                    return;

                dm.OpenGate(0);
                for (int j = 0; j < 2; j++)
                {
                    PictureInfo prevInfo = originalPictures[j].GetComponent<PictureInfo>();
                    if (!prevInfo.HasBeenSent)
                        prevInfo.HasExpired = true;
                }
                break;
            case 3:
                if (dm.currentGateIndex != 1)
                    return;
                dm.OpenGate(1);
                break;
            case 4:
                if (dm.currentGateIndex != 3)
                    return;
                dm.OpenGate(3);
                break;
            default:
                break;
        }
    }

    private void FindPictures()
    {
        List<GameObject> days = new List<GameObject>();
        content = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).gameObject;
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        for (int i = 0; i < content.transform.childCount; i++)
        {
            GameObject currentChild = content.transform.GetChild(i).gameObject;
            if (currentChild.name.Contains("day"))
                days.Add(currentChild);
        }

        foreach (GameObject day in days)
        {
            GameObject photos = day.transform.Find("photos").gameObject;
            for (int i = 0; i < photos.transform.childCount; i++)
                pictures.Add(photos.transform.GetChild(i).gameObject);
        }
    }

    private void FindOriginalPictures()
    {
        List<GameObject> days = new List<GameObject>();
        if(originalContent == null)
            originalContent = originalPictureroll.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        for (int i = 0; i < originalContent.transform.childCount; i++)
        {
            GameObject currentChild = originalContent.transform.GetChild(i).gameObject;
            if (currentChild.name.Contains("day"))
                days.Add(currentChild);
        }

        foreach (GameObject day in days)
        {
            GameObject photos = day.transform.Find("photos").gameObject;
            for (int i = 0; i < photos.transform.childCount; i++)
                originalPictures.Add(photos.transform.GetChild(i).gameObject);
        }
    }

    private void CleanUp()
    {
        pictures.Clear();
        originalPictures.Clear();
        content = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
