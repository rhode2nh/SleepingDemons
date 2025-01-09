using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WalkBehavior : StateMachineBehaviour
{
    private NavMeshAgent _navMesh;
    private TestEnemyManager enemyManager;
    private EyeSensor _eyeSensor;
    [SerializeField] private float margin;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _navMesh = animator.GetComponent<NavMeshAgent>();
        enemyManager = animator.GetComponent<TestEnemyManager>();
        _eyeSensor = animator.GetComponentInChildren<EyeSensor>();

        var waypoints = enemyManager.waypoints;

        if (enemyManager.lastWaypoint != null)
        {
            waypoints = waypoints.Where(x => x != enemyManager.lastWaypoint).ToList();
        }
        
        enemyManager.currentWaypoint = waypoints[Random.Range(0, waypoints.Count - 1)];
        _navMesh.destination = enemyManager.currentWaypoint.transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_eyeSensor.HasAcknowledgedPlayer)
        {
            animator.SetTrigger("isStartled");
        }
        if (Vector3.Distance(enemyManager.transform.position, _navMesh.destination) <= margin)
        {
            animator.SetBool("isIdle", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyManager.lastWaypoint = enemyManager.currentWaypoint;
        enemyManager.currentWaypoint = null;
        animator.SetBool("isWalking", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
