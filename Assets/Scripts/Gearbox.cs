using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gearbox : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private Text _gearText;

    public enum Gear
    {
        D,
        N,
        R
    }

    private Gear _currentGear;

    public Gear currentGear
    {
        get
        {
            return _currentGear;
        }
    }

    private void Start()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        //0 = R 
        //1 = N 
        //2 = D
        if((int) value == 0)
        {
            _gearText.text = "R";
            _currentGear = Gear.R;
        }
        else if((int) value == 1)
        {
            _gearText.text = "N";
            _currentGear = Gear.N;
        }
        else if((int) value == 2)
        {
            _gearText.text = "D";
            _currentGear = Gear.D;
        }
    }
}
