using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InstagramManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private List<GameObject> profiles = new List<GameObject>();
    private GameObject currentProfile;

    public void GoToProfile(int index)
    {
        currentProfile = Instantiate(profiles[index], canvas.transform);
        currentProfile.transform.SetParent(transform);
    }

    public void GoBackToFeed()
    {
        currentProfile.GetComponent<ProfileScript>().isClosing = true;
        StartCoroutine(DestroyObject(currentProfile));
    }

    IEnumerator DestroyObject(GameObject target)
    {
        yield return new WaitForSeconds(1f);
        Destroy(target);
    }

    public void GoBackToHomeScreen()
    {
        homeScreen.SetActive(true);
        GetComponent<ShowUp>().isClosing = true;
        StartCoroutine(WaitTilDisactivate());
    }

    IEnumerator WaitTilDisactivate()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

}
