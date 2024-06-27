using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public GameObject objectToLookAt;
    public float lookAtSpeed;

    // Update is called once per frame
    void Update()
    {
        if (objectToLookAt != null) {
            // transform.LookAt(objectToLookAt.transform.position, transform.up);
            Vector3 toTarget = objectToLookAt.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(toTarget);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
        }
    }
}
