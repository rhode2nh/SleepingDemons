using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PortalSetType
{
    OneToOne,
    ManyToOne,
}
public class PortalSet : MonoBehaviour
{
    public Portal PortalA { get; private set; }
    public Portal PortalB { get; private set; }
    [field: SerializeField] public PortalSetType PortalSetType { get; private set; }
    public List<Portal> AdditionalPortals { get; private set; }
    [field: SerializeField] public Material PortalMaterial { get; private set; }
    public Camera PortalCamera { get; private set; }
    public bool IsInitialized { get; private set; }
    public RenderTexture TargetTexture { get; private set; }
    [field: SerializeField] public RenderTexture MainRenderTexture { get; private set; }
    public List<PortalLightObject> PortalLightObjects { get; private set; }

    public Action<PortalSet, Portal> onWarp;

    private void Awake()
    {
        Portal[] portals = GetComponentsInChildren<Portal>();
        PortalCamera = GetComponentInChildren<Camera>();
        PortalLightObjects = FindObjectsByType<PortalLightObject>(FindObjectsSortMode.None).ToList();
        if (portals.Length < 2)
        {
            Debug.LogError("There must at least be 2 portals!");
            return;
        }

        if (PortalCamera == null)
        {
            Debug.LogError("A portal camera is required!");
            return;
        }

        switch (PortalSetType)
        {
            case PortalSetType.ManyToOne:
                if (portals.Length == 2)
                {
                    Debug.LogError("A many to one portal set must have at least 3 portals!");
                }
                AdditionalPortals = portals.ToList().GetRange(2, portals.Length - 2);
                break;
        }

        PortalA = portals[0];
        PortalB = portals[1];
        TargetTexture = new RenderTexture(Screen.width / 4, Screen.height / 4, 24);
        TargetTexture.antiAliasing = 1;
    }

    public void Init(int id)
    {
        PortalA.Init(PortalB, id * 2 + 1, this, OnWarp);
        PortalB.Init(PortalA, id * 2 + 2, this, OnWarp);
        if (PortalSetType == PortalSetType.ManyToOne)
        {
            foreach (var additionalPortal in AdditionalPortals)
            {
                additionalPortal.Init(PortalB, id * 2 + 3, this, OnWarp);
            }
        }
        PortalCamera.targetTexture = TargetTexture;
        PortalCamera.aspect = (float)MainRenderTexture.width / MainRenderTexture.height;
        IsInitialized = true;
    }

    private void OnWarp(Portal portal)
    {
        onWarp?.Invoke(this, portal);
    }
}
