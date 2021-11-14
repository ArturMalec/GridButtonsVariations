using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingSystem : MonoBehaviour, IPoolingSystem
{
    [SerializeField] GridButton _GridButton;

    private List<GridButton> pooledObjects = new List<GridButton>();

    public static int PoolSize;
    public static PoolingSystem Instance;

    public List<GridButton> PooledObjects { get { return pooledObjects; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void InitializePooling()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GridButton gridButton = Instantiate(_GridButton, transform);
            gridButton.gameObject.SetActive(false);
            PooledObjects.Add(gridButton);
        }
    }

    public GridButton GetPooledButton()
    {
        for (int i = 0; i < PooledObjects.Count; i++)
        {
            if (!PooledObjects[i].gameObject.activeInHierarchy)
            {
                return PooledObjects[i];
            }
        }

        GridButton gridButton = Instantiate(_GridButton, transform);
        PooledObjects.Add(gridButton);
        return gridButton;
    }

    public void DeactivatePooledButtons()
    {
        for (int i = 0; i < PooledObjects.Count; i++)
        {
            PooledObjects[i].gameObject.SetActive(false);
        }
    }
}
