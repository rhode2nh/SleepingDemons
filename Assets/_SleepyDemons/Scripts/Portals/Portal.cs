using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [SerializeField] private Renderer outlineRenderer;
    [SerializeField] private Color portalColour;
    [SerializeField] private LayerMask placementMask;
    [field: SerializeField] public Transform Center { get; private set; }
    [SerializeField] private List<Collider> _wallColliders = new(); 

    public int MaskID { get; private set; }
    
    private bool _isPlaced = true;
    private Portal _otherPortal;
    public List<PortableObject> _portalObjects = new();

    private Material _material;
    private Renderer _renderer;
    private PortalSet _portalSet;
    private Action<Portal> _onWarp;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
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
        // return _renderer.isVisible;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        return GeometryUtility.TestPlanesAABB(planes, _renderer.bounds);
    }
    
    public bool IsVisibleFromCamera()
    {
        Vector3 dirToTarget = Center.position - Camera.main.transform.position;
        Ray ray = new Ray(Camera.main.transform.position, dirToTarget);

        if (Physics.Raycast(ray, out RaycastHit hit, dirToTarget.magnitude, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
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
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<PortableObject>();

        if(_portalObjects.Contains(obj))
        {
            _portalObjects.Remove(obj);
            obj.ExitPortal(_wallColliders);
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
            new Vector3(-1.1f,  0.0f, 0.1f),
            new Vector3( 1.1f,  0.0f, 0.1f),
            new Vector3( 0.0f, -2.1f, 0.1f),
            new Vector3( 0.0f,  2.1f, 0.1f)
        };

        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        for(int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = transform.TransformPoint(testPoints[i]);
            Vector3 raycastDir = transform.TransformDirection(testDirs[i]);

            if(Physics.CheckSphere(raycastPos, 0.05f, placementMask))
            {
                break;
            }
            else if(Physics.Raycast(raycastPos, raycastDir, out hit, 2.1f, placementMask))
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
}
