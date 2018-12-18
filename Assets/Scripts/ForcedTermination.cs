using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ForcedTermination : MonoBehaviour
{
    public static ForcedTermination instance;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }
	
	//========================================================
	void Update ()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            Application.Quit();
        #endif
        }
    }
}
