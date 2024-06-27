using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IHittable
{
    [SerializeField] private float _health;
    [SerializeField] private GameObject _ragdoll;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float damage, Vector3 forceDir, Vector3 torqueDir) {
        _health -= damage;
        if (_health <= 0) {
            Die(forceDir, torqueDir);
        }
        // _rb.AddForce(forceDir, ForceMode.Impulse);
        // _rb.AddTorque(torqueDir, ForceMode.Impulse);
    }

    private void Die(Vector3 forceDir, Vector3 torqueDir) {
        var instantiatedRagdoll = Instantiate(_ragdoll, transform.position, transform.rotation);
        instantiatedRagdoll.GetComponentInChildren<Rigidbody>().AddForce(forceDir, ForceMode.Impulse);
        instantiatedRagdoll.GetComponentInChildren<Rigidbody>().AddTorque(torqueDir, ForceMode.Impulse);
        Destroy(transform.root.gameObject);
    }
}
