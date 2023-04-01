using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GuardBT : BTree
{
    //https://youtu.be/aR6wt5BlE-E
    public UnityEngine.Transform[] waypoints;
    public static float range = 6f;
    public static float attackRange = 1f;
    [SerializeField] private CharacterInfo character;
    private PathFinder pathfinder; 

    protected override Node SetupTree()
    {
        Debug.Log("SetupTree");
        pathfinder = new PathFinder();
        
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new EnemyInRange(transform),
                new TaskGoToTarget(transform, character, pathfinder),
            }),
            new TaskPatrol(transform, waypoints, character, pathfinder),
        });

        return root;
    }
}
