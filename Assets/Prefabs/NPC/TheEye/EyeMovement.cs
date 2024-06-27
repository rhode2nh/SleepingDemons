using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    public float minSeconds;
    public float maxSeconds;

    public float minLookArea;
    public float maxLookArea;
    public float minRotation;
    public float maxRotation;
    public float rotationSpeed;
    public float zFocusOffset;
    public float delay;
    public bool lookAtPlayer;
    public float lookAtDelay;
    private Quaternion targetRotation;
    private Vector3 pointToLookAt;
    private Vector3 initialForwardDirection;
    private Vector3 initialUpDirection;
    // Start is called before the first frame update
    void Awake()
    {
        targetRotation = transform.rotation;
        initialForwardDirection = transform.forward;
        initialUpDirection = transform.up;
    }

    void OnEnable() {
        StopAllCoroutines();
        if (lookAtPlayer) {
            StartCoroutine(LookAtPlayer());
        } else {
            StartCoroutine(RandomEyeMovements());
        }
    }

    IEnumerator LookAtPlayer() {
        while (true) {
            // transform.LookAt(Camera.main.transform.position, transform.up);
            Vector3 toTarget = Camera.main.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(toTarget);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5.0f * Time.deltaTime);
            yield return new WaitForSeconds(lookAtDelay);
        }
    }

    IEnumerator RandomEyeMovements() {
        while (true) {
            // var xArea = Random.Range(minLookArea, maxLookArea);
            // var yArea = Random.Range(minLookArea, maxLookArea);
            // pointToLookAt = new Vector3(xArea + transform.forward.x, yArea + transform.forward.y, zFocusOffset + transform.forward.z);
            // targetRotation = Quaternion.LookRotation(pointToLookAt - transform.position);

            // Vector3 directionWithoutSpread = targetPoint - projectileSpawner.position;
            // float x = UnityEngine.Random.Range(-totalXSpread * 0.5f, totalXSpread * 0.5f);
            // float y = UnityEngine.Random.Range(-totalYSpread * 0.5f, totalYSpread * 0.5f);
            // Vector3 directionWithSpread = Quaternion.AngleAxis(x, projectileSpawner.up) * Quaternion.AngleAxis(y, projectileSpawner.forward) * directionWithoutSpread;

            Vector3 forwardDirection = initialForwardDirection * zFocusOffset;
            var xArea = Random.Range(minLookArea, maxLookArea);
            var yArea = Random.Range(minLookArea, maxLookArea);
            Vector3 directionToRotate = Quaternion.AngleAxis(xArea, initialUpDirection) * Quaternion.AngleAxis(yArea, initialForwardDirection) * forwardDirection;
            targetRotation = Quaternion.LookRotation(directionToRotate);

            while (transform.rotation != targetRotation) {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitForSeconds(Random.Range(minSeconds, maxSeconds));
        }
    }
}
