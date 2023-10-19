using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    public GameObject stack;
    GameObject stcakChild;
    public GameObject[] stacks;
    int stackCount;
    public int stack›ndex;
    private float posY = 10;

    void Start()
    {
        stackCount = stack.transform.childCount;
        stacks = new GameObject[stackCount];
        for (int i = 0; i < stackCount; i++)
        {
            stacks[i] = stack.transform.GetChild(i).gameObject;
        }
        stack›ndex = stackCount - 1;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetStack();
        }
    }

    void GetStack()
    {
        if (stack›ndex < 0)
            stack›ndex = stackCount - 1;

        stacks[stack›ndex].transform.position += new Vector3(0, posY, 0);
        stack›ndex--;
    }
}
