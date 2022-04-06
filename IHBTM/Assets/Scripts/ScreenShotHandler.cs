using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotHandler : MonoBehaviour
{
    [SerializeField] Camera myCamera;
    private bool takeScreenShotNextFrame;
    Texture2D renderResult, newTexture;
    Rect rect;
    [SerializeField] GameObject photo, content, todayContent;
    private GameObject clone;
    [SerializeField] Canvas canvas;
    public Sprite sprite;
    private bool firstScreenshot = true;

    [SerializeField] private CameraRollManager CRM;

    private void Start()
    {
        
    }

    /*
    private void OnPostRender()
    {
        if (takeScreenShotNextFrame)
        {
            takeScreenShotNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;
            renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            renderResult.Apply();

            SaveScreenshot();

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }*/

    public IEnumerator RecordFrame(int index)
    {
        yield return new WaitForEndOfFrame();
        newTexture = ScreenCapture.CaptureScreenshotAsTexture();
        rect = new Rect(0, 0, newTexture.width, newTexture.height);
        newTexture.ReadPixels(rect, 0, 0);
        newTexture.Apply();
        SaveScreenshot(index);
    }

    /*
    public void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenShotNextFrame = true;
    }*/

    public void SaveScreenshot(int index)
    {
        if (firstScreenshot)
        {
            clone = Instantiate(todayContent, canvas.transform) as GameObject;
            clone.transform.SetParent(content.transform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
            firstScreenshot = false;
        }

        sprite = Sprite.Create(newTexture, new Rect(0, 0, rect.width, rect.height), Vector2.zero);
        GameObject currentPhoto = Instantiate(photo, canvas.transform) as GameObject;
        currentPhoto.GetComponent<PictureInfo>().Index = index;

        Button photoBtn = currentPhoto.transform.GetChild(0).GetComponent<Button>();
        photoBtn.onClick.AddListener(() => CRM.SetPictureViewActive(photoBtn));

        Image photoImage = photoBtn.image;
        photoImage.sprite = sprite;
        photoImage.SetNativeSize();
        photoImage.transform.localScale *= 1.5f;


        currentPhoto.transform.SetParent(clone.transform.GetChild(1).transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(clone.GetComponent<RectTransform>());
    }
}
