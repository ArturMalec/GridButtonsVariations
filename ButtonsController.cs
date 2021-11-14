using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class ButtonsController : MonoBehaviour
{
    [SerializeField] TMP_InputField _InputRows;
    [SerializeField] TMP_InputField _InputColumns;

    private int[,] gridArray;
    private bool isGridGenerated = false;
    private static GridButton actualButton;
    public int Rows { get { return int.Parse(_InputRows.text); } }
    public int Columns { get { return int.Parse(_InputColumns.text); } }
    public static GridButton ActualButton { get { return actualButton; } set { actualButton = value; } }

    public void GenerateGrid()
    {
        ActualButton = null;

        if (isGridGenerated)
        {
            PoolingSystem.Instance.DeactivatePooledButtons();
            PoolingSystem.PoolSize = Rows * Columns;

            for (int i = 0; i < PoolingSystem.PoolSize; i++)
            {
                GridButton btn = PoolingSystem.Instance.GetPooledButton();
                if (btn != null)
                {
                    btn.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            PoolingSystem.PoolSize = Rows * Columns;
            PoolingSystem.Instance.InitializePooling();
            for (int i = 0; i < PoolingSystem.PoolSize; i++)
            {
                GridButton btn = PoolingSystem.Instance.GetPooledButton();
                if (btn != null)
                {
                    btn.gameObject.SetActive(true);
                }
            }
        }
        ResizeGrid();      
        SetGridButtonsCoordinates();
        SetGridButtonsStates();
        isGridGenerated = true;
    }

    private void ResizeGrid()
    {
        RectTransform parentTransform = GetComponent<RectTransform>();
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(parentTransform.rect.width / Columns, parentTransform.rect.height / Rows);
    }

    private void SetGridButtonsCoordinates()
    {
        gridArray = new int[Rows, Columns];
        int count = 0;
        for (int x = 1; x < gridArray.GetLength(0) + 1; x++)
        {
            for (int y = 1; y < gridArray.GetLength(1) + 1; y++)
            {
                PoolingSystem.Instance.PooledObjects[count].Coordinates = new Vector2Int(x, y);
                count++;
            }
        }
    }

    private void SetGridButtonsStates()
    {
        List<GridButton> listOfButtonsToBlock = new List<GridButton>();
        List<GridButton> listOfActiveObjects = new List<GridButton>();
        List<GridButton> gridButtonsList = PoolingSystem.Instance.PooledObjects;

        for (int i = 0; i < gridButtonsList.Count; i++)
        {
            if (gridButtonsList[i].gameObject.activeSelf)
            {
                listOfActiveObjects.Add(gridButtonsList[i]);
            }
        }
        
        int dividedListCount = (int)(listOfActiveObjects.Count * 0.1f);

        for (int i = 0; i < gridButtonsList.Count; i++)
        {
            if (gridButtonsList[i].gameObject.activeSelf)
            {
                gridButtonsList[i].InitializeButton(GridButton.ButtonState.active);
            }               
        }

        for (int i = 0; i < dividedListCount; i++)
        {
            if (listOfButtonsToBlock.Count > 0)
            {
                GridButton gridButton = listOfActiveObjects[Random.Range(0, listOfActiveObjects.Count)];
                if (CheckForSameElement(listOfButtonsToBlock, gridButton))
                {
                    listOfButtonsToBlock.Add(gridButton);
                }
                else
                {
                    dividedListCount++;
                }
            }
            else
            {
                listOfButtonsToBlock.Add(listOfActiveObjects[Random.Range(0, listOfActiveObjects.Count)]);
            }            
        }

        for (int i = 0; i < listOfButtonsToBlock.Count; i++)
        {
            listOfButtonsToBlock[i].InitializeButton(GridButton.ButtonState.blocked);
        }
    }

    private bool CheckForSameElement(List<GridButton> listToCheck, GridButton element)
    {
        for (int i = 0; i < listToCheck.Count; i++)
        {
            if (listToCheck[i].Coordinates == element.Coordinates)
            {
                return false;
            }
        }
        return true;
    }

    public static void SetActualGridButton(GridButton btn)
    {
        if (ActualButton == null)
        {
            ActualButton = btn;
            ActualButton.SetGridButtonCategory(GridButton.ButtonCategory.start);
        }
        else
        {
            PathFinder.FindPath(ActualButton.Coordinates, btn.Coordinates);
            ActualButton = btn;
        }
    }
    public static void SetButtonsColorsInPath(List<GridButton> listOfButtonsInPath)
    {
        if (listOfButtonsInPath.Count > 0)
        {
            listOfButtonsInPath[0].SetGridButtonCategory(GridButton.ButtonCategory.start);         
            listOfButtonsInPath[listOfButtonsInPath.Count - 1].SetGridButtonCategory(GridButton.ButtonCategory.target);
        }      
    }

    
    public static GridButton GetButtonByPosition(Vector2Int pos)
    {
        for (int i = 0; i < PoolingSystem.Instance.PooledObjects.Count; i++)
        {
            if (PoolingSystem.Instance.PooledObjects[i].Coordinates == pos)
            {
                return PoolingSystem.Instance.PooledObjects[i];
            }
        }
        return null;
    }
}

