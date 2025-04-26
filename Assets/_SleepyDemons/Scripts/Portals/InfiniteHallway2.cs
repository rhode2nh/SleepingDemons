using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InfiniteHallway2 : MonoBehaviour
{
    [SerializeField] private int _currentDepth;
    [SerializeField] private int _triggerDepth1;
    [SerializeField] private int _maxDepth;

    private List<PortalSet> _portalSets = new();
    private bool _maxDepthReached = false;

    private void Awake()
    {
        _portalSets = GetComponentsInChildren<PortalSet>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var portalSet in _portalSets)
        {
            portalSet.onWarp += OnWarp;
        }
        _portalSets[^1].gameObject.SetActive(false);
    }

    private void OnWarp(PortalSet portalSet, Portal portal)
    {
        if (portalSet == _portalSets[0])
        {
        }
        else if (portalSet == _portalSets[^1])
        {
            _currentDepth += portal == portalSet.PortalA ? -1 : 1;
        }
        else
        {
            _currentDepth += portal == portalSet.PortalA ? -1 : 1;
        }

        if (_currentDepth == 0)
        {
            _portalSets[0].gameObject.SetActive(true);
            _portalSets[^1].gameObject.SetActive(false);
        }

        if (_currentDepth > 0 && !_maxDepthReached)
        {
            _portalSets[0].gameObject.SetActive(false);
            _portalSets[^1].gameObject.SetActive(true);
        }

        if (_currentDepth == _triggerDepth1)
        {
            _portalSets[^1].PortalA.gameObject.SetActive(false);
        }

        if (_currentDepth == _maxDepth)
        {
            _portalSets[^1].gameObject.SetActive(false);
            _maxDepthReached = true;
        }
    }
}
