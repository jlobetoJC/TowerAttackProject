﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTargetSkill : BaseMinionSkill
{
    float _to;

    protected override void Start()
    {
        base.Start();
        skillType = SkillType.ChangeTarget;
    }


    public override bool Initialize(float lastingTime, float cooldown)
    {
        return base.Initialize(lastingTime, cooldown);
    }


	public void SetYDest(float y)
    {
        _to = y;
    }

    public override bool ExecuteSkill()
    {
        if (_to == 0) return false;

        var y = transform.position.y;
        y = Mathf.Lerp(y, _to, Time.deltaTime * 5.5f);
        
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
  		      
		transform.localPosition = new Vector3 (transform.localPosition.x, transform.position.y, transform.localPosition.z);


        return true;
    }

}
