﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{


	void Start ()
    {
		
	}
	

	void Update ()
    {
		
	}

    public void GoToMainMap()
    {
        SceneManager.LoadScene("NewMainMap");
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
