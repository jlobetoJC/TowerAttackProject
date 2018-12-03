﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    static PopupManager _instance;
    public List<BasePopup> popupPrefabs = new List<BasePopup>();

    
	void Start ()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }
	
	
	void Update () {
		
	}

    public void BuildOneButtonPopup(Transform parent, string title, string descript, string btnText, PopupsID popupId = PopupsID.OneButton)
    {
        var popup = Instantiate<BasePopup>(popupPrefabs.FirstOrDefault(i => i.popupId == popupId), parent);
        popup.title.text = title.ToUpper();
        popup.description.text = descript.ToUpper();
        popup.GetComponent<Animator>().SetFloat("EntryAnim", GetRandomAnimation());

        if (popup.actionButton != null)
        {
            popup.actionButton.GetComponentInChildren<Text>().text = btnText.ToUpper();
        }
    }

    float GetRandomAnimation()
    {
        return Random.Range(0f, 3.9f);
    }
}
