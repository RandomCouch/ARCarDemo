using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private WheelCollider _flWheel, _frWheel, _blWheel, _brWheel;

    [SerializeField]
    private Transform _flWheelTransform, _frWheelTransform, _blWheelTransform, _brWheelTransform;

    [SerializeField]
    private float _maxSteeringAngle, _motorForce, _brakeForce;

    public DrivingControls _controls;

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public void GetInput()
    {
        //m_horizontalInput = Input.GetAxis("Horizontal");
        //m_verticalInput = Input.GetAxis("Vertical");
        m_horizontalInput = _controls.steeringInput;
        m_verticalInput = _controls.accelerationInput;
    }

    private void Steer()
    {
        m_steeringAngle = _maxSteeringAngle * m_horizontalInput;
        _flWheel.steerAngle = m_steeringAngle;
        _frWheel.steerAngle = m_steeringAngle;
    }

    private void Accelerate()
    {
        int multiplier = 1;
        if(_controls.gearInput == Gearbox.Gear.N)
        {
            multiplier = 0;
        }else if(_controls.gearInput == Gearbox.Gear.R)
        {
            multiplier = -1;
        }

        _flWheel.motorTorque = m_verticalInput * _motorForce * multiplier;
        _frWheel.motorTorque = m_verticalInput * _motorForce * multiplier;
        _flWheel.brakeTorque = 0;
        _frWheel.brakeTorque = 0;
        _blWheel.brakeTorque = 0;
        _brWheel.brakeTorque = 0;
        //_blWheel.motorTorque = m_verticalInput * _motorForce;
        //_brWheel.motorTorque = m_verticalInput * _motorForce;
    }

    private void Brake()
    {
        _flWheel.motorTorque = 0;
        _frWheel.motorTorque = 0;
        _flWheel.brakeTorque = _brakeForce;
        _frWheel.brakeTorque = _brakeForce;
        _blWheel.brakeTorque = _brakeForce;
        _brWheel.brakeTorque = _brakeForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(_flWheel, _flWheelTransform);
        UpdateWheelPose(_frWheel, _frWheelTransform);
        UpdateWheelPose(_blWheel, _blWheelTransform);
        UpdateWheelPose(_brWheel, _brWheelTransform);
    }

    private void UpdateWheelPose(WheelCollider collider, Transform trans)
    {
        Vector3 pos = trans.position;
        Quaternion quat = trans.rotation;

        collider.GetWorldPose(out pos, out quat);

        trans.position = pos;
        trans.rotation = quat;

        trans.Rotate(collider.rpm / 60 * 360 * Time.deltaTime, 0, 0);
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        
        if (_controls.brakeInput > 0f)
        {
            Brake();
        }
        else
        {
            Accelerate();
        }
        
        UpdateWheelPoses();
    }
}
