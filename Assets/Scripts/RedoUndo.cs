using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RedoUndo : MonoBehaviour {

    public static Stack<GameObject[]> undoList = new Stack<GameObject[]>();
    public static Stack<GameObject[]> redoList = new Stack<GameObject[]>();
    static int count = 0;

    void OnEnable()
    {
        UndoButton.OnClick += UndoChange;
        RedoButton.OnClick += RedoChange;
    }

    void OnDisable()
    {
        UndoButton.OnClick -= UndoChange;
        RedoButton.OnClick -= RedoChange;
    }

    public static void AddUndo(GameObject[] objectList)
    {
        Debug.Log("Count: " + count++);
        undoList.Push(objectList);
    }

    public static bool HasUndo()
    {
        return undoList.Count > 0;
    }

    public static bool HasRedo()
    {
        return redoList.Count > 0;
    }

    void FixedUpdate()
    {
        //Debug.Log("Undo Count: " + undoList.Count);
        //Debug.Log("Redo Count: " + redoList.Count);
    }

    private void UndoChange()
    {
        if(undoList.Count < 0)
        {
            return;
        }

        GameObject[] currentItems = undoList.Pop();
        if (currentItems[0] == null)
        {
            currentItems[1].SetActive(false);
        }
        else if(currentItems[1] == null)
        {
            currentItems[0].SetActive(true);
        }
        else
        {
            currentItems[0].SetActive(true);
            currentItems[1].SetActive(false);
        }

        redoList.Push(currentItems);
    }

    private void RedoChange()
    {
        if (redoList.Count < 0)
        {
            return;
        }

        GameObject[] currentItems = redoList.Pop();
        if (currentItems[0] == null)
        {
            currentItems[1].SetActive(true);
        }
        else if (currentItems[1] == null)
        {
            currentItems[0].SetActive(false);
        }
        else
        {
            currentItems[0].SetActive(false);
            currentItems[1].SetActive(true);
        }

        undoList.Push(currentItems);
    }
}
