using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(WaterLevelFinder))]
[RequireComponent(typeof(Rigidbody))]
public class Buoyancy : MonoBehaviour
{
    [Range(0, 15)]
    public float drag = 1f;
    [Range(0, 35)]
    public float buoyancy = 10f;

    public bool realtime;

    public Vector3[] positions = new Vector3[1];
    Vector3[] prevPositions;

    public bool drawGizmos;
    public float gizmoSize = 0.1f;

    public int submersed = 0;
    public float SubmersedPercentage
    {
        get
        {
            return submersed / positions.Length;
        }
    }

    WaterLevelFinder finder;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        finder = GetComponent<WaterLevelFinder>();
        rb = GetComponent<Rigidbody>();

        prevPositions = new Vector3[positions.Length];
    }

    void OnEnable()
    {
        SetNodePositions();
    }

    void Update()
    {
        if (realtime) Step();
    }

    void FixedUpdate()
    {
        if (!realtime) Step();
    }

    void Step()
    {
        submersed = 0;
        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 position = transform.TransformPoint(positions[i]);
            Vector3 velocity = (position - prevPositions[i]) / Time.deltaTime;
            Vector3 waterPosition = finder.GetWaterSurfacePosition(position);

            float difference = position.y - waterPosition.y;
            if (difference < 0)
            {
                submersed++;
                difference = Mathf.Clamp(-difference, 0f, 3f);

                rb.AddForceAtPosition(((drag * rb.mass) * -velocity / positions.Length) * Time.deltaTime * 60f, position);
                rb.AddForceAtPosition(((buoyancy * rb.mass) * Vector3.up * difference / positions.Length) * Time.deltaTime * 60f, position);
            }


            prevPositions[i] = position;
        }
    }

    public void SetNodePositions()
    {
        if (prevPositions == null) prevPositions = new Vector3[positions.Length];
        for (int i = 0; i < positions.Length; i++) prevPositions[i] = transform.TransformPoint(positions[i]);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (finder == null)
        {
            finder = GetComponent<WaterLevelFinder>();
        }

        if (drawGizmos)
        {
            foreach (var position in positions)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(transform.TransformPoint(position), gizmoSize);

                Gizmos.color = Color.green;
                Gizmos.DrawSphere(finder.GetWaterSurfacePosition(transform.TransformPoint(position)), gizmoSize);
            }

            //Gizmos.DrawSphere(finder.GetWaterSurfacePosition(transform.position), 0.1f);
        }
    }
#endif
}
