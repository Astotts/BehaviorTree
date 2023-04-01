using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskPatrol : Node
{
    private Transform _transform;
    private Transform[] _waypoints;

    private CharacterInfo _character;
    private PathFinder _pathfinder;

    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;

    private List<OverlayTile> path;

    public TaskPatrol(Transform transform, Transform[] waypoints, CharacterInfo character, PathFinder pathfinder)
    {
        _transform = transform;
        _waypoints = waypoints;
        _pathfinder = pathfinder;
        _character = character;
        path = new List<OverlayTile>();
    }

    public override NodeState Evaluate()
    {
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
                Transform wp = _waypoints[_currentWaypointIndex];
                path = _pathfinder.FindPath(_character.activeTile, MovementController.GetTileFromPos(wp.position));
            }
        }
        else
        { 
            if(path.Count > 0){
                MovementController.MoveAlongPath(_character, path);
            }
            else{
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
            
        }


        state = NodeState.RUNNING;
        return state;
    }

}