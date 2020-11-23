using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Listens for touch events and performs an AR raycast from the screen touch point.
    /// AR raycasts will only hit detected trackables like feature points and planes.
    ///
    /// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
    /// and moved to the hit position.
    /// </summary>
    [RequireComponent(typeof(ARRaycastManager))]
    public class ARPlacer : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;

        [SerializeField]
        [Tooltip("Always visible where user would place prefab")]
        GameObject m_placementIndicator;

        private bool _isPlaced = false;


        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;
        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject placedPrefab
        {
            get { return m_PlacedPrefab; }
            set { m_PlacedPrefab = value; }
        }

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject spawnedObject { get; private set; }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
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

        void Update()
        {
            Vector2 screenCenter = Camera.current.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
            Vector3 camForward = Camera.current.transform.forward;
            Vector3 camBearing = new Vector3(camForward.x, 0, camForward.z).normalized;

            if (!_isPlaced)
            {
                if (m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = s_Hits[0].pose;
                    m_placementIndicator.SetActive(true);
                    m_placementIndicator.transform.SetPositionAndRotation(hitPose.position, Quaternion.LookRotation(camBearing)); ;
                }
                if (!TryGetTouchPosition(out Vector2 touchPosition))
                    return;
            }
            
            if (m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                if (!_isPlaced)
                {
                    // Raycast hits are sorted by distance, so the first one
                    // will be the closest hit.
                    var hitPose = s_Hits[0].pose;

                    if (spawnedObject == null)
                    {
                        spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, Quaternion.LookRotation(camBearing));
                    }
                    else
                    {
                        spawnedObject.transform.position = hitPose.position;
                    }

                
                    _isPlaced = true;
                }
            }
        }

    }
} 