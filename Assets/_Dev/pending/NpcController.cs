using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.BehaviourTrees;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NpcController : MonoBehaviour
{
    private NavMeshAgent agent;
    private BehaviourTree tree;

    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private Transform safeSpot;
    [SerializeField] private bool inDanger;
    [SerializeField] private GameObject treasure1;
    [SerializeField] private GameObject treasure2;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        tree = new BehaviourTree("Enemy");

        PrioritySelector actions = new PrioritySelector("Agent Logic");
        
        Sequence runToSafetySeq = new Sequence("RunToSafety", 100);

        bool IsSafe()
        {
            if (!inDanger)
            {
                runToSafetySeq.Reset();
                return false;
            }

            return true;
        }
        
        runToSafetySeq.AddChild(new Leaf("isSafe?", new Condition(IsSafe)));
        runToSafetySeq.AddChild(new Leaf("Go To Safety", new MoveToTarget(transform, agent, safeSpot)));
        actions.AddChild(runToSafetySeq);

        Selector goToTreasure = new Selector("GoToTreasure");
        Sequence getTreasure1 = new Sequence("GetTreasure1");
        getTreasure1.AddChild(new Leaf("isTreasure1?", new Condition(() => treasure1.activeSelf)));
        getTreasure1.AddChild(new Leaf("GoToTreasure1", new MoveToTarget(transform, agent, treasure1.transform)));
        getTreasure1.AddChild(new Leaf("PickUpTreasure1", new ActionStrategy(() => treasure1.SetActive(false))));
        goToTreasure.AddChild(getTreasure1);

        Sequence getTreasure2 = new Sequence("GetTreasure1");
        getTreasure2.AddChild(new Leaf("isTreasure2?", new Condition(() => treasure2.activeSelf)));
        getTreasure2.AddChild(new Leaf("GoToTreasure2", new MoveToTarget(transform, agent, treasure2.transform)));
        getTreasure2.AddChild(new Leaf("PickUpTreasure2", new ActionStrategy(() => treasure2.SetActive(false))));
        goToTreasure.AddChild(getTreasure2);
        
        actions.AddChild(goToTreasure);

        Leaf patrol = new Leaf("Patrol", new PatrolStrategy(transform, agent, waypoints));
        actions.AddChild(patrol);
        
        tree.AddChild(actions);
    }

    void Update()
    {
        tree.Process();
    }
}
