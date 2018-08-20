﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public string minionName;
    public float hp = 50;
    public GameObjectTypes type = GameObjectTypes.None;
    public MinionType minionType = MinionType.Runner;
    public float speed = 4;
    public int pointsValue = 15;
    public float strength = 0;
    public IMinionSkill skill;
    public int coinValue = 0;
    [Range(0f,1f)]
    public float levelPointsToRecover = 0.75f;
    public Action<Minion> OnWalkFinished = delegate { };
    public Action<Minion> OnDeath = delegate { };

    protected WalkNode pNextNode;
    protected InfoCanvas pInfoCanvas;
    protected float pDistanceToNextNode = 0.3f;//To change the next node;

    int _currentLevel = 1;//Level of the minion, ///TODO manage this when buying an upgrade of lvl;
    int _spawnOrder;
    int _id;
    bool _canWalk;

    public int Id { get { return _id; } }
    public bool CanWalk { get { return _canWalk; } }

    public void InitMinion(WalkNode n)
    {
        pNextNode = n.GetNextWalkNode();
    }

    public void SetWalk(bool val)
    {
        if (pNextNode.isEnd) return;//don't know if this will be here, for testing porpuse must be for the moment.

        _canWalk = val;
    }

    public void GetDamage(float dmg)
    {
        hp -= dmg;
        pInfoCanvas.UpdateLife(hp);
        DeathChecker();
    }

    protected virtual void PerformAction()
    {
        if (_canWalk)
            Walk();
    }

    protected virtual void Walk()
    {
        var dir = (pNextNode.transform.position - transform.position).normalized;
        transform.forward = dir;
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, pNextNode.transform.position) <= pDistanceToNextNode)
        {
            if (!pNextNode.isEnd)
                pNextNode = pNextNode.GetNextWalkNode();
            else
                FinishWalk();
        }
    }

    protected void Init()
    {
        _id = gameObject.GetInstanceID();
        pInfoCanvas = GetComponentInChildren<InfoCanvas>();
        if (pInfoCanvas == null)
            throw new Exception("InfoCanvas is not set as a child");

        pInfoCanvas.Init(hp);
    }

    protected virtual void Start ()
    {
        Init();
	}


    protected virtual void Update () {
        PerformAction();
	}

    protected void FinishWalk()
    {
        _canWalk = false;
        OnWalkFinished(this);
    }

    void DeathChecker()
    {
        if (hp <= 0)
            OnDeath(this);
    }

    
}
