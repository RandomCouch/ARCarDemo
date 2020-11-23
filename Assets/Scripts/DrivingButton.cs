using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrivingButton : MonoBehaviour
{

    private bool _buttonHeld = false;

    public bool buttonHeld
    {
        get
        {
            return _buttonHeld;
        }
    }

    public void OnButtonHeld(bool held)
    {
        _buttonHeld = held;
    }
}
