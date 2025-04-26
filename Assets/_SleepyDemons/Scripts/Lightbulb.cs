using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LightBulb : MonoBehaviour, IDamageable
{
    [SerializeField] private float health;
    [SerializeField] private GameObject destroyedBulb;
    [SerializeField] private ParticleSystem sparks;
    [SerializeField] private List<Light> lightSources;
    [FormerlySerializedAs("lightController2")] [SerializeField] private LightController lightController;
    [SerializeField] private LensFlare flare;
    [SerializeField] public bool IsOn { get; private set; }

    private void OnEnable()
    {
        lightController.OnDimLightBulb += DimLightBulb;
    }

    private void OnDisable()
    {
        lightController.OnDimLightBulb -= DimLightBulb;
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
        foreach (var lightSource in lightSources)
        {
            lightSource.intensity = intensity;
            lightSource.enabled = turnOn;
        }

        flare.brightness = intensity;
    }

    private void OnDrawGizmos()
    {
        if (lightSources.Count <= 0) return;
        
        foreach (var lightSource in lightSources)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, lightSource.transform.position);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb == null) return;
        
        TakeDamage(rb.velocity.magnitude, new Vector3(), new Vector3());
    }
}
