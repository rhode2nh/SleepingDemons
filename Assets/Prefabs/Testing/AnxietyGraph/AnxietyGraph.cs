using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnxietyGraph : MonoBehaviour
{
    [SerializeField] private float yMin;

    [SerializeField] private float yMax;

    [SerializeField] private GameObject point;
    // Start is called before the first frame update
    void Start()
    {
        var position = point.transform.position;
        position = new Vector3(position.x, 0.0f, position.z);
        point.transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        var position = point.transform.position;
        Vector3 newPosition = new Vector3(position.x,
            SanityManager.Instance.AnxietyManager.Current * yMax, position.z);
        point.transform.position = newPosition;
    }
}
