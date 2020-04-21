using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{

    public static Dragon Instance;
    public bool Collected;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Error: Too many dragons!");
    }

    public void Pickup(Transform parent)
    {
        Collected = true;
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.rotation = parent.rotation;

    }

    public void Drop(Vector3 dropPosition, Quaternion dropRotation)
    {
        Collected = false;
        transform.SetParent(null);
        transform.position = dropPosition;
        transform.rotation = dropRotation;
    }
}
