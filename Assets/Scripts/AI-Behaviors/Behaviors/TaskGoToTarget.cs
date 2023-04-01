using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskGoToTarget : Node
{
    private Transform _transform;
    private CharacterInfo _character;
    private PathFinder _pathfinder;
    private List<OverlayTile> path;
    
    public TaskGoToTarget(Transform transform, CharacterInfo character, PathFinder pathfinder){
        _transform = transform;
        _character = character;
        _pathfinder = pathfinder;
        path = new List<OverlayTile>();
    }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData("target");
        if(Vector3.Distance(_transform.position, target.position) > 0.55f){ //Swap this out for a tile check later on otherwise it'll stop mid movement instead of being tile-locked
            if(path.Count <= 0 && target != null){
                path = _pathfinder.FindPath(_character.activeTile, MovementController.GetBestTile(MovementController.GetTileFromPos(target.position), _transform));
            }
            else{
                MovementController.MoveAlongPath(_character, path);
            }
            state = NodeState.RUNNING;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
