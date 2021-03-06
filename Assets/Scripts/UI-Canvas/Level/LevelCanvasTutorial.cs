﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvasTutorial : MonoBehaviour
{
	public Canvas lvlCanvas;
	public RectTransform pointsBar;

	List<Image> _arrowImages = new List<Image>();
    List<Text> _texts = new List<Text>();

	void Awake ()
    {
        _arrowImages = GetComponentsInChildren<Image>().ToList();
        DisableAllArrows();

		_texts = GetComponentsInChildren<Text> ().ToList ();
		DisableTexts ();

		if(lvlCanvas != null && pointsBar != null)
			pointsBar.SetParent (lvlCanvas.transform);
	}
	
    public void EnableArrowByName(string name)
    {
        foreach (var item in _arrowImages)
        {
            if (item.gameObject.name == name)
            {
                item.enabled = true;
                break;
            }
                
        }
    }

	public void DisableArrowByName(string name)
	{
		foreach (var item in _arrowImages)
		{
			if (item.gameObject.name == name)
			{
				item.enabled = false;
				break;
			}
		}
	}

    public void DisableAllArrows()
    {
        foreach (var item in _arrowImages)
        {
            item.enabled = false;
        }
    }

	public void SetArrowParentPosition(Vector3 pos, string name, float deltaY)
    {
        foreach (var item in _arrowImages)
        {
            if (item.gameObject.name == name)
            {
				item.rectTransform.parent.position = new Vector3(pos.x, pos.y + deltaY, 0);
                break;
            }
        }
    }

	public void DisableTexts()
	{
		foreach (var item in _texts)
		{
            if(item.enabled)
                item.enabled = false;
		}
	}

	public void EnableDisableTextByName(string name, bool value)
	{
		foreach (var item in _texts)
		{
			if (item.gameObject.name == name)
			{
				item.enabled = value;
				break;
			}
		}
	}

	void Update () {
		
	}
}
