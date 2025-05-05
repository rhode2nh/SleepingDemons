using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FakeWall : MonoBehaviour
{
    [SerializeField] private LightBulb _lightBulb;
    [SerializeField] private List<GameObject> _walls;

    private void Update()
    {
        if (!_lightBulb.IsOn || _lightBulb.IsRemoved)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
