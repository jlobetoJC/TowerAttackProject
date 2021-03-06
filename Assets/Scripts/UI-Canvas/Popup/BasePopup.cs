﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePopup : MonoBehaviour
{
    public PopupsID popupId;
    public Text title;
    public Text description;
    public Button okButton;
    public bool isShowing;

    protected RectTransform _rect;

    /// <summary>
    /// from string (type of action like close or play press btn) to Actions.
    /// </summary>
    Dictionary<FunctionTypes, List<Action>> _functions;
    

    public enum FunctionTypes
    {
        ok,
        displayCallback,
        cancel
    }

    protected virtual void Awake()
    {
        _functions = new Dictionary<FunctionTypes, List<Action>>();
        okButton.onClick.AddListener(() => OkButtonPressed());
        _rect = GetComponent<RectTransform>();
    }

    protected virtual void Start ()
    {

    }

    public void AddFunction(FunctionTypes type, Action func)
    {
        if (!_functions.ContainsKey(type))
            _functions.Add(type, new List<Action>());

        _functions[type].Add(func);
    }

    public virtual void DisplayPopup()
    {
        if (isShowing) return;

        isShowing = true;
        ExecuteFunctions(FunctionTypes.displayCallback);
    }
	
    protected virtual void ExecuteFunctions(FunctionTypes type)
    {
        if (!_functions.ContainsKey(type))
            return;

        var list = _functions[type];
        foreach (var item in list)
        {
            item.Invoke();
        }
    }
	
	void Update () {
		
	}

    public virtual void OkButtonPressed()
    {
        ExecuteFunctions(FunctionTypes.ok);
        Destroy(gameObject);
    }
}
