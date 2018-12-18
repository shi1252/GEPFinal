using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private void LateUpdate()
    {
        Vector3 offset = new Vector3(0.0f, 15.0f, 0.0f);
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime);
    }
}