using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> waypoints;
    [SerializeField] public GameObject currentWaypoint;
    [SerializeField] public GameObject lastWaypoint;
}
