using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuBehaviour : MonoBehaviour
{
    public GameObject uiElement;


    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void EnableUIElement()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }
    }
    public void DisableUIElement()
    {
        if (uiElement != null)
        {
            Debug.Log("Deactivating UI element.");
            uiElement.SetActive(false); 
        }
        else
        {
            
        }
    }

}
