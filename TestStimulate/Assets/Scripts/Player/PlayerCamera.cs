using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    void Update()
    {
        Vector3 newPosition = this.gameObject.transform.position;
        newPosition.x = playerTransform.position.x;
        newPosition.z = playerTransform.position.z;
        this.gameObject.transform.position = newPosition;

        this.gameObject.transform.rotation = Quaternion.Euler(0, playerTransform.eulerAngles.y, 0);
    }
}
