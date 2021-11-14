using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolingSystem
{
    void InitializePooling();
    GridButton GetPooledButton();
    void DeactivatePooledButtons();
}
