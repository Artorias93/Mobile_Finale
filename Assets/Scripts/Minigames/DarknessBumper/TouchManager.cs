using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DarknessBumper
{
    public enum DrawingStatus
    {
        DRAWING, INVALID, FINISHED
    }
    public class TouchManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Drawing Parameters")]
        [SerializeField, Tooltip("Removes points from drawn shape, higher value make calculation easier but less precise")]
        private float f_drawingSmoothness = 5.0f;
        [SerializeField, Tooltip("Minimum distance before the drawing can close itself")]
        private float f_closingDistance = 0.5f;
        [SerializeField, Tooltip("Activates interpolation of target vertices: adds more vertices but will make calculation heavier")]
        private bool b_shouldInterpolate = true;
        [SerializeField, Tooltip("Number of vertices to add between any couple of vertices. If shouldInterpolate = false it won't do anything")]
        private int f_interpolationSubdivision = 3;


        private List<Vector3> targetShape;

        private LineRenderer m_lineRenderer;
        private List<Vector3> v3_positions;

        private DrawingStatus m_status = DrawingStatus.INVALID;
        private float f_distance;

        public DrawingStatus Status
        {
            get { return m_status; }
        }

        public float Distance
        {
            get
            {
                if (m_status == DrawingStatus.FINISHED)
                {
                    m_status = DrawingStatus.INVALID;
                    return f_distance;
                }
                return -1;
            }
        }

        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
        }

        public void requestShape(List<Vector3> shape)
        {
            if (shape == null || shape.Count == 0)
            {
                Debug.LogWarning("Passed invalid shape");
                return;
            }

            if (b_shouldInterpolate)
            {
                InterpolateTarget(shape);
            }
            else
            {
                targetShape = new List<Vector3>(shape);
            }
        }

        private void InterpolateTarget(List<Vector3> originalVector)
        {
            targetShape = new List<Vector3>();

            for(int i = 0; i< originalVector.Count - 1; i++)
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
            Vector3 vFinal = originalVector[originalVector.Count - 1];
            Vector3 vInitial = originalVector[0];

            targetShape.Add(vFinal);

            for (int j = 1; j < f_interpolationSubdivision; j++)
            {
                Vector3 newVertex = Vector3.Lerp(vFinal, vInitial,(float) j / f_interpolationSubdivision);
                targetShape.Add(newVertex);
            }
        }

        private void OnDrawGizmos()
        {
            if(targetShape != null && targetShape.Count > 0)
            {
                for(int i = 0; i < targetShape.Count - 1; i++)
                {
                    Gizmos.DrawSphere(targetShape[i], 0.1f);
                    Gizmos.DrawLine(targetShape[i], targetShape[i + 1]);
                }
                int lastIndex = targetShape.Count - 1;

                Gizmos.DrawSphere(targetShape[lastIndex], 0.1f);
                Gizmos.DrawLine(targetShape[lastIndex], targetShape[0]);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_status = DrawingStatus.DRAWING;

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
                m_status = DrawingStatus.INVALID;
            }
            else
            {
                m_lineRenderer.loop = true;
                m_lineRenderer.Simplify(f_drawingSmoothness);


                float distance = calculateSetsDistance(v3_positions.ToArray(), targetShape.ToArray());

                f_distance = distance;
                m_status = DrawingStatus.FINISHED;

                m_lineRenderer.positionCount = 0;
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
