using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    public GameObject stack;
    GameObject stcakChild;
    public GameObject[] stacks;
    int stackCount;
    public int stackİndex;
    private float posY = 10;

    void Start()
    {
        stackCount = stack.transform.childCount;
        stacks = new GameObject[stackCount];
        for (int i = 0; i < stackCount; i++)
        {
            stacks[i] = stack.transform.GetChild(i).gameObject;
        }
        stackİndex = stackCount - 1;
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
        if (stackİndex < 0)
            stackİndex = stackCount - 1;

        stacks[stackİndex].transform.position += new Vector3(0, posY, 0);
        stackİndex--;
    }
}
