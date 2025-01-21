using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Serialization;

public class PullChain : Holdable
{
    [SerializeField] internal float pullDistance;
    [SerializeField] internal Rigidbody anchor;
    [SerializeField] internal Light lightBulb;
    [SerializeField] internal float emissiveIntensity;
    [SerializeField] internal Renderer lampShadeRenderer;
    [SerializeField] private float tension;
    [SerializeField] private float pullStrength;
    [SerializeField] private float maxPullLength;
    [SerializeField] private AudioSource lampSwitchOn;
    [SerializeField] private AudioSource lampSwitchOff;

    private Vector3 initialPos;
    private Vector3 initialAnchorPos;
    private Color _emissiveColor;

    public void Init(float pullDistance, Renderer lampShadeRenderer, Rigidbody anchor, Light lightBulb, float emissiveIntensity, AudioSource lampSwitchOn, AudioSource lampSwitchOff)
    {
        this.pullDistance = pullDistance;
        this.lampShadeRenderer = lampShadeRenderer;
        this.anchor = anchor;
        this.lightBulb = lightBulb;
        this.emissiveIntensity = emissiveIntensity;
        this.lampSwitchOn = lampSwitchOn;
        this.lampSwitchOff = lampSwitchOff;
    }
    
    void Start()
    {
        initialAnchorPos = anchor.position;
        _emissiveColor = lampShadeRenderer.material.GetColor("_EmissionColor");
    }
    
    public override void ExecuteInteraction(GameObject other)
    {
        var interact = other.gameObject.GetComponent<Interact>();
        base.ExecuteInteraction(other);
        StartCoroutine(MoveAnchor(interact));
    }

    IEnumerator MoveAnchor(Interact interact)
    {
        var initialAncPos = anchor.position;
        var relativeDistance = Vector3.Distance(Camera.main.transform.position, interact._inputRaycast.hit.point);
        var initialReferencePoint = interact._inputRaycast.hit.point;
        var distanceToTravel = Vector3.Distance(initialReferencePoint, Camera.main.transform.position + (Camera.main.transform.forward * relativeDistance));
        var lightSwitched = false;
        while (PlayerManager.instance.isHolding)
        {
            if (anchor.position.y > initialAncPos.y - maxPullLength && anchor.position.y <= initialAncPos.y)
            {
                var calculatedDir = (Camera.main.transform.forward + new Vector3(0, distanceToTravel, 0)).normalized;
                var posToHold = Camera.main.transform.position + (calculatedDir * relativeDistance);
                var relativePointToHold = initialReferencePoint + (anchor.position - initialAncPos);
                anchor.MovePosition(anchor.position + new Vector3(0, (posToHold.y - relativePointToHold.y), 0) * pullStrength * Time.deltaTime);
            }
            else if (anchor.position.y > initialAncPos.y)
            {
                anchor.MovePosition(initialAncPos);
                lightSwitched = false;
            } 

            if (initialAncPos.y - anchor.position.y > pullDistance && !lightSwitched)
            {
                lampSwitchOn.Play();
                lightSwitched = true;
                lightBulb.enabled = !lightBulb.enabled;
                if (lightBulb.enabled)
                {
                    lampShadeRenderer.material.SetColor("_EmissionColor", _emissiveColor * emissiveIntensity);
                }
                else
                {
                    lampShadeRenderer.material.SetColor("_EmissionColor", _emissiveColor * 0);
                }
            }
            yield return new WaitForEndOfFrame();
        }

        while (anchor.position.y < initialAnchorPos.y)
        {
            Vector3 newPos = new Vector3(0, initialAnchorPos.y, 0);
            anchor.MovePosition(anchor.position + newPos * tension * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        if (lightSwitched)
        {
            lampSwitchOff.Play();
        }
    }

    IEnumerator CheckDistance()
    {
        initialPos = transform.position;
        while (true)
        {
            if (!_isHolding)
            {
                break;
            }
            if (Vector3.Distance(initialAnchorPos, transform.position) > Vector3.Distance(initialAnchorPos, initialPos) + pullDistance)
            {
                lightBulb.enabled = !lightBulb.enabled;
                if (lightBulb.enabled)
                {
                    lampShadeRenderer.material.SetColor("_EmissionColor", _emissiveColor * emissiveIntensity);
                }
                else
                {
                    lampShadeRenderer.material.SetColor("_EmissionColor", _emissiveColor * 0);
                }
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
