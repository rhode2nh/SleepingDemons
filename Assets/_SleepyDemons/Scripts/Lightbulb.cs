using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class LightBulb : Interactable, IDamageable
{
    [field: SerializeField] public LightBulbData LightBulbSpawnObject { get; private set; }
    public bool IsOn { get; private set; }
    public bool IsRemoved { get; private set; }
    
    [SerializeField] private float health;
    [SerializeField] private GameObject destroyedBulb;
    [SerializeField] private ParticleSystem sparks;
    [FormerlySerializedAs("lightController2")] [SerializeField] private LightController lightController;
    [SerializeField] private LensFlare flare;

    private LightBulbData _lightBulbData;
    private List<Light> _lightSources;

    private void Awake()
    {
        _lightBulbData = GetComponent<LightBulbData>();
        _lightSources = GetComponentsInChildren<Light>().ToList();
    }
    
    private void OnEnable()
    {
        if (lightController != null)
        {
            lightController.OnDimLightBulb += DimLightBulb;
        }
    }

    private void OnDisable()
    {
        if (lightController != null)
        {
            lightController.OnDimLightBulb -= DimLightBulb;
        }
    }

    public void Replace(LightBulbData lightBulbData)
    {
        gameObject.SetActive(true);
        IsRemoved = false;
        _lightBulbData.Init(lightBulbData);
        Destroy(lightBulbData.gameObject);
    }

    public void TakeDamage(float damage, Vector3 force, Vector3 torque)
    {
        health -= damage;
        if (health < 0)
        {
            gameObject.SetActive(false);
            Instantiate(destroyedBulb, transform.position, transform.rotation, transform.parent);
            var instantiatedSparks = Instantiate(sparks, transform.position, sparks.transform.rotation);
            instantiatedSparks.Play();
        }
    }

    private void DimLightBulb(float intensity)
    {
        bool turnOn = intensity > 0.0f;
        IsOn = turnOn;
        foreach (var lightSource in _lightSources)
        {
            lightSource.intensity = intensity;
            lightSource.enabled = turnOn;
        }

        flare.brightness = intensity;
    }

    private void OnDrawGizmos()
    {
        _lightSources = GetComponentsInChildren<Light>().ToList();
        if (_lightSources.Count == 0) return;
        
        foreach (var lightSource in _lightSources)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, lightSource.transform.position);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody>();
        var myRb = GetComponent<Rigidbody>();
        if (myRb != null)
        {
            if (myRb.linearVelocity.magnitude > 3.0f)
                TakeDamage(myRb.linearVelocity.magnitude, new Vector3(), new Vector3());
        }
        if (rb == null) return;
        
        TakeDamage(rb.linearVelocity.magnitude, new Vector3(), new Vector3());
    }

    public override void ExecuteInteraction(GameObject other)
    {
        if (lightController == null) return;
        IsRemoved = true;
        IsOn = false;
        lightController.RemoveLightBulb(this);
    }

    public void Spawn()
    {
        LightBulbData lightBulbData = Instantiate(LightBulbSpawnObject, transform.position, transform.rotation, null);
        lightBulbData.Init(_lightBulbData);
        lightBulbData.GetComponent<Holdable>().Hold(PlayerManager.Instance.InputRaycast.hit.point, PlayerManager.Instance.IsHolding);
    }
}
