using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singelton
    public static CameraController instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    #endregion

    [SerializeField] float delayY;
    [SerializeField][Range(0, 1)] float smoothness = 0.12f;
    Transform target;
    Stack stack;
    private void Start()
    {
        stack = Stack.instance;
    }
    void FixedUpdate()
    {
        float posY = stack.posY + delayY;
        Vector3 desiredPosition = new Vector3(transform.position.x, posY, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothness);
        transform.position = smoothedPosition;
    }
}
