using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Generic utility for loading scenes by name. called by UI Button OnClick events in the Inspector
public class sceneLoader : MonoBehaviour
{
    public void LoadByName(string n)
    {
        SceneManager.LoadScene(n);
    }
}
