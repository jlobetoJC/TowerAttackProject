﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverchargePilar : MonoBehaviour
{
    public List<TowerBase> affected;
    public Transform trailSpawnPoint;
    public LineRenderer line;
    public MeshRenderer ballRenderer;
    public ParticleSystem activatePS;

    Color activatedLineColor = new Color(0, 200f / 255f, 255);
    Color deactivatedLineColor;

    List<LineRenderer> _linesToTower;
    bool _isActive;
    float _effectTimeAux;
	
	void Start ()
    {
        _linesToTower = new List<LineRenderer>();
        _linesToTower.Add(line);
        line.SetPosition(0, trailSpawnPoint.position);
        line.SetPosition(1, affected[0].transform.position);
        deactivatedLineColor = line.startColor;

        for (int i = 1; i < affected.Count; i++)
        {
            if (affected[i] == null) continue;

            var l = Instantiate<LineRenderer>(line, trailSpawnPoint);
            l.SetPosition(0, trailSpawnPoint.position);
            l.SetPosition(1, affected[i].transform.position);
            _linesToTower.Add(l);
        }

        ballRenderer.material.SetColor("_EmissionColor", Color.black);
        ballRenderer.material.SetFloat("EmissionQty", 0f);
    }


    void Update ()
    {

    }

    /// <summary>
    /// Will be a clickable pilar. Manage the effects here.
    /// </summary>
    public void ActivatePilar(float effectTime)
    {
        _effectTimeAux = effectTime;
        _isActive = true;
        ballRenderer.material.SetColor("_EmissionColor", Color.green);
        ballRenderer.material.SetFloat("EmissionQty", 1f);

        activatePS.Play(true);
    }

    public void DeactivatePilar()
    {
        ballRenderer.material.SetColor("_EmissionColor", Color.black);
        ballRenderer.material.SetFloat("EmissionQty", 0f);
        activatePS.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        _isActive = false;

    }

    public void StunTowers()
    {
        foreach (var item in affected)
        {
            item.ReceiveStun(_effectTimeAux);
        }

        foreach (var item in _linesToTower)
        {
            item.startColor = activatedLineColor;
            item.endColor = activatedLineColor;
        }

        StartCoroutine(WaitUntilEffectTimeEnd());
    }

    IEnumerator WaitUntilEffectTimeEnd()
    {
        yield return new WaitForSeconds(_effectTimeAux);

        foreach (var item in _linesToTower)
        {
            item.startColor = deactivatedLineColor;
            item.endColor = deactivatedLineColor;
        }
    }
}
