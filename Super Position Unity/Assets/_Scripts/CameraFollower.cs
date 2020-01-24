using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [Header("Followed objects Data")]
    [SerializeField] private Transform followedObject = null;

    [Header("Follow Data")]
    [SerializeField] private Vector3 followOffset = Vector3.zero;
    [SerializeField] private Vector3 lookAtOffset = Vector3.zero;
    [SerializeField] [Range(0, 1)] private float followSpeed = 0;

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, followedObject.position + followOffset, followSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((followedObject.position + lookAtOffset) - transform.position), followSpeed);
        //transform.LookAt(followedObject.position + lookAtOffset);
    }
}
