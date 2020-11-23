using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrivingControls : MonoBehaviour
{
    [SerializeField]
    private DrivingButton _leftButton, _rightButton, _gasPedal, _brakePedal;

    [SerializeField]
    private Gearbox _gearbox;

    private float _sInput;

    public float steeringInput
    {
        get
        {
            
            return _sInput;
        }
    }

    public float accelerationInput
    {
        get
        {
            float aInput = 0f;

            if (_gasPedal.buttonHeld)
            {
                aInput += 1f;
            }

            return aInput;
        }
    }

    public float brakeInput
    {
        get
        {
            float bInput = 0f;
            if (_brakePedal.buttonHeld)
            {
                bInput += 1f;
            }
            
            return bInput;
        }
    }

    public Gearbox.Gear gearInput
    {
        get {
            return _gearbox.currentGear;
        }
    }

    private void Update()
    {
        if (_leftButton.buttonHeld)
        {
            _sInput -= 1f * Time.deltaTime * 10f;
        }
        if (_rightButton.buttonHeld)
        {
            _sInput += 1f * Time.deltaTime * 10f;
        }

        if(!_leftButton.buttonHeld && !_rightButton.buttonHeld)
        {
            _sInput = Mathf.Lerp(_sInput, 0f, Time.deltaTime * 10f);
        }

        _sInput = Mathf.Clamp(_sInput, -1f, 1f);
        
    }
}
