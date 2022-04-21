using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DrawingRunes
{
    public class TouchManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Drawing Parameters")]
        [SerializeField] private float f_drawingSmoothness = 5.0f;
        [SerializeField, Tooltip("Regulates how different shapes can be, a value around 3 seems optimal")] 
        private float f_Tollerance = 3.1f;
        [SerializeField, Tooltip("Minimum distance before the drawing can close itself")]
        private float f_closingDistance = 0.5f;

        [SerializeField]
        private Vector3[] testShape = {
        new Vector3(-3, 3, 0),
        new Vector3(-3, -3, 0),
        new Vector3(3, -3, 0),
        new Vector3(3, 3, 0)
    };

        private LineRenderer m_lineRenderer;
        private List<Vector3> v3_positions;

        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            for (int i = 1; i < testShape.Length; i++)
            {
                Debug.DrawLine(testShape[i - 1], testShape[i], Color.red);
            }
            Debug.DrawLine(testShape[testShape.Length - 1], testShape[0], Color.red);
#endif
        }

        public void OnBeginDrag(PointerEventData eventData)
        {

            v3_positions = new List<Vector3>();

            Vector3 firstPos = screenToWorldPos(eventData.position);

            v3_positions.Add(firstPos);

            m_lineRenderer.loop = false;
            m_lineRenderer.positionCount = 1;
            m_lineRenderer.SetPosition(0, firstPos);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 dragPos = screenToWorldPos(eventData.position);

            v3_positions.Add(dragPos);

            m_lineRenderer.positionCount++;
            m_lineRenderer.SetPosition(v3_positions.Count - 1, dragPos);

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("Finished");
            if(Vector3.Distance(v3_positions[0], v3_positions[v3_positions.Count - 1]) > f_closingDistance)
            {
                //I lifted my finger while the shape without closing the shape
                Debug.Log("Can't close shape, you lose");
            }
            else
            {
                m_lineRenderer.loop = true;
                m_lineRenderer.Simplify(f_drawingSmoothness);


                float distance = calculateSetsDistance(v3_positions.ToArray(), testShape);

                Debug.Log("Distance: " + distance + " " + (distance <= f_Tollerance ? "Hai vinto!" : "Hai Perso!"));
            }
        }

        private Vector3 screenToWorldPos(Vector3 screenPos)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;
            return worldPos;
        }

        private float calculateSetsDistance(Vector3[] setA, Vector3[] setB)
        {
            /*
            *Implements Hausdorff Distance
            *Reference: https://en.wikipedia.org/wiki/Hausdorff_distance
            *This gives acceptable results for a prototype, but needs more testing with more complex shapes
            *to assess robustness
            */

            float excedenceAtoB = ExcedenceBetweenSets(setA, setB);
            float excedenceBtoA = ExcedenceBetweenSets(setB, setA);

            return Mathf.Max(excedenceAtoB, excedenceBtoA);
        }

        private float pointDistanceFromSet(Vector3 point, Vector3[] set)
        {
            //Distance of a point to a set is defined as the minimum distance between the point with any point from the set

            List<float> distances = new List<float>();
            foreach (Vector3 setPoint in set)
            {
                distances.Add(Vector3.Distance(point, setPoint));
            }

            distances.Sort();
            return distances[0];
        }

        private float ExcedenceBetweenSets(Vector3[] setA, Vector3[] setB)
        {
            //Excedence of a setA to a setB is defined as the maximum distance of a point of A to the set B

            List<float> distances = new List<float>();
            foreach (Vector3 setPoint in setA)
            {
                distances.Add(pointDistanceFromSet(setPoint, setB));
            }

            distances.Sort();
            return distances[distances.Count - 1];
        }
    }
}
