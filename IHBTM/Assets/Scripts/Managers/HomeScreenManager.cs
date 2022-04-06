using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreenManager : MonoBehaviour
{
    public List<GameObject> appScreens = new List<GameObject>();
    private GameObject currApp;
    public static GameObject CurrentScreen;
    [SerializeField] private GameObject canvas;
    private GameObject gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>().gameObject;
    }

    public void GoToApp(int index)
    {
        appScreens[index].SetActive(true);
        currApp = appScreens[index];
        GameObject a = appScreens[index];
        bool noActiveChildren;
        noActiveChildren = FindActiveChild(a);
        if (noActiveChildren)
        {
            CurrentScreen = appScreens[index];
        }
    }

    private bool FindActiveChild(GameObject p)
    {
        bool noActiveChildren = false;
        for (int i = p.transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = p.transform.GetChild(i).gameObject;
            bool noChildScreensActive = true;
            if (child.activeSelf && child.CompareTag("Screen"))
            {
                CurrentScreen = child;
                if(child.transform.childCount != 0)
                {
                    noChildScreensActive = FindActiveChild(child);
                }
                break;
            }
            if (i == 0 && noChildScreensActive)
            {
                noActiveChildren = true;
            }
        }
        return noActiveChildren;
    }

    public void BackToHomeScreen()
    {
        if(currApp != null && currApp.activeSelf == true)
        {
            currApp.GetComponent<Transition>().Exit();
        }
        ToggleScreen();
    }

    public void Backtrack()
    {
        if(CurrentScreen != null && CurrentScreen.activeSelf == true)
        {
            GameObject prev = CurrentScreen;
            if (prev.GetComponent<Transition>() != null)
                prev.GetComponent<Transition>().Exit();
            if(CurrentScreen.transform.parent.gameObject == canvas)
            {
                CurrentScreen = null;
            }
            else
            {
                CurrentScreen = CurrentScreen.transform.parent.gameObject;
            }

            ToggleScreen();
        }
    }

    private void ToggleScreen()
    {
        gm.GetComponent<BoxCollider2D>().enabled = false;
        gm.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void SetCurrentScreen(GameObject screen)
    {
        CurrentScreen = screen;
    }
}
