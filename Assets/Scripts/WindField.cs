using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindField : MonoBehaviour
{
    [Tooltip("Positive values for left, negative values for right")]
    public float windStrength;
    private TightRopeWalker walker;
    public ParticleSystem windParticles; // Reference to the particle system

    private void OnTriggerEnter(Collider other)
    {
        walker = other.GetComponent<TightRopeWalker>();
        if (walker != null)
        {
            walker.SetInfluence(windStrength);
            PlayParticles();
            print("Entered Wind Zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (walker != null)
        {
            walker.SetInfluence(0);
            walker = null;
            StopParticles();
            print("Exited Wind Zone");
        }
    }

    private void OnDrawGizmos()
    {
        // Get the BoxCollider component
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            return;
        }

        // Set the gizmo color
        Gizmos.color = new Color(0, 0, 1, 0.5f); // Blue with some transparency

        // Draw a cube to represent the wind field
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(boxCollider.center, boxCollider.size);

        // Draw an arrow to represent wind direction
        Gizmos.color = Color.red;
        Vector3 boxCenter = transform.TransformPoint(boxCollider.center);
        // Set the z axis to 0
        boxCenter.z = 0;
        Vector3 direction = windStrength > 0 ? Vector3.left : Vector3.right;
        Gizmos.DrawRay(boxCenter, direction * Mathf.Abs(windStrength));
    }

    private void PlayParticles()
    {
        if (windParticles != null && !windParticles.isPlaying)
        {
            windParticles.Play();
        }
    }

    private void StopParticles()
    {
        if (windParticles != null && windParticles.isPlaying)
        {
            windParticles.Stop();
        }
    }
}
