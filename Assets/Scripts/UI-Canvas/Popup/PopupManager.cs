﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    static PopupManager _instance;
    public List<BasePopup> popupPrefabs = new List<BasePopup>();

    List<BasePopup> _popupQueue;

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            _popupQueue = new List<BasePopup>();
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }


    void Update() {

    }

    public BasePopup BuildPopup(Transform parent, string title, string descript, string btnText, PopupsID popupId = PopupsID.BasePopup)
    {
        if (popupPrefabs.All(i => i.popupId != popupId))
            return null;

        var popup = Instantiate<BasePopup>(popupPrefabs.FirstOrDefault(i => i.popupId == popupId), parent);
        popup.title.text = title.ToUpper();
        popup.description.text = descript.ToUpper();
        popup.GetComponent<Animator>().SetFloat("EntryAnim", GetRandomAnimation());
        popup.isShowing = true;
        if (popup.okButton != null)
            popup.okButton.GetComponentInChildren<Text>().text = btnText.ToUpper();

        return popup;
    }

    float GetRandomAnimation()
    {
        return Random.Range(0f, 3.9f);
    }
}
