using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class EnemyInRange : Node
{
    private Transform _transform;

    public EnemyInRange(Transform transform){
        _transform = transform;
    }

    public override NodeState Evaluate(){
        object t = GetData("target");
        if(t == null){
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2f, 64);

            //Debug.Log(colliders.Length);
            foreach(Collider2D collider in colliders){
                Debug.Log(collider);
            }

            if(colliders.Length > 0){        
                parent.parent.SetData("target", colliders[0].transform);
                

                state = NodeState.SUCCESS;
                return state;
            }
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
