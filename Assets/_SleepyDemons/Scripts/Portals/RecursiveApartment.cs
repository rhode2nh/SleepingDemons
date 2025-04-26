using System;
using UnityEngine;

public class RecursiveApartment : MonoBehaviour
{
    private PortalSet _portalSet;

    private void Awake()
    {
        _portalSet = GetComponentInChildren<PortalSet>();
    }

    private void Start()
    {
        _portalSet.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _portalSet.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _portalSet.gameObject.SetActive(false);
        }
    }
}
