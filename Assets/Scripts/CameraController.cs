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
    [SerializeField][Range(0, 1)] float endSmoothness = 0.12f;
    Transform target;
    Stack stack;
    bool once;
    public Vector3 pos;
    public Vector3 endPosition;
    private void Start()
    {
        once = true;
        stack = Stack.instance;
    }
    void FixedUpdate()
    {
        if (stack.gameOver)
        {

            if (once)
            {
                endPosition = transform.position + new Vector3(stack.posY / 2, stack.posY / 3, -stack.posY / 2);

                once = false;
            }
            pos = Vector3.Lerp(transform.position, endPosition, endSmoothness);
            transform.position = pos;
        }

        else
        {
            float posY = stack.posY + delayY;
            Vector3 desiredPosition = new Vector3(transform.position.x, posY, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothness);
            transform.position = smoothedPosition;
        }



    }
}
