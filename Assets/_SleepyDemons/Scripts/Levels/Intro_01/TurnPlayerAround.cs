using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPlayerAround : MonoBehaviour
{
    private bool playerEntered = false;
    private void OnTriggerEnter(Collider other)
    {
        CharacterController player = other.GetComponent<CharacterController>();
        if (player == null) return;
        if (playerEntered) return;
        
        player.enabled = false;
        Vector3 localPos = transform.InverseTransformPoint(player.transform.position);
        player.transform.position = transform.TransformPoint(new Vector3(-localPos.x, localPos.y, localPos.z));
        player.enabled = true;
        Physics.SyncTransforms();
        playerEntered = true;
        player.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterController player = other.GetComponent<CharacterController>();
        if (player == null) return;
        if (playerEntered) playerEntered = false;
    }
}
