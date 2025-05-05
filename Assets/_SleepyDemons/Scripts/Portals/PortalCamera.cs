using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    [SerializeField] private Material portalMaterial;
    private List<PortalSet> _portalSets = new();

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
        _portalSets = FindObjectsByType<PortalSet>(FindObjectsSortMode.None).ToList();
    }

    private void Start()
    {
        for (int i = 0; i < _portalSets.Count; i++)
        {
            _portalSets[i].Init(i);
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        foreach (PortalSet portalSet in _portalSets)
        {
            if (!portalSet.IsInitialized) continue;
            
            CheckPortal(portalSet, portalSet.PortalA, portalSet.PortalB, src);
            CheckPortal(portalSet, portalSet.PortalB, portalSet.PortalA, src);

            if (portalSet.PortalSetType == PortalSetType.ManyToOne)
            {
                foreach (var additionalPortal in portalSet.AdditionalPortals)
                {
                    CheckPortal(portalSet, additionalPortal, portalSet.PortalB, src);
                }
            }
        }

        // Output the combined texture.
        Graphics.Blit(src, dst);
    }

    private void CheckPortal(PortalSet portalSet, Portal inPortal, Portal outPortal, RenderTexture src)
    {
        if ((!inPortal.IsRendererVisible() || !inPortal.RaycastCheck()) && !inPortal.IsPlayerInPortal()) return;
        foreach (var portalLightObject in portalSet.PortalLightObjects)
        {
            if (portalLightObject.IsLightOn())
            {
                portalLightObject.Activate();
                portalLightObject.SetPortals(inPortal, outPortal);
                portalLightObject.UpdatePos();
            }
            else
            {
                portalLightObject.Deactivate();
            }
        }
        RenderCamera(inPortal, outPortal, portalSet.PortalCamera);
        portalMaterial.SetInt("_MaskID", inPortal.MaskID);
        Graphics.Blit(portalSet.TargetTexture, src, portalSet.PortalMaterial);
    }

    private void RenderCamera(Portal inPortal, Portal outPortal, Camera portalCamera)
    {
        Transform inTransform = inPortal.Center;
        Transform outTransform = outPortal.Center;

        // Position the camera behind the other portal.
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
        portalCamera.transform.position = outTransform.TransformPoint(relativePos);
        
        // Rotate the camera to look through the other portal.
        Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
        relativeRot = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeRot;
        portalCamera.transform.rotation = outTransform.rotation * relativeRot;
        
        // Set the camera's oblique view frustum.
        Plane p = new Plane(outTransform.forward, outTransform.position);
        Vector4 clipPlane = new Vector4(p.normal.x, p.normal.y, p.normal.z, p.distance);
        Vector4 clipPlaneCameraSpace =
            Matrix4x4.Transpose(Matrix4x4.Inverse(portalCamera.worldToCameraMatrix)) * clipPlane;
        
        var newMatrix = _mainCamera.CalculateObliqueMatrix(clipPlaneCameraSpace);
        portalCamera.projectionMatrix = newMatrix;

        // Render the camera to its render target.
        portalCamera.ResetWorldToCameraMatrix();
        portalCamera.ResetProjectionMatrix();
        portalCamera.Render();
    }
}
