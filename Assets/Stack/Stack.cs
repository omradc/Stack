using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    #region Singelton
    public static Stack instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    #endregion
    public float stackSpeed;
    public float failTolerance;
    public GameObject stackPrefab;
    public GameObject stackParent;
    public GameObject currentStack;

    public float posX = -5;
    public float posY = 10;
    public float posZ = 5;



    public GameObject stackA;
    public GameObject stackB;

    float currentSpeed;
    bool zAxisMovement;
    bool startGame;
    bool touchedTheScreen;
    bool gameOver;

    Vector3 stackScale;
    Vector3 trashPos;
    Vector3 trashScale;
    float rePosition;
    void Start()
    {
        stackScale = new Vector3(3, 1, 3);
    }

    void Update()
    {
        if (gameOver)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (StackControl())
            {
                CreateStack();
                posY++;
                startGame = true;
                touchedTheScreen = true;
            }
            else
            {
                GameOver();
            }

        }

        if (Input.GetMouseButtonUp(0))
            touchedTheScreen = false;

        MoveStack();
    }

    void CreateStack()
    {
        currentStack = Instantiate(stackPrefab, currentStack.transform.position, currentStack.transform.rotation);
        currentStack.transform.SetParent(stackParent.transform);
        currentStack.transform.localScale = stackScale;

        zAxisMovement = !zAxisMovement;
    }

    void MoveStack()
    {
        if (!startGame)
            return;

        if (zAxisMovement)
        {
            if (touchedTheScreen)
            {
                currentStack.transform.position = new Vector3(rePosition, posY, posZ);
            }

            if (currentStack.transform.position.z >= posZ)
            {
                currentSpeed = stackSpeed;
            }

            if (currentStack.transform.position.z <= -posZ)
            {
                currentSpeed = -stackSpeed;
            }


            currentStack.transform.Translate(0, 0, -1 * currentSpeed * Time.deltaTime);
        }

        else
        {
            if (touchedTheScreen)
            {
                currentStack.transform.position = new Vector3(posX, posY, rePosition);
            }

            if (currentStack.transform.position.x <= posX)
            {
                currentSpeed = stackSpeed;
            }

            if (currentStack.transform.position.x >= -posX)
            {
                currentSpeed = -stackSpeed;

            }

            currentStack.transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
        }

    }

    bool StackControl()
    {
        /* 
           Stack sýralamasý
           1- currentStack
           2- stackA
           3- stackB
        */

        stackA = stackParent.transform.GetChild(stackParent.transform.childCount - 1).gameObject;
        stackB = stackParent.transform.GetChild(stackParent.transform.childCount - 2).gameObject;

        if (zAxisMovement)
        {
            float minus = stackA.transform.position.z - stackB.transform.position.z;
            // Fail tolarence
            if (Mathf.Abs(minus) < failTolerance)
            {
                minus = 0;
                stackA.transform.position = stackB.transform.position + new Vector3(0, 1, 0);
            }

            // Stack pos / stack scale
            stackScale.z -= Mathf.Abs(minus);
            if (stackScale.z < 0)
                return false;
            stackA.transform.localScale = stackScale;
            float mid = (stackA.transform.position.z + stackB.transform.position.z) / 2;
            stackA.transform.position = new Vector3(stackA.transform.position.x, posY, mid);
            rePosition = stackA.transform.position.z;

            //Trash
            if (stackA.transform.position.z > stackB.transform.position.z)
                trashPos = new Vector3(stackA.transform.position.x, stackA.transform.position.y, stackA.transform.position.z + stackA.transform.localScale.z);
            else
                trashPos = new Vector3(stackA.transform.position.x, stackA.transform.position.y, stackA.transform.position.z - stackA.transform.localScale.z);

            trashScale = new Vector3(stackA.transform.localScale.x, 1, minus);
            if (minus != 0)
                CreateTrash(trashPos, trashScale);



        }
        else
        {
            float minus = stackA.transform.position.x - stackB.transform.position.x;

            // Fail tolarence
            if (Mathf.Abs(minus) < failTolerance)
            {
                minus = 0;
                stackA.transform.position = stackB.transform.position + new Vector3(0, 1, 0);
            }

            stackScale.x -= Mathf.Abs(minus);
            if (stackScale.x < 0)
                return false;
            stackA.transform.localScale = stackScale;
            float mid = (stackA.transform.position.x + stackB.transform.position.x) / 2;
            stackA.transform.position = new Vector3(mid, posY, stackA.transform.position.z);
            rePosition = stackA.transform.position.x;

            //Trash
            if (stackA.transform.position.x > stackB.transform.position.x)
                trashPos = new Vector3(stackA.transform.position.x + stackA.transform.localScale.x, stackA.transform.position.y, stackA.transform.position.z);
            else
                trashPos = new Vector3(stackA.transform.position.x - stackA.transform.localScale.x, stackA.transform.position.y, stackA.transform.position.z);

            trashScale = new Vector3(minus, 1, stackA.transform.localScale.z);
            if (minus != 0)
                CreateTrash(trashPos, trashScale);
        }

        return true;

    }

    void CreateTrash(Vector3 pos, Vector3 scale)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = scale;
        cube.transform.position = pos;
        cube.AddComponent<Rigidbody>();
    }

    void GameOver()
    {
        gameOver = true;
        print("Game Over");
        currentStack.AddComponent<Rigidbody>();

    }
}
