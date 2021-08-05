using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class Buoyancy : MonoBehaviour
{
    [SerializeField, Range(0, 20), Tooltip("How much drag the object has when under the water")]
    private float drag = 1f;
    [SerializeField, Range(0, 100), Tooltip("How buoyant the object is")]
    private float buoyancy = 10f;
    [SerializeField, Tooltip("How heigh above the water surface you want the buoyancy to rise to")]
    private float heightOffset = 0f;

    [SerializeField, Tooltip("Uses physics updates or normal updates")]
    private bool realtime;
    [SerializeField, Tooltip("Whether or not the water can push you in the direction of the wave normal")]
    private bool usesNormal;

    [SerializeField, Tooltip("This will determine which direction the object can move without drag\n(length 0 = full drag, 1 = no drag in that direction)\n(local space)")]
    public Vector3 draglessDirection = Vector3.zero;
    //[SerializeField, Range(0f, 1f), Tooltip("The range of the drag that is dragless...")]
    //private float draglessWidth = 1f;
    [SerializeField, Tooltip("Whether or not the dragless direction applies to the opposite direction as well")]
    private bool draglessOneDirection = true;

    [SerializeField, ShowIf("usesNormal"), Range(0f, 1f), Tooltip("How much of the normal affects the object (0-1)")]
    private float normalStrength = 1f;

    [SerializeField, Tooltip("The positions on which the bouyancy will be applied to on the object")]
    public BuoyantPosition[] positions = new BuoyantPosition[1];

    //[SerializeField, Tooltip("Allows the gizmos to be seen")]
    //private bool drawGizmos;
    [SerializeField, Tooltip("The size of the gizmos")] // , ShowIf("drawGizmos")
    private float gizmoSize = 0.1f;

    /// <summary>
    /// How many points are submersed underneath the water
    /// </summary>
    //[HideInInspector]
    public int submersed = 0;
    /// <summary>
    /// Percentage of points submersed in the water
    /// </summary>
    public float SubmersedPercentage
    {
        get
        {
            return (float)submersed / (float)positions.Length;
        }
    }

    WaterLevelFinder finder;
    Rigidbody rb;

    #region Properties

    //private int[] forcePositions;
    //public BuoyantPosition[] ForcePositions
    //{
    //    get
    //    {
    //        if (forcePositions == null)
    //        {
    //            List<int> indices = new List<int>();
    //            for (int i = 0; i < positions.Length; i++)
    //            {
    //                if (positions[i].isForcePoint)
    //                {
    //                    indices.Add(i);
    //                }
    //            }
    //            forcePositions = indices.ToArray();
    //        }

    //        //List<BuoyantPosition> pos = new List<BuoyantPosition>();
    //        //for (int i = 0; i < positions.Length; i++)
    //        //{
    //        //    pos.Add(ref positions[i]);
    //        //}
    //        //if (forcePositions == null)
    //        //{
    //        //    forcePositions = positions.Where(p => p.isForcePoint).Select(p => ref p).ToArray();
    //        //}
    //        return forcePositions.Select(p => positions[p]).ToArray();
    //    }
    //}

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        finder = GetComponent<WaterLevelFinder>();
        rb = GetComponent<Rigidbody>();

        SetNodePositions();
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
            Vector3 position = transform.TransformPoint(positions[i].position);
            Vector3 velocity = (position - positions[i].previous) / Time.deltaTime;
            Vector3 dragVelocity = velocity;

            if (draglessDirection.sqrMagnitude > 0f)
            {
                float amountOfDrag = Vector3.Dot(-dragVelocity.normalized, transform.TransformDirection(draglessDirection).normalized);
                //Debug.Log($"Dot product - {amountOfDrag}");
                amountOfDrag = draglessOneDirection ? Mathf.Clamp(amountOfDrag, 0f, 1f) : Mathf.Abs(amountOfDrag);
                //Debug.Log($"Drag 0f to 1f - {amountOfDrag}");
                dragVelocity = dragVelocity * (1f - (amountOfDrag * Mathf.Clamp(draglessDirection.magnitude, 0f, 1f)));
            }

            // Get the water position
            Vector3 waterPosition = finder != null ? finder.GetWaterSurfacePosition(position) + Vector3.up * heightOffset : new Vector3(position.x, heightOffset, position.z);

            // Normal vector of the point
            Vector3 normal = Vector3.up;
            if (usesNormal)
            {
                normal = ((finder.GetWaterNormal(position) * normalStrength) + (Vector3.up * (1f - normalStrength))).normalized;
            }

            // Difference of where we are to where we want to be
            float difference = position.y - waterPosition.y;
            if (difference < 0)
            {
                positions[i].isSubmersed = true;
                submersed++;

                difference = Mathf.Clamp(-difference, 0f, 3f); // clamp the difference, I can't remember why this is...

                // Drag force
                rb.AddForceAtPosition(((drag * rb.mass) * -dragVelocity / positions.Length) * Time.deltaTime * 60f, position);
                //rb.AddForceAtPosition(((drag * rb.mass) * -velocity / positions.Length) * Time.deltaTime * 60f, position);

                // Bouyancy force
                rb.AddForceAtPosition(((buoyancy * rb.mass) * normal * difference / positions.Length) * Time.deltaTime * 60f, position);
            }
            else
            {
                positions[i].isSubmersed = false;
            }


            positions[i].previous = position;
        }
    }

    public void SetNodePositions()
    {
        foreach (var position in positions)
        {
            position.previous = transform.TransformPoint(position.position);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (finder == null)
        {
            finder = GetComponent<WaterLevelFinder>();
        }

        //if (drawGizmos)
        //{
        foreach (var position in positions)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.TransformPoint(position.position), gizmoSize);

            Gizmos.color = Color.green;
            if (finder)
            {
                Gizmos.DrawSphere(finder.GetWaterSurfacePosition(transform.TransformPoint(position.position)) + Vector3.up * heightOffset, gizmoSize);
            }
            else
            {
                Vector3 pos = transform.TransformPoint(position.position);
                pos.y = heightOffset;
                Gizmos.DrawSphere(pos, gizmoSize);
            }
        }

            //Gizmos.DrawSphere(finder.GetWaterSurfacePosition(transform.position), 0.1f);
        //}
    }
#endif
}

[Serializable]
public class BuoyantPosition
{
    public Vector3 position = Vector3.zero;
    public Vector3 previous = Vector3.zero;
    public bool isSubmersed = false;
    //public bool isForcePoint = false;
#if UNITY_EDITOR
    public bool isEditing = false;
#endif
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(BuoyantPosition))]
public class BuoyantPositionDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 2f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        label.text = label.text.Replace("Element", "Position");
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        if (property.FindPropertyRelative("isEditing").boolValue)
        {
            GUI.backgroundColor = Color.cyan;
        }

        // Draw the position
        position.height /= 2f;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("position"), GUIContent.none);

        //// Toggles force point
        //position.y += position.height;
        //Rect newPosition = EditorGUI.PrefixLabel(position, new GUIContent { text = "Is Force Point" });
        //property.FindPropertyRelative("isForcePoint").boolValue = EditorGUI.Toggle(newPosition, property.FindPropertyRelative("isForcePoint").boolValue);

        // Toggles editing
        position.y += position.height;
        Rect newPosition = EditorGUI.PrefixLabel(position, new GUIContent { text = "Edit" });
        property.FindPropertyRelative("isEditing").boolValue = EditorGUI.Toggle(newPosition, property.FindPropertyRelative("isEditing").boolValue);

        EditorGUI.EndProperty();
    }
}

#endif