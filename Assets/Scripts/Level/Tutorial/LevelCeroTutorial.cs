﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This is the level tutorial for level 0.
/// </summary>
public class LevelCeroTutorial : Level
{
	public Action<GameObject> ExecuteStep = delegate {};

    public HitAreaCollider forRunner;
    public HitAreaCollider forDoveOne;
    public HitAreaCollider forDoveTwo;
    public HitAreaCollider forTankOne; // spawn more minions
    public HitAreaCollider forTankTwo; //show skill range
    public HitAreaCollider forTankThree; //spawn skill coll
	public bool tankTutoStarted;

    LevelCanvasTutorial _lvlCanvasTuto;
	[HideInInspector]public List<MinionType> addMinionButton = new List<MinionType>();
	TutorialCeroManager _tutoManager;

    bool _stopTutorial;
    bool _canShowTheOtherMinions;
    bool _builtFirstMinion;
    int _runnerCount;
    bool _runnerMinionHasDead;
    int _doveCount;
    int _tankCount;
	bool _hasViewedFirstTowerRange;//if user saw only one tower range ps (dove part)
	TowerType _otherTower;
	int _spawnedAfterTank;

	public LevelCanvasTutorial LevelCanvasTutorial{get{ return _lvlCanvasTuto;}}

    protected override void Init()
    {
		_lvlCanvasTuto = FindObjectOfType<LevelCanvasTutorial>();

        base.Init();
        _minionManager.OnMinionWalkFinished += MinionWalkFinishedHandler;
        _minionManager.OnMinionDeath += MinionDeathHandler;
        _minionManager.OnMinionSkillSelected += MinionSkillSelectedHandler;
		_towerManager.OnClickTower += TowerClickedHandler;
        _towerManager.HideAllTowers();

    }

    protected override void InitLevelCanvas()
    {
        if (_lvlCanvasManager == null)
            _lvlCanvasManager = FindObjectOfType<LevelCanvasManager>();
        
        _lvlCanvasManager.ShowHideAllUI(false);

        _lvlCanvasManager.level = this;

		//Add Runner;
		ExecuteStep (gameObject);
		//Activate Arrow
		ExecuteStep(_lvlCanvasTuto.gameObject);
    }


    public override bool BuildMinion(MinionType t)
    {
        var result = base.BuildMinion(t);

		if (t == MinionType.Runner) 
		{
			_runnerCount++;
			if(!tankTutoStarted)
				ExecuteStep (gameObject);
		}
		else if (t == MinionType.Dove)
		{
			_doveCount++;
		} 
		else if (t == MinionType.Tank) 
		{
			_tankCount++;
			_livesRemoved = 0;
		}

		if (tankTutoStarted && t != MinionType.Tank) 
		{
			ExecuteStep (MinionManager.GetMinion(t).gameObject);

			if (t == MinionType.Dove)
			{
				_spawnedAfterTank++;
			}
			else if (t == MinionType.Runner)
			{
				_spawnedAfterTank++;
			}

			if (_spawnedAfterTank == 2)
			{
				Time.timeScale = 1;
				_stopTutorial = true;
			}
		}

        return result;
    }

    public void OnPopupButtonPressed()
    {
		//if first runner ends path.
		//if second runner ends path > dove part triggered.
		//Dove has arrived to end.
		ExecuteStep (gameObject);

    }

    protected override void GoalCompletedHandler()
    {
        if (_stopTutorial)
            base.GoalCompletedHandler();
    }

    void MinionWalkFinishedHandler(MinionType type)
    {
        if (type == MinionType.Runner)
        {
			if(!tankTutoStarted)
				ExecuteStep (gameObject);
        }

        if(type == MinionType.Dove)
        {
			if (_doveCount == 1) 
			{
				ExecuteStep (null);
			}
        }
	}
    
    void MinionDeathHandler(MinionType type)
    {
        if(type == MinionType.Runner)
        {
            if(_runnerCount == 2)
				ExecuteStep (gameObject);
        }
    }

    public void OnRunnerColEnter(Collider col)
    {
		forRunner.OnTriggerEnterCallback -= OnRunnerColEnter;
		
		ExecuteStep (col.gameObject);
    }

	public void OnDoveColEnter(Collider col)
	{
		ExecuteStep (col.gameObject);
	}

    public void OnTankEnterOne(Collider col)
    {
        if(col.GetComponent<Minion>().minionType == MinionType.Tank)
        {
			ExecuteStep (null);
        }
    }

    public void OnTankEnterTwo(Collider col)
    {
        if (col.GetComponent<Minion>().minionType == MinionType.Tank)
        {
            _lvlCanvasTuto.EnableArrowByName("PointingToRunnerSkill");
            var pos = Camera.main.WorldToScreenPoint(col.transform.position);
            _lvlCanvasTuto.SetArrowPosition(pos, "PointingToRunnerSkill");
            Time.timeScale = 0;
        }
    }

    void MinionSkillSelectedHandler(MinionType t)
    {
        _lvlCanvasTuto.DisableAllArrows();
        Time.timeScale = 1;
    }

	void TowerClickedHandler(TowerType t)
	{
		_lvlCanvasTuto.DisableArrowByName (t == TowerType.Antiair ? "SecondPointer" : "ThirdPointer");
		if (_hasViewedFirstTowerRange && _otherTower != t)
		{
			ExecuteStep (gameObject);
			_towerManager.OnClickTower -= TowerClickedHandler;
		}

		_hasViewedFirstTowerRange = true;
		_otherTower = t;
	}
}