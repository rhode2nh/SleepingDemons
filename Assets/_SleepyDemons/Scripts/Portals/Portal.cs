using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Utility;
using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [SerializeField] private Renderer outlineRenderer;
    [SerializeField] private Color portalColour;
    [SerializeField] private LayerMask placementMask;
    [field: SerializeField] public Transform Center { get; private set; }
    [SerializeField] private List<Collider> _wallColliders = new();

    [SerializeField] private int raycastCols;
    [SerializeField] private int raycastRows;

    public int MaskID { get; private set; }

    private bool _isPlaced = true;
    private Portal _otherPortal;
    public List<PortableObject> _portalObjects = new();

    private Material _material;
    private Renderer _renderer;
    private PortalSet _portalSet;
    private Action<Portal> _onWarp;
    private Collider _collider;
    private bool _playerIsInTrigger;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        _collider = GetComponent<Collider>();
    }

    public void Init(Portal otherPortal, int maskID, PortalSet portalSet, Action<Portal> onWarp)
    {
        _otherPortal = otherPortal;
        MaskID = maskID;
        _material.SetInt("_MaskID", maskID);
        _portalSet = portalSet;
        _onWarp += onWarp;
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _portalObjects.Count; ++i)
        {
            Vector3 objPos = Center.InverseTransformPoint(_portalObjects[i].transform.position);

            if (objPos.z < 0.0f)
            {
                _portalObjects[i].Warp();
                _portalSet.PortalLightObjects.ForEach((x) => x.Deactivate());
                _portalSet.PortalCamera.Render();
                _onWarp?.Invoke(this);
            }
        }
    }

    public Portal GetOtherPortal()
    {
        return _otherPortal;
    }

    public Color GetColour()
    {
        return portalColour;
    }

    public void SetColour(Color colour)
    {
        _material.SetColor("_Colour", colour);
        outlineRenderer.material.SetColor("_OutlineColour", colour);
    }

    public void SetTexture(RenderTexture tex)
    {
        _material.mainTexture = tex;
    }

    public bool IsRendererVisible()
    {
        return _renderer.isVisible;
        // Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // return GeometryUtility.TestPlanesAABB(planes, _renderer.bounds);
        // var cam = Camera.main;
        // if (_renderer == null || cam == null)
        //     return false;
        //
        // // Get the bounding box of the renderer
        // Bounds bounds = _renderer.bounds;
        //
        // // Get the 8 corners of the bounding box
        // Vector3[] corners = new Vector3[8];
        // Vector3 min = bounds.min;
        // Vector3 max = bounds.max;
        //
        // corners[0] = cam.WorldToViewportPoint(new Vector3(min.x, min.y, min.z));
        // corners[1] = cam.WorldToViewportPoint(new Vector3(min.x, min.y, max.z));
        // corners[2] = cam.WorldToViewportPoint(new Vector3(min.x, max.y, min.z));
        // corners[3] = cam.WorldToViewportPoint(new Vector3(min.x, max.y, max.z));
        // corners[4] = cam.WorldToViewportPoint(new Vector3(max.x, min.y, min.z));
        // corners[5] = cam.WorldToViewportPoint(new Vector3(max.x, min.y, max.z));
        // corners[6] = cam.WorldToViewportPoint(new Vector3(max.x, max.y, min.z));
        // corners[7] = cam.WorldToViewportPoint(new Vector3(max.x, max.y, max.z));
        //
        // foreach (var point in corners)
        // {
        //     // Check if the point is in front of the camera and inside the viewport
        //     if (point.z > 0 &&
        //         point.x >= 0 && point.x <= 1 &&
        //         point.y >= 0 && point.y <= 1)
        //     {
        //         return true; // At least one corner is visible
        //     }
        // }
        //
        // return false; // No corners are visible
    }

    public bool RaycastCheck()
    {
       BoxCollider boxCollider = GetComponent<BoxCollider>();
       Vector3 halfSize = boxCollider.size * 0.5f;
       float xStep = (boxCollider.size.x) / raycastCols;
       float yStep = (boxCollider.size.y) / raycastRows;
       for (int i = 0; i <= raycastCols; i++)
       {
           for (int j = 0; j <= raycastRows; j++)
           {
               Vector3 localOffset = new Vector3(halfSize.x, -halfSize.y, halfSize.z);
               Vector3 localCorner = boxCollider.center + localOffset + new Vector3(-xStep * i, yStep * j, 0.0f);
               var raycastPos = boxCollider.transform.TransformPoint(localCorner);
               RaycastHit hit;
               if (!Physics.Raycast(Camera.main.transform.position, (raycastPos - Camera.main.transform.position),
                       out hit, Mathf.Infinity)) continue;
               
               if (hit.collider == boxCollider)
               {
                   return true;
               }
           }
       }
       
       return false;
    }

    public bool IsPlayerInPortal()
    {
        return _playerIsInTrigger;
    }

    public bool IsVisibleFromCamera()
    {
        Vector3 dirToTarget = Center.position - Camera.main.transform.position;
        Ray ray = new Ray(Camera.main.transform.position, dirToTarget);

        if (Physics.Raycast(ray, out RaycastHit hit, dirToTarget.magnitude, Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Collide))
        {
            // If the hit object is the target, it's visible
            return hit.transform == transform;
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.GetComponent<PortableObject>();
        if (obj != null)
        {
            _portalObjects.Add(obj);
            obj.SetIsInPortal(this, _otherPortal, _wallColliders);
        }

        var player = other.GetComponent<FirstPersonController>();
        if (player != null)
        {
            _playerIsInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<PortableObject>();

        if (_portalObjects.Contains(obj))
        {
            _portalObjects.Remove(obj);
            obj.ExitPortal(_wallColliders);
        }
        
        var player = other.GetComponent<FirstPersonController>();
        if (player != null)
        {
            _playerIsInTrigger = false;
        }
    }

// public void PlacePortal(Collider wallCollider, Vector3 pos, Quaternion rot)
// {
//     this._wallColliders = wallCollider;
//     transform.position = pos;
//     transform.rotation = rot;
//     transform.position -= transform.forward * 0.001f;
//
//     FixOverhangs();
//     FixIntersects();
// }

// Ensure the portal cannot extend past the edge of a surface.
    private void FixOverhangs()
    {
        var testPoints = new List<Vector3>
        {
            new Vector3(-1.1f, 0.0f, 0.1f),
            new Vector3(1.1f, 0.0f, 0.1f),
            new Vector3(0.0f, -2.1f, 0.1f),
            new Vector3(0.0f, 2.1f, 0.1f)
        };

        var testDirs = new List<Vector3>
        {
            Vector3.right,
            -Vector3.right,
            Vector3.up,
            -Vector3.up
        };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = transform.TransformPoint(testPoints[i]);
            Vector3 raycastDir = transform.TransformDirection(testDirs[i]);

            if (Physics.CheckSphere(raycastPos, 0.05f, placementMask))
            {
                break;
            }
            else if (Physics.Raycast(raycastPos, raycastDir, out hit, 2.1f, placementMask))
            {
                var offset = hit.point - raycastPos;
                transform.Translate(offset, Space.World);
            }
        }
    }

// Ensure the portal cannot intersect a section of wall.
    private void FixIntersects()
    {
        var testDirs = new List<Vector3>
        {
            Vector3.right,
            -Vector3.right,
            Vector3.up,
            -Vector3.up
        };

        var testDists = new List<float> { 1.1f, 1.1f, 2.1f, 2.1f };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = transform.TransformPoint(0.0f, 0.0f, -0.1f);
            Vector3 raycastDir = transform.TransformDirection(testDirs[i]);

            if (Physics.Raycast(raycastPos, raycastDir, out hit, testDists[i], placementMask))
            {
                var offset = (hit.point - raycastPos);
                var newOffset = -raycastDir * (testDists[i] - offset.magnitude);
                transform.Translate(newOffset, Space.World);
            }
        }
    }

// Once positioning has taken place, ensure the portal isn't intersecting anything.
    private bool CheckOverlap()
    {
        var checkPosition = transform.position - new Vector3(0.0f, 0.0f, 0.1f);
        var checkExtents = new Vector3(0.9f, 1.9f, 0.05f);
        if (Physics.CheckBox(checkPosition, checkExtents, transform.rotation, placementMask))
        {
            return false;
        }

        return true;
    }

    public void RemovePortal()
    {
        gameObject.SetActive(false);
        _isPlaced = false;
    }

    public bool IsPlaced()
    {
        return _isPlaced;
    }
    
   void OnDrawGizmosSelected()
   {
       BoxCollider collider = GetComponent<BoxCollider>();
       Transform t = collider.transform;
       Vector3 halfSize = collider.size * 0.5f;
       float xStep = (collider.size.x) / raycastCols;
       float yStep = (collider.size.y) / raycastRows;
       for (int i = 0; i <= raycastCols; i++)
       {
           for (int j = 0; j <= raycastRows; j++)
           {
               Vector3 localOffset = new Vector3(halfSize.x, -halfSize.y, halfSize.z);
               Vector3 localCorner = collider.center + localOffset + new Vector3(-xStep * i, yStep * j, 0.0f);
               var raycastPos = collider.transform.TransformPoint(localCorner);
               RaycastHit hit;
               if (Physics.Raycast(Camera.main.transform.position, (raycastPos - Camera.main.transform.position), out hit, Mathf.Infinity))
               {
                   if (hit.collider == collider)
                   {
                       Gizmos.color = Color.green;
                   }
                   else
                   {
                       Gizmos.color = Color.red;
                   }
                   Gizmos.DrawRay(Camera.main.transform.position, (raycastPos - Camera.main.transform.position));
               }
           }
       }
   } 
}