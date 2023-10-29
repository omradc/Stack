using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

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

    [Header("UNITY SETUP")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject scoreObj;
    public GameObject menuPanel;
    public GameObject stackPrefab;
    public GameObject stackParent;

    [Header("GAME SETUP")]
    public float stackSpeed;
    public float failTolerance;
    public float comboScale;
    public float comboNumber;
    public float posX = -5;
    public float posZ = 5;
    public float stackHeight = 0.5f;
    public float stackWidth = 3f;
    public float posY;
    float combo;
    float highScore;
    float score;
    float currentSpeed;
    float rePosition;

    [HideInInspector] public GameObject currentStack;
    GameObject stackA;
    GameObject stackB;

    [HideInInspector] public bool gameOver;
    bool restartGame;
    bool zAxisMovement;
    bool startGame;
    bool touchedTheScreen;

    Vector3 stackScale;
    Vector3 trashPos;
    Vector3 trashScale;

    Color color;
    void Start()
    {
        color = Color.instance;
        stackScale = new Vector3(stackWidth, stackHeight, stackWidth);
        highScoreText.text = $"Best: {PlayerPrefs.GetFloat("High Score")}";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && restartGame == true)
            SceneManager.LoadScene(0);

        if (Input.GetKeyDown(KeyCode.Space) && restartGame == true)
            SceneManager.LoadScene(0);

        if (gameOver)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (StackControl())
            {
                menuPanel.SetActive(false);

                CreateStack();
                posY += stackHeight;
                Score();
                startGame = true;
                touchedTheScreen = true;
            }
            else
            {
                GameOver();
            }

        }

        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
            touchedTheScreen = false;

        MoveStack();
    }

    void CreateStack()
    {
        currentStack = Instantiate(stackPrefab, transform.position, transform.rotation);
        currentStack.transform.SetParent(stackParent.transform);
        currentStack.transform.localScale = stackScale;

        color.StackColor(currentStack);

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
                {
                    Combo(new Vector3(0, 0, comboScale));
                }
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
                {
                    Combo(new Vector3(comboScale, 0, 0));
                }
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
        cube.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        color.StackColor(cube);
    }

    void Combo(Vector3 scale)
    {
        combo++;

        if (combo >= comboNumber)
        {
            stackA.transform.localScale += scale;
            stackScale = stackA.transform.localScale;
        }
    }

    void GameOver()
    {
        gameOver = true;
        print("Game Over");
        currentStack.AddComponent<Rigidbody>();

        StartCoroutine("RestartDelay");
    }

    IEnumerator RestartDelay()
    {
        yield return new WaitForSeconds(2);
        restartGame = true;
    }

    void Score()
    {
        scoreObj.SetActive(true);
        score = (posY * 2) - 1;
        scoreText.text = $"{score}";

        if (score > PlayerPrefs.GetFloat("High Score"))
        {
            highScore = score;
            PlayerPrefs.SetFloat("High Score", highScore);
        }

    }
}
