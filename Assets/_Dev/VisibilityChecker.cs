using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisibilityChecker : MonoBehaviour
{
    private List<GameObject> _targetObjects;
    private List<Renderer> _objRenderers;
    [SerializeField, Range(-1, 1)] private float _dotCheck;
    [SerializeField, ReadOnly] private bool _initiallyInvisible;

    private void Start()
    {
        _objRenderers = GetComponentsInChildren<Renderer>().ToList();
    }

    private void Update()
    {
        foreach (var objRenderer in _objRenderers)
        {
            objRenderer.gameObject.SetActive(IsVisible(objRenderer));
        }
    }

    private bool IsVisible(Renderer objRenderer)
    {
        bool isLookingInSameDirection = IsLookingInSameDirection(objRenderer.transform, Camera.main.transform);
        bool isInFront = IsInFront(objRenderer.transform, Camera.main.transform);
        if (!isInFront)
        {
            _initiallyInvisible = !isLookingInSameDirection;
            return false;
        }

        if (isLookingInSameDirection)
        {
            _initiallyInvisible = false;
            return false;
        }
        if (_initiallyInvisible) return false;
        
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        return GeometryUtility.TestPlanesAABB(planes, objRenderer.bounds);
    }

    private bool IsLookingInSameDirection(Transform reference, Transform target)
    {
        return Vector3.Dot(reference.forward, target.forward) >= _dotCheck;
    }

    private bool IsInFront(Transform reference, Transform target)
    {
        var toTarget = (target.position - reference.position).normalized;
        return Vector3.Dot(reference.forward, toTarget) > 0f;
    }
}
