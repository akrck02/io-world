using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartControls : MonoBehaviour
{
 
    public LevelLoader levelLoader;

    // Start is called before the first frame update
    void OnStart()
    {
        levelLoader.LoadNextLevel();
    }

}
