using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DrawingRunes
{
    public class TouchManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Drawing Parameters")]
        [SerializeField, Tooltip("Removes points from drawn shape, higher value make calculation easier but less precise")]
        private float f_drawingSmoothness = 5.0f;
        [SerializeField, Tooltip("Regulates how different shapes can be, interpolation and smoothness setting will alter the optimal value")] 
        private float f_Tollerance = 3.1f;
        [SerializeField, Tooltip("Minimum distance before the drawing can close itself")]
        private float f_closingDistance = 0.5f;
        [SerializeField, Tooltip("Activates interpolation of target vertices: adds more vertices but will make calculation heavier")]
        private bool b_shouldInterpolate = true;
        [SerializeField, Tooltip("Number of vertices to add between any couple of vertices. If shouldInterpolate = false it won't do anything")]
        private int f_interpolationSubdivision = 3;


        [SerializeField]
        private Vector3[] testShape = {
        new Vector3(-3, 3, 0),
        new Vector3(-3, -3, 0),
        new Vector3(3, -3, 0),
        new Vector3(3, 3, 0)
    };

        private List<Vector3> targetShape;

        private LineRenderer m_lineRenderer;
        private List<Vector3> v3_positions;

        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();

            if (b_shouldInterpolate)
            {
                InterpolateTarget(testShape);
            } else
            {
                targetShape = new List<Vector3>(testShape);
            }
        }

        private void InterpolateTarget(Vector3[] originalVector)
        {
            targetShape = new List<Vector3>();

            for(int i = 0; i< originalVector.Length - 1; i++)
            {
                Vector3 v1 = originalVector[i];
                Vector3 v2 = originalVector[i + 1];

                targetShape.Add(v1);

                for(int j = 1; j < f_interpolationSubdivision; j++)
                {
                    Vector3 newVertex = Vector3.Lerp(v1, v2, (float) j / f_interpolationSubdivision);
                    targetShape.Add(newVertex);
                }
            }

            //Interpolate last and first to close loop
            Vector3 vFinal = originalVector[originalVector.Length - 1];
            Vector3 vInitial = originalVector[0];

            targetShape.Add(vFinal);

            for (int j = 1; j < f_interpolationSubdivision; j++)
            {
                Vector3 newVertex = Vector3.Lerp(vFinal, vInitial,(float) j / f_interpolationSubdivision);
                targetShape.Add(newVertex);
            }
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

        private void OnDrawGizmos()
        {
            //Used to visualize target vertices
            if(targetShape != null)
                foreach (Vector3 vertex in targetShape)
                    Gizmos.DrawSphere(vertex, 0.2f);
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


                float distance = calculateSetsDistance(v3_positions.ToArray(), targetShape.ToArray());

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
            *The algorithm roughly performs in O(2n^2) time, so it's important to keep the interpolation and smoothness
            *setting optimal to avoid unnecessary precision
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
