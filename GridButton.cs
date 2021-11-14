using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _Text;
    [SerializeField] Color _ButtonBlockedColor;
    [SerializeField] Color _ButtonActiveColor;
    [SerializeField] Color _StartButtonColor;
    [SerializeField] Color _TargetButtonColor;
    [SerializeField] Color _ButtonInPathColor;

    private Vector2Int coordinates;
    public enum ButtonState { blocked = 0, active = 1 }
    public enum ButtonCategory { none = 0 ,start = 1, target = 2, path = 3 }
    public Vector2Int Coordinates { get { return coordinates; } set { coordinates = value; } }
    public ButtonState GridButtonState { get; set; }

    public void InitializeButton(ButtonState state)
    {
        _Text.enabled = false;
        GridButtonState = state;

        switch (GridButtonState)
        {
            case ButtonState.active:
                GetComponent<Image>().color = _ButtonActiveColor;
                GetComponent<Button>().interactable = true;
                break;
            case ButtonState.blocked:
                GetComponent<Image>().color = _ButtonBlockedColor;
                GetComponent<Button>().interactable = false;
                break;
            default:
                break;
        }
    }

    public void ShowCoords()
    {
        ButtonsController.SetActualGridButton(this);
        _Text.enabled = true;
    }

    public void SetGridButtonCategory(ButtonCategory category, Vector2Int distanceVector = new Vector2Int())
    {
        switch (category)
        {
            case ButtonCategory.none:
                if (GridButtonState == ButtonState.active)
                    GetComponent<Image>().color = _ButtonActiveColor;
                else
                    GetComponent<Image>().color = _ButtonBlockedColor;
                _Text.enabled = false;
                break;
            case ButtonCategory.start:
                GetComponent<Image>().color = _StartButtonColor;
                _Text.text = Coordinates.ToString();
                _Text.enabled = true;
                break;
            case ButtonCategory.target:
                GetComponent<Image>().color = _TargetButtonColor;
                break;
            case ButtonCategory.path:
                GetComponent<Image>().color = _ButtonInPathColor;
                if (distanceVector != Vector2Int.zero)
                    _Text.text = Coordinates.ToString() + " + D: \n" + distanceVector; // "D" means distance
                else
                    _Text.text = Coordinates.ToString();             
                _Text.enabled = true;
                break;
            default:
                break;
        }
    }

    public Vector2Int CalculateDistanceToStartButton(Vector2Int startCoords)
    {
        return new Vector2Int(Coordinates.x - startCoords.x, Coordinates.y - startCoords.y);
    }
}
