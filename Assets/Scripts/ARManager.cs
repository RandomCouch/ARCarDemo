using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARManager : MonoBehaviour
{
    [SerializeField] 
    ARSession m_Session;

    [SerializeField]
    ARSessionOrigin m_arOrigin;

    [SerializeField]
    private GameObject m_placementIndicator;

    [SerializeField]
    private GameObject m_PlacedPrefab;

    [SerializeField]
    private Camera m_arCamera;

    [SerializeField]
    private DemoUIManager _uiManager;

    [SerializeField]
    private Material _planeUnplacedMaterial, _planePlacedMaterial;

    [SerializeField]
    private GameObject _testObj;

    [SerializeField]
    private GameObject _hand ;

    private Pose placementPose;
    private bool poseIsValid = false;

    private bool _isPlaced = false;
    private bool _isAdjusted = false;

    private ARRaycastManager m_RaycastManager;

    private ARPlaneManager m_planeManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public GameObject spawnedObject { get; private set; }


    IEnumerator Start()
    {
        if ((ARSession.state == ARSessionState.None) ||
            (ARSession.state == ARSessionState.CheckingAvailability))
        {
            yield return ARSession.CheckAvailability();
        }

        if (ARSession.state == ARSessionState.Unsupported)
        {
            // Start some fallback experience for unsupported devices
            _uiManager.UpdateStatusText("This device does not support AR");
        }
        else
        {
            // Start the AR session
            m_Session.enabled = true;
        }

        _uiManager.onReplaceClicked += OnReplace;
        _uiManager.onContinueClicked += OnContinue;
        _uiManager.onScaleUpdated += OnScaleUpdated;
        _uiManager.onRotateUpdated += OnRotateUpdated;
    }

    private void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_planeManager = GetComponent<ARPlaneManager>();

    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;

            return true;
        }

        touchPosition = default;
        return false;
    }

    private void OnReplace()
    {
        if (_isPlaced)
        {
            _isPlaced = false;
            if (spawnedObject != null)
            {
                Destroy(spawnedObject);
            }
            _uiManager.ToggleAdjustmentControls(false);
        }
        if (_isAdjusted)
        {
            _isAdjusted = false;
        }
    }

    private void OnContinue()
    {
        if(_isPlaced && !_isAdjusted)
        {
            _isAdjusted = true;
        }
    }

    private void SetPlaneMaterial(Material mat)
    {
        foreach (ARPlane plane in m_planeManager.trackables)
        {
            MeshRenderer planeRenderer = plane.GetComponent<MeshRenderer>();
            if (planeRenderer != null)
            {
                planeRenderer.material = mat;
            }
        }
    }

    private void OnScaleUpdated(float newScale)
    {
        if (_isPlaced)
        {
            spawnedObject.transform.localScale = Vector3.one * newScale;

            //update joint anchors
            Joint[] joints = spawnedObject.GetComponentsInChildren<Joint>();
            foreach (Joint j in joints)
            {
                Rigidbody jrb = j.gameObject.GetComponent<Rigidbody>();
                j.connectedAnchor = j.connectedBody.transform.InverseTransformPoint(jrb.transform.position);
            }
        }
    }

    private void OnRotateUpdated(float newAngle)
    {
        if (_isPlaced)
        {
            Vector3 angles = spawnedObject.transform.eulerAngles;
            angles.y = newAngle;
            spawnedObject.transform.eulerAngles = angles;
        }
    }

    private void Update()
    {
        Vector2 screenCenter = m_arCamera.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        Vector3 camForward = m_arCamera.transform.forward;
        Vector3 camBearing = new Vector3(camForward.x, 0, camForward.z).normalized;

        if (!_isPlaced)
        {
            _uiManager.ToggleStatusText(true);
            _uiManager.ToggleReplaceButton(false);
            _uiManager.ToggleAdjustmentControls(false);
            _hand.SetActive(false);
            if(m_planeManager.trackables.count == 0)
            {
                _uiManager.UpdateStatusText("Status: Scanning for surfaces");
            }
            else if(m_planeManager.trackables.count > 0)
            {
                _uiManager.UpdateStatusText("Status: Surface found! Tap to place vehicle");
            }

            if (m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = s_Hits[0].pose;
                m_placementIndicator.SetActive(true);
                m_placementIndicator.transform.SetPositionAndRotation(hitPose.position, Quaternion.LookRotation(camBearing));
            }

            SetPlaneMaterial(_planeUnplacedMaterial);
        }
        else
        {
            _uiManager.ToggleStatusText(false);
            _uiManager.ToggleReplaceButton(true);
            _uiManager.ToggleAdjustmentControls(true);
            _hand.SetActive(true);
        }

        if (!TryGetTouchPosition(out Vector2 touchPosition) && !_isPlaced)
            return;

        if (m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            if (!_isPlaced)
            {
                var hitPose = s_Hits[0].pose;

                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, Quaternion.LookRotation(camBearing));
                }
                else
                {
                    spawnedObject.transform.position = hitPose.position;
                }

                m_placementIndicator.SetActive(false);

                SetPlaneMaterial(_planePlacedMaterial);

                CarController carController = spawnedObject.GetComponentInChildren<CarController>();
                if(carController != null)
                {
                    carController._controls = _uiManager.drivingControls;
                }

                _isPlaced = true;
            }
        }

        if (!_isAdjusted && _isPlaced)
        {
            _uiManager.ToggleAdjustmentControls(true);
            _uiManager.ToggleDrivingControls(false);
            _hand.SetActive(false);
        }
        else if(_isAdjusted)
        {
            _uiManager.ToggleAdjustmentControls(false);
            _uiManager.ToggleDrivingControls(true);
            _hand.SetActive(true);
        }

    }
}
