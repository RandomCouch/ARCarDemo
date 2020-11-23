using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoUIManager : MonoBehaviour
{
    [SerializeField]
    private Text _statusText;

    [SerializeField]
    private Button _replaceButton;

    [SerializeField]
    private Slider _scaleSlider;

    [SerializeField]
    private Slider _rotateSlider;

    [SerializeField]
    private Button _continueButton;

    [SerializeField]
    private DrivingControls _drivingControls;

    public DrivingControls drivingControls
    {
        get
        {
            return _drivingControls;
        }
    }

    public Action onReplaceClicked;
    public Action onContinueClicked;
    public Action<float> onScaleUpdated;
    public Action<float> onRotateUpdated;
    // Start is called before the first frame update
    void Start()
    {
        _replaceButton?.onClick.AddListener(OnReplaceButtonClicked);
        _continueButton?.onClick.AddListener(OnContinueButtonClicked);
        _scaleSlider?.onValueChanged.AddListener(OnScaleSliderUpdated);
        _rotateSlider?.onValueChanged.AddListener(OnRotateSliderUpdated);
    }
    private void OnScaleSliderUpdated(float value)
    {
        onScaleUpdated?.Invoke(value);
    }
    private void OnRotateSliderUpdated(float value)
    {
        onRotateUpdated?.Invoke(value);
    }
    public void ToggleAdjustmentControls(bool enabled)
    {
        _scaleSlider?.gameObject.SetActive(enabled);
        _rotateSlider?.gameObject.SetActive(enabled);
        _continueButton?.gameObject.SetActive(enabled);
    }
    public void ToggleDrivingControls(bool enabled)
    {
        _drivingControls?.gameObject.SetActive(enabled);
    }
    private void OnReplaceButtonClicked()
    {
        onReplaceClicked?.Invoke();
    }
    private void OnContinueButtonClicked()
    {
        onContinueClicked?.Invoke();
    }
    public void ToggleReplaceButton(bool enabled)
    {
        _replaceButton?.gameObject.SetActive(enabled);
    }
    public void UpdateStatusText(string text)
    {
        if(_statusText != null)
        {
            _statusText.text = text;
        }
    }
    public void ToggleStatusText(bool enabled)
    {
        if(_statusText != null)
        {
            _statusText.transform.parent.gameObject.SetActive(enabled);
        }
    }
}
