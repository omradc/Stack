using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public TextMeshProUGUI scoreText;
    public float stackSpeed;
    public float failTolerance;
    public GameObject stackPrefab;
    public GameObject stackParent;
    public float comboScale;
    public float comboNumber;
    public float posX = -5;
    public float posZ = 5;
    public float stackHeight = 0.5f;
    public float stackWidth = 3f;
    public float posY;


    GameObject currentStack;
    GameObject stackA;
    GameObject stackB;

    float combo;
    float currentSpeed;
    float rePosition;

    bool zAxisMovement;
    bool startGame;
    bool touchedTheScreen;
    bool gameOver;

    Vector3 stackScale;
    Vector3 trashPos;
    Vector3 trashScale;
    void Start()
    {
        stackScale = new Vector3(stackWidth, stackHeight, stackWidth);
        scoreText.text = $"{0}";
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
                posY += stackHeight;
                scoreText.text = $"{posY * 2}";
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
        currentStack = Instantiate(stackPrefab, transform.position, transform.rotation);
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
            if (Mathf.Abs(minus) <= failTolerance)
            {
                minus = 0;
                stackA.transform.position = stackB.transform.position + new Vector3(0, stackHeight, 0);

                if (stackA.transform.localScale.z < stackWidth)
                    Combo(new Vector3(0, 0, comboScale));
            }


            else
            {
                combo = 0;

                // Stack pos / stack scale
                stackScale.z -= Mathf.Abs(minus);
                if (stackScale.z < 0)
                    return false;
                stackA.transform.localScale = stackScale;
                float mid = (stackA.transform.position.z + stackB.transform.position.z) / 2;
                stackA.transform.position = new Vector3(stackA.transform.position.x, posY, mid);


                //Trash
                if (stackA.transform.position.z > stackB.transform.position.z)
                    trashPos = new Vector3(stackA.transform.position.x, stackA.transform.position.y, stackA.transform.position.z + stackA.transform.localScale.z);
                else
                    trashPos = new Vector3(stackA.transform.position.x, stackA.transform.position.y, stackA.transform.position.z - stackA.transform.localScale.z);

                trashScale = new Vector3(stackA.transform.localScale.x, stackHeight, minus);
                if (minus != 0)
                    CreateTrash(trashPos, trashScale);
            }

            rePosition = stackA.transform.position.z;


        }
        else
        {
            float minus = stackA.transform.position.x - stackB.transform.position.x;

            // Fail tolarence
            if (Mathf.Abs(minus) <= failTolerance)
            {
                minus = 0;
                stackA.transform.position = stackB.transform.position + new Vector3(0, stackHeight, 0);

                if (stackA.transform.localScale.z < stackWidth)
                    Combo(new Vector3(comboScale, 0, 0));
            }


            else
            {
                combo = 0;

                // Stack pos / stack scale
                stackScale.x -= Mathf.Abs(minus);
                if (stackScale.x < 0)
                    return false;
                stackA.transform.localScale = stackScale;
                float mid = (stackA.transform.position.x + stackB.transform.position.x) / 2;
                stackA.transform.position = new Vector3(mid, posY, stackA.transform.position.z);

                //Trash
                if (stackA.transform.position.x > stackB.transform.position.x)
                    trashPos = new Vector3(stackA.transform.position.x + stackA.transform.localScale.x, stackA.transform.position.y, stackA.transform.position.z);
                else
                    trashPos = new Vector3(stackA.transform.position.x - stackA.transform.localScale.x, stackA.transform.position.y, stackA.transform.position.z);

                trashScale = new Vector3(minus, stackHeight, stackA.transform.localScale.z);
                if (minus != 0)
                    CreateTrash(trashPos, trashScale);
            }

            rePosition = stackA.transform.position.x;

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

    void Combo(Vector3 scale)
    {
        combo++;

        if (combo >= comboNumber)
        {
            print("ok");
            stackA.transform.localScale += scale;
            stackScale = stackA.transform.localScale;
        }
    }

    void GameOver()
    {
        gameOver = true;
        print("Game Over");
        currentStack.AddComponent<Rigidbody>();

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine("RestartGame");
        }

    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
