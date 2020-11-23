using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveHandle : MonoBehaviour
{
    [SerializeField]
    private Color _inactiveColor, _activeColor;

    private MeshRenderer _renderer;

    private Rigidbody _rb;

    private bool _isInteractable = false;

    private Transform _hand;

    private Vector3 _posOffset = Vector3.zero;

    [SerializeField]
    private HingeJoint _carHingeJoint;

    [SerializeField]
    private CarDoor _door;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();
    }

    public void ToggleActive(bool active)
    {
        if (active)
        {
            _renderer.material.color = _activeColor;
            _renderer.materials[0].color = _activeColor;
            _renderer.material.SetColor("_BaseColor", _activeColor);
            _isInteractable = true;
           
        }
        else
        {
            _renderer.material.color = _inactiveColor;
            _renderer.materials[0].color = _inactiveColor;
            _renderer.material.SetColor("_BaseColor", _inactiveColor);
            _isInteractable = false;
            _carHingeJoint.useSpring = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInteractable && _hand != null)
        {
            if(Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    _door.ToggleDoor();
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                _door.ToggleDoor();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "hand")
        {
            _hand = other.transform;
            ToggleActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "hand" && _hand == null)
        {
            _hand = other.transform;
            ToggleActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "hand")
        {
            _hand = null;
            ToggleActive(false);
        }
    }
}
