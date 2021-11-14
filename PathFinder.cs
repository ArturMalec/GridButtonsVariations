using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinder
{
    private static List<GridButton> buttonsInPath;
    private static bool isOnLeft = false, isOnRight = false, isOnUp = false, isOnDown = false;
    private static bool isOnRightDown = false, isOnLeftUp = false, isOnLeftDown = false, isOnRightUp = false;

    public static void FindPath(Vector2Int startCoords, Vector2Int targetCoords)
    {
        if (buttonsInPath != null)
        {
            ResetButtonsToDefault();
        }
        buttonsInPath = new List<GridButton>();
        Vector2Int moveCoords = new Vector2Int(targetCoords.x - startCoords.x, targetCoords.y - startCoords.y);

        int x = moveCoords.x > 0 ? moveCoords.x : checked(-moveCoords.x);
        int y = moveCoords.y > 0 ? moveCoords.y : checked(-moveCoords.y);
        int count = x + y;

        int columnsIterator = startCoords.y;
        int rowsIterator = startCoords.x;       

        for (int i = 0; i <= count; i++)
        {
            if ((targetCoords.y > startCoords.y) && (targetCoords.x == startCoords.x)) // right
            {
                isOnRight = true;
                if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                {
                    ResetButtonsToDefault();
                    return;
                }
                columnsIterator++;               
            }
            if ((targetCoords.x > startCoords.x) && (targetCoords.y == startCoords.y)) // down
            {
                isOnDown = true;
                if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                {
                    ResetButtonsToDefault();
                    return;
                }
                rowsIterator++;               
            }
            if ((targetCoords.y < startCoords.y) && (targetCoords.x == startCoords.x)) // left
            {
                isOnLeft = true;
                if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                {
                    ResetButtonsToDefault();
                    return;
                }
                columnsIterator--;               
            }
            if ((targetCoords.x < startCoords.x) && (targetCoords.y == startCoords.y)) // up
            {
                isOnUp = true;
                if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                {
                    ResetButtonsToDefault();
                    return;
                }
                rowsIterator--;
            }
            if ((targetCoords.y > startCoords.y) && (targetCoords.x > startCoords.x)) //right and down
            {
                isOnRightDown = true;
                if (columnsIterator < targetCoords.y && rowsIterator < targetCoords.x)
                {
                    if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                    {
                        ResetButtonsToDefault();
                        return;
                    }
                    columnsIterator++;
                    rowsIterator++;
                    count--;
                }
                else
                {
                    if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                    {
                        ResetButtonsToDefault();
                        return;
                    }
                    if (rowsIterator == targetCoords.x)
                    {
                        columnsIterator++;
                    }
                    else
                    {
                        rowsIterator++;
                    }
                   
                }
            }
            if ((targetCoords.x > startCoords.x) && (targetCoords.y < startCoords.y)) // left and down
            {
                isOnLeftDown = true;
                if (columnsIterator > targetCoords.y && rowsIterator < targetCoords.x)
                {
                    if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                    {
                        ResetButtonsToDefault();
                        return;
                    }
                    columnsIterator--;
                    rowsIterator++;
                    count--;
                }
                else
                {
                    if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                    {
                        ResetButtonsToDefault();
                        return;
                    }
                    if (rowsIterator == targetCoords.x)
                    {
                        columnsIterator--;
                    }
                    else
                    {
                        rowsIterator++;
                    }                  
                }
            }
            if ((targetCoords.y < startCoords.y) && (targetCoords.x < startCoords.x)) // left and up
            {
                isOnLeftUp = true;
                if (columnsIterator > targetCoords.y && rowsIterator > targetCoords.x)
                {
                    if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                    {
                        ResetButtonsToDefault();
                        return;
                    }
                    columnsIterator--;
                    rowsIterator--;
                    count--;
                }
                else
                {
                    if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                    {
                        ResetButtonsToDefault();
                        return;
                    }
                    if (rowsIterator == targetCoords.x)
                    {
                        columnsIterator--;
                    }
                    else
                    {
                        rowsIterator--;
                    }                  
                }
            }
            if ((targetCoords.x < startCoords.x) && (targetCoords.y > startCoords.y)) // right and up
            {
                isOnRightUp = true;
                if (columnsIterator < targetCoords.y && rowsIterator > targetCoords.x)
                {
                    if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                    {
                        ResetButtonsToDefault();
                        return;
                    }
                    columnsIterator++;
                    rowsIterator--;
                    count--;
                }
                else
                {
                    if (!ButtonsValidation(rowsIterator, columnsIterator, startCoords))
                    {
                        ResetButtonsToDefault();
                        return;
                    }
                    if (rowsIterator == targetCoords.x)
                    {
                        columnsIterator++;
                    }
                    else
                    {
                        rowsIterator--;
                    }                  
                }               
            }
        }
        ButtonsController.SetButtonsColorsInPath(buttonsInPath);
    }

    private static bool ButtonsValidation(int rowsIt, int columnsIt, Vector2Int startCoords)
    {
        if (isOnLeft || isOnRight)
        {
            Vector2Int newCoords = new Vector2Int(rowsIt, columnsIt);
            GridButton gridButton = ButtonsController.GetButtonByPosition(newCoords);
            if (gridButton.GridButtonState == GridButton.ButtonState.blocked)
            {
                newCoords = new Vector2Int(rowsIt + 1, columnsIt);
                gridButton = ButtonsController.GetButtonByPosition(newCoords);
                if (gridButton == null || gridButton.GridButtonState == GridButton.ButtonState.blocked)
                {
                    newCoords = new Vector2Int(rowsIt - 1, columnsIt);
                    gridButton = ButtonsController.GetButtonByPosition(newCoords);
                    if (gridButton == null || gridButton.GridButtonState == GridButton.ButtonState.blocked)
                    {
                        Debug.LogError("No way");
                        return false;
                    }
                }
            }
            Vector2Int pathButtonCoords = gridButton.CalculateDistanceToStartButton(startCoords);
            gridButton.SetGridButtonCategory(GridButton.ButtonCategory.path, pathButtonCoords);
            buttonsInPath.Add(gridButton);
        }
        if (isOnUp || isOnDown || isOnRightDown || isOnLeftUp || isOnLeftDown || isOnRightUp)
        {
            Vector2Int newCoords = new Vector2Int(rowsIt, columnsIt);
            GridButton gridButton = ButtonsController.GetButtonByPosition(newCoords);
            if (gridButton.GridButtonState == GridButton.ButtonState.blocked)
            {
                newCoords = new Vector2Int(rowsIt, columnsIt + 1);
                gridButton = ButtonsController.GetButtonByPosition(newCoords);
                if (gridButton == null || gridButton.GridButtonState == GridButton.ButtonState.blocked)
                {
                    newCoords = new Vector2Int(rowsIt, columnsIt - 1);
                    gridButton = ButtonsController.GetButtonByPosition(newCoords);
                    if (gridButton == null || gridButton.GridButtonState == GridButton.ButtonState.blocked)
                    {
                        newCoords = new Vector2Int(rowsIt + 1, columnsIt);
                        gridButton = ButtonsController.GetButtonByPosition(newCoords);
                        if (gridButton == null || gridButton.GridButtonState == GridButton.ButtonState.blocked)
                        {
                            newCoords = new Vector2Int(rowsIt - 1, columnsIt);
                            gridButton = ButtonsController.GetButtonByPosition(newCoords);
                            if (gridButton == null || gridButton.GridButtonState == GridButton.ButtonState.blocked)
                            {
                                Debug.LogError("No way");
                                return false;
                            }
                        }
                    }
                }
            }
            Vector2Int pathButtonCoords = gridButton.CalculateDistanceToStartButton(startCoords);
            gridButton.SetGridButtonCategory(GridButton.ButtonCategory.path, pathButtonCoords);
            buttonsInPath.Add(gridButton);
        }
        return true;
    }

    private static void ResetButtonsToDefault()
    {
        for (int i = 0; i < buttonsInPath.Count; i++)
        {
            buttonsInPath[i].SetGridButtonCategory(GridButton.ButtonCategory.none);
        }
        ButtonsController.ActualButton = null;
    }
}
