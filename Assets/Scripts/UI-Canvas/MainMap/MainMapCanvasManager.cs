﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMapCanvasManager : MonoBehaviour
{
    public Button levelNodeButton;

    HorizontalLayoutGroup _levelNodesContainer;

	void Awake () {
        _levelNodesContainer = GetComponentInChildren<HorizontalLayoutGroup>();
	}
	
	void Update () {
		
	}

    public void AddLevelButton(LevelInfo lvlInfo, Action<LevelInfo> onClick)
    {
        var btn = Instantiate<Button>(levelNodeButton, _levelNodesContainer.transform);
        LevelInfo lazyLvlInfo = new LevelInfo();
        lazyLvlInfo = lvlInfo;
        btn.onClick.AddListener(() => onClick(lazyLvlInfo));
        btn.GetComponentInChildren<Text>().text = "Level " + lazyLvlInfo.id;
    }
}