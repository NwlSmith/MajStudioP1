using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
/*
 * Date created: 5/7/2020
 * Creator: Nate Smith
 * 
 * Description: Basic Inverse Kinematics Controller.
 * Based on FastIK by DitzelGames.
 * https://youtu.be/qqOAzn05fvk
 */
public class IKBasic : MonoBehaviour
{
    [Header("Chain Information")]
    // Chain Length
    public int chainLength = 2;

    // Target the chain should bend to
    public Transform target;
    public Transform pole;

    [Header("Curvature Solving Parameters")]
    public int iterations = 10;
    // Distance from the target the solver will stop.
    public float precision = 0.001f;

    [Range(0, 1)]
    public float snapBackStrength = 1f;

    protected float[] bonesLength;
    protected float totalLength;
    protected Transform[] bones;
    protected Vector3[] positions;
    protected Vector3[] startDirChild;
    protected Quaternion[] startRotBone;
    protected Quaternion startRotTarget;
    protected Quaternion startRotRoot;

    private void Awake()
    {
        InitBones();
    }

    private void InitBones()
    {
        // Initial array.
        bones = new Transform[chainLength + 1];
        positions = new Vector3[chainLength + 1];
        bonesLength = new float[chainLength];
        startDirChild = new Vector3[chainLength + 1];
        startRotBone = new Quaternion[chainLength + 1];

        if (target == null)
        {
            target = new GameObject(gameObject.name + " Target").transform;
            target.position = transform.position;
        }
        startRotTarget = target.rotation;

        totalLength = 0f;

        // Initialize bones data.
        Transform current = transform;
        for (int i = bones.Length - 1; i >= 0f; i--)
        {
            bones[i] = current;
            startRotBone[i] = current.rotation;

            if (i == bones.Length - 1)
            {
                startDirChild[i] = target.position - current.position;
            }
            else
            {
                startDirChild[i] = bones[i + 1].position - current.position;
                bonesLength[i] = startDirChild[i].magnitude;
                totalLength += bonesLength[i];
            }

            current = current.parent;
        }
    }

    private void LateUpdate()
    {
        ResolveIK();
    }

    private void ResolveIK()
    {
        if (target == null)
            return;

        if (bonesLength.Length != chainLength)
        {
            InitBones();
        }

        // Retrieve positions from bones.
        for (int i = 0; i < bones.Length; i++)
        {
            positions[i] = bones[i].position;
        }

        Quaternion rootRot = (bones[0].parent != null) ? bones[0].parent.rotation : Quaternion.identity;
        Quaternion rootRotDif = rootRot * Quaternion.Inverse(startRotRoot);

        // Can the target be directly reached?
        // If no, just rotate toward the target, ie, stretching.
        // Note: sqrMagnitude >= totalLength^2 is more efficient than Magnitude >= totalLength
        if ((target.position - bones[0].position).sqrMagnitude >= totalLength * totalLength)
        {
            Vector3 direction = (target.position - positions[0]).normalized;

            // Set everything after root.
            for (int i = 1; i < positions.Length; i++)
            {
                positions[i] = positions[i - 1] + direction * bonesLength[i - 1];
            }
        } // Curve armature to reach target.
        else
        {
            // Idk what this does.
            for (int i = 0; i < positions.Length - 1; i++)
                positions[i + 1] = Vector3.Lerp(positions[i + 1], positions[i] + rootRotDif * startDirChild[i], snapBackStrength);


            // Calculate the curvature up to iterations times
            for (int i = 0; i < iterations; i++)
            {
                // Backwards calculation
                for (int j = positions.Length - 1; j > 0; j--)
                {
                    // If this is the leaf, set leaf to target position.
                    if (j == positions.Length - 1)
                        positions[j] = target.position;
                    // Otherwise, set position to the bone length along the distance from this bone to the next bone
                    else
                        positions[j] = positions[j + 1] + (positions[j] - positions[j + 1]).normalized * bonesLength[j];
                }


                // Forwards calculation
                for (int j = 1; j < positions.Length; j++)
                {
                    // Set position to the bone length along the distance from this bone to the last bone
                    positions[j] = positions[j - 1] + (positions[j] - positions[j - 1]).normalized * bonesLength[j - 1];
                }

                // Calculate if we are close enough, if yes, stop looping.
                if ((positions[positions.Length - 1] - target.position).sqrMagnitude < precision * precision)
                    break;
            }
        }

        // Correct positions by moving toward pole.
        if (pole != null)
        {
            // For each child other than leaf.
            for (int i = 1; i < positions.Length - 1; i++)
            {
                // Create a plane at prev bone whose normal points to next bone.
                Plane plane = new Plane(positions[i + 1] - positions[i - 1], positions[i - 1]);
                // Project the pole onto the plane.
                Vector3 projectedPole = plane.ClosestPointOnPlane(pole.position);
                // Project this bone onto the plane.
                Vector3 projectedBone = plane.ClosestPointOnPlane(positions[i]);
                // Calculate the angle from the projected bone to the projected pole on the plane.
                float angle = Vector3.SignedAngle(projectedBone - positions[i - 1], projectedPole - positions[i - 1], plane.normal);
                // Set the bone to the previous bone, plus the distance between the previous bone and this bone rotated by angle.
                positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (positions[i] - positions[i - 1]) + positions[i - 1];
            }
        }


        // Set bones to positions and rotations.
        for (int i = 0; i < positions.Length; i++)
        {
            // Rotation of leaf shares rotation of target.
            if (i == positions.Length - 1)
            {
                bones[i].rotation = target.rotation * Quaternion.Inverse(startRotTarget) * startRotBone[i];
            }
            // Rotation of child is relative rotation of it to its child.
            else
            {
                bones[i].rotation = Quaternion.FromToRotation(startDirChild[i], positions[i + 1] - positions[i]) * startRotBone[i];
            }
            bones[i].position = positions[i];
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Transform current = transform;
        for (int i = 0; i < chainLength && current != null && current.parent != null; i++)
        {
            float scale = Vector3.Distance(current.position, current.parent.position) * .1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }

        if (pole != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(pole.position, .5f);
        }

        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(target.position, .5f);
        }
#endif
    }
}
