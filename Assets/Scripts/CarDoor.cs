using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoor : MonoBehaviour
{
    [SerializeField]
    private HingeJoint _hingeJoint;

    [SerializeField]
    private float _openDoorValue;

    [SerializeField]
    private float _closedDoorValue;

    private enum RotationAxis
    {
        X,
        Y,
        Z
    }

    [SerializeField]
    private RotationAxis _axis;

    private float _currentRotation;

    private bool _doorIsOpen = false;

    // Update is called once per frame
    void Update()
    {
        switch (_axis)
        {
            case RotationAxis.X:
                _currentRotation = transform.eulerAngles.x;
                break;
            case RotationAxis.Y:
                _currentRotation = transform.eulerAngles.y;
                break;
            case RotationAxis.Z:
                _currentRotation = transform.eulerAngles.z;
                break;
        }

        /*
        float diffClosed = Mathf.Abs(Mathf.DeltaAngle(_currentRotation , _closedDoorValue));
        float diffOpen = Mathf.Abs(Mathf.DeltaAngle(_currentRotation , _openDoorValue));
        if (_doorIsOpen)
        {
            //if current rotation is closer to closedvalue than it is to open value
            if (diffOpen > diffClosed) 
            {
                //close door
                JointSpring spring = new JointSpring();
                spring.targetPosition = _closedDoorValue;
                spring.damper = 0.5f;
                spring.spring = 25f;
                _hingeJoint.spring = spring;
                _doorIsOpen = false;
            }
        }
        else
        {
            if (diffClosed > diffOpen)
            {
                //close door
                JointSpring spring = new JointSpring();
                spring.targetPosition = _openDoorValue;
                spring.damper = 0.5f;
                spring.spring = 25f;
                _hingeJoint.spring = spring;
                _doorIsOpen = true;
            }
        }
        */

    }

    public void ToggleDoor(bool open)
    {
        if (open)
        {
            JointSpring spring = new JointSpring();
            spring.targetPosition = _openDoorValue;
            spring.damper = 0.5f;
            spring.spring = 55f;
            _hingeJoint.spring = spring;
            _doorIsOpen = true;
        }
        else
        {
            JointSpring spring = new JointSpring();
            spring.targetPosition = _closedDoorValue;
            spring.damper = 0.5f;
            spring.spring = 55f;
            _hingeJoint.spring = spring;
            _doorIsOpen = false;
        }
    }
    public void ToggleDoor()
    {
        ToggleDoor(!_doorIsOpen);
    }
}
