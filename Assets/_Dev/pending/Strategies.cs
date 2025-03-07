using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Pathfinding.BehaviourTrees
{
    public interface IStrategy
    {
        Node.Status Process();

        void Reset()
        {
            
        }
    }

    public class ActionStrategy : IStrategy
    {
        private Action doSomething;

        public ActionStrategy(Action doSomething)
        {
            this.doSomething = doSomething;
        }

        public Node.Status Process()
        {
            doSomething();
            return Node.Status.Success;
        }
    }

    public class Condition : IStrategy
    {
        private readonly Func<bool> predicate;

        public Condition(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public Node.Status Process() => predicate() ? Node.Status.Success : Node.Status.Failure;
    }

    public class MoveToTarget : IStrategy
    {
        private readonly Transform entity;
        private readonly NavMeshAgent agent;
        private readonly Transform target;
        private bool isPathCalculated;

        public MoveToTarget(Transform entity, NavMeshAgent agent, Transform target)
        {
            this.entity = entity;
            this.agent = agent;
            this.target = target;
        }
        
        public Node.Status Process()
        {
            if (isPathCalculated && agent.remainingDistance < 0.1f) return Node.Status.Success;

            agent.SetDestination(target.position);

            if (agent.pathPending)
            {
                isPathCalculated = true;
            }

            return Node.Status.Running;
        }

        public void Reset() => isPathCalculated = false;
    }

    public class PatrolStrategy : IStrategy
    {
        private readonly Transform entity;
        private readonly NavMeshAgent agent;
        private readonly List<Transform> patrolPoints;
        private readonly float patrolSpeed;
        private int currentIndex;
        private bool isPathCalculated;

        public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
        {
            this.entity = entity;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
        }

        public Node.Status Process()
        {
            if (currentIndex == patrolPoints.Count) return Node.Status.Success;

            var target = patrolPoints[currentIndex];
            agent.SetDestination(target.position);

            if (isPathCalculated && agent.remainingDistance < 0.1f)
            {
                currentIndex++;
                isPathCalculated = false;
            }

            if (agent.pathPending)
            {
                isPathCalculated = true;
                Debug.Log(isPathCalculated);
            }

            return Node.Status.Running;
        }

        public void Reset() => currentIndex = 0;
    }
}

