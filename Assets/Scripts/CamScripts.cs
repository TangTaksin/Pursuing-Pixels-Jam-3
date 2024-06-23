using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScripts : MonoBehaviour
{
    public Camera cam;
    public Transform camTarget;
    Animator _animator;

    public Vector3 camOffset ;
    Quaternion camRotOffset;

    private void Start()
    {
        cam = Camera.main;
        gameObject.TryGetComponent<Animator>(out _animator);

        camRotOffset = cam.transform.rotation;
    }

    private void Update()
    {
        CamFollow();
    }

    public void DisableAnimation()
    {
        if (_animator)
            _animator.enabled = false;
    }

    void CamFollow()
    {
        cam.transform.position = new Vector3(camTarget.position.x, 0, camTarget.position.z) + camOffset;
        cam.transform.rotation = camRotOffset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw a cube where the BoxCollider is
        if (camTarget)
            Gizmos.DrawWireSphere(new Vector3(camTarget.position.x, 0, camTarget.position.z) + camOffset, 0.5f);
    }
}
