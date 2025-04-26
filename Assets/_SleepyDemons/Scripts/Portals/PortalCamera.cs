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
        _portalSets = FindObjectsOfType<PortalSet>().ToList();
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
            if (!portalSet.PortalA.IsRendererVisible() && !portalSet.PortalB.IsRendererVisible())
            {
                portalSet.PortalLightObject.SetPortals(null, null);
            }
            
            if (portalSet.PortalA.IsRendererVisible())
            {
                // Render the first portal output onto the image.
                if (portalSet.PortalA.IsVisibleFromCamera())
                {
                    portalSet.PortalLightObject.SetPortals(portalSet.PortalA, portalSet.PortalB);
                    portalSet.PortalLightObject.UpdatePos();
                }
                RenderCamera(portalSet.PortalA, portalSet.PortalB, portalSet.PortalCamera);
                portalMaterial.SetInt("_MaskID", portalSet.PortalA.MaskID);
                Graphics.Blit(portalSet.TargetTexture, src, portalMaterial);
            }
            
            if (portalSet.PortalB.IsRendererVisible())
            {
                // Render the first portal output onto the image.
                if (portalSet.PortalB.IsVisibleFromCamera())
                {
                    portalSet.PortalLightObject.SetPortals(portalSet.PortalB, portalSet.PortalA);
                    portalSet.PortalLightObject.UpdatePos();
                }
                RenderCamera(portalSet.PortalB, portalSet.PortalA, portalSet.PortalCamera);
                portalMaterial.SetInt("_MaskID", portalSet.PortalB.MaskID);
                Graphics.Blit(portalSet.TargetTexture, src, portalSet.PortalMaterial);
            }

            if (portalSet.PortalSetType == PortalSetType.ManyToOne)
            {
                foreach (var additionalPortal in portalSet.AdditionalPortals)
                {
                    if (additionalPortal.IsRendererVisible())
                    {
                        // Render the first portal output onto the image.
                        if (additionalPortal.IsVisibleFromCamera())
                        {
                            portalSet.PortalLightObject.SetPortals(additionalPortal, portalSet.PortalB);
                            portalSet.PortalLightObject.UpdatePos();
                        }
                        RenderCamera(additionalPortal, portalSet.PortalB, portalSet.PortalCamera);
                        portalMaterial.SetInt("_MaskID", additionalPortal.MaskID);
                        Graphics.Blit(portalSet.TargetTexture, src, portalSet.PortalMaterial);
                    }
                }
            }
        }

        // Output the combined texture.
        Graphics.Blit(src, dst);
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
