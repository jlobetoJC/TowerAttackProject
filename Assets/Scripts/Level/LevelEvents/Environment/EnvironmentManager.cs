﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnvironmentManager : MonoBehaviour, IEvent
{

    Level _lvl;
    EnvironmentEventItem _item;

    List<EnvironmentBridge> _bridges;
    bool _enabled;
    bool _bridgeEnabled;
    float _currentTimeToChange;
    bool _didFirstShow;

    public void Init(EnvironmentEventItem item, Level lvl)
    {
        _item = item;
        _lvl = lvl;
        _enabled = true;

        if (!item.bridgeEnabled) return;
        
        _bridges = new List<EnvironmentBridge>();
        _bridgeEnabled = true;
        _currentTimeToChange = 1f;
        var list = GetNodeList(_lvl.GetInitWalkNode(), new List<WalkNode>());
        var pivots = GetWalkNodesBridge(list, "pivot");
        var dests = GetWalkNodesBridge(list, "dest");
        var bridgesGO = FindObjectsOfType<EnvironBridgeEffect>().ToList();
        var clocksOnScene = FindObjectsOfType<BridgeClock>();

        foreach (var p in pivots)
        {
            if (_bridges.Any(i => i.pivot == p)) continue;

            var bridge = new EnvironmentBridge(p);
            var pivotNum = int.Parse(p.levelEventBridgeNodeName.Split('_')[1]);
            foreach (var destination in dests)
            {
                var splitted = destination.levelEventBridgeNodeName.Split('_');
                if (int.Parse(splitted[1]) == pivotNum)
                {
                        
                    if (splitted[2] == "a")
                    {
                        //Debug.Log("A");
                        bridge.destinationA = destination;
                        bridge.bridge_A_GameObject = bridgesGO.FirstOrDefault(i => i.destination == destination.levelEventBridgeNodeName);
                    }
                    else if (splitted[2] == "b")
                    {
                        //Debug.Log("B");
                        bridge.destinationB = destination;
                        bridge.bridge_B_GameObject = bridgesGO.FirstOrDefault(i => i.destination == destination.levelEventBridgeNodeName);
                    }

                }
            }

			if (bridge.bridge_B_GameObject == null || bridge.bridge_A_GameObject == null) 
			{
				var name = bridge.bridge_A_GameObject == null ? "A" : "B";
				throw new Exception ("There is not a GameObject With 'EvironBridgeEffect' for bridge " + name);
			}

            foreach (var c in clocksOnScene)
            {
                if (c.bridgePivot.Contains(pivotNum.ToString()))
                {
                    bridge.bridgeClock = c;
                    break;
                }
            }

            bridge.bridgeClock.StartCountdown(_currentTimeToChange);
            _bridges.Add(bridge);
        }
        
    }

    List<WalkNode> GetWalkNodesBridge(List<WalkNode> l, string name)
    {
        return l.Where(p => p.levelEventBridgeNodeName.Contains(name)).ToList();
    }


    List<WalkNode> GetNodeList(WalkNode node, List<WalkNode> list)
    {
        if(node)
        list.Add(node);
        if (node.isEnd)
            return list;

        foreach (var item in node.nexts)
        {
            list.AddRange(GetNodeList(item, new List<WalkNode>()));
        }

        return list;
    }

    public void StopEvent()
    {
        _enabled = false;
    }

    public void UpdateEvent()
    {
        if (!_enabled) return;

        _currentTimeToChange -= Time.deltaTime;
        
        if (_currentTimeToChange < 0)
        {
            ManageBridge();
        }
    }

    void ManageBridge()
    {
        _currentTimeToChange = UnityEngine.Random.Range(_item.eventTimer[0], _item.eventTimer[1]);

        foreach (var bridge in _bridges)
        {
            bridge.pivot.nexts = new List<WalkNode>();
            bridge.pivot.nexts.Add(bridge.isPointingA ? bridge.destinationB : bridge.destinationA);

            bridge.bridgeClock.StartCountdown(_currentTimeToChange);

            if (bridge.isPointingA)
            {
                bridge.bridge_A_GameObject.MakeAllWayDown();
                if (_didFirstShow)
                    bridge.bridge_B_GameObject.PushUpFloor();
            }
            else
            {
                bridge.bridge_A_GameObject.PushUpFloor();
                bridge.bridge_B_GameObject.MakeAllWayDown();
            }
                

            bridge.isPointingA = !bridge.isPointingA;
                
        }

        _didFirstShow = true;
    }
}
