using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TightRopeWalker : MonoBehaviour
{
    float angle = 0;
    float angleVelocity;
    float angleAcceleration;

    float influence;
    public Transform objectToRotate;

    public Camera cam;
    public Transform camTarget;

    Vector3 camOffset;

    [Range(.1f,2)] public float mouseSentivity = 1;

    float dot = 0;
    [Range(0.1f, 0.9f)] float dotFail = .45f;

    public TextMeshProUGUI angletext, dotText;

    private void OnEnable()
    {
        if (!objectToRotate)
        {
            objectToRotate = transform;
        }

        if (!cam)
        {
            cam = Camera.main;
        }

        camOffset = cam.transform.position - camTarget.position;

        if (!camTarget)
        {
            camTarget = transform;
        }
    }

    private void Update()
    {
        dot = Vector2.Dot(objectToRotate.up, Vector2.up);
        Drifting();

        if (angle > 90)
            angle = -90;
        if (angle < -90)
            angle = 90;


        objectToRotate.rotation = Quaternion.Euler(0, 0, angle);

        angletext.text = angle.ToString();
        dotText.text = dot.ToString();

        FailCheck();
        CamFollow();
    }

    public void SetInfluence(float value)
    {
        influence = value;
    }

    public void SetSensitivity(float value)
    {
        mouseSentivity = value;
    }

    public void OnBalance(InputValue inputValue)
    {
        var mXDelta = inputValue.Get<float>();
        print(mXDelta);

        Accelerate(-mXDelta * mouseSentivity);

    }    

    void Drifting()
    {
        var remaped = ExtensionMethod.Remap(angle, -90, 90, -1, 1);
        Accelerate(remaped);
    }

    void Accelerate(float amount = 0)
    {
        angleAcceleration += amount; 
        angleVelocity = influence + angleAcceleration * Time.deltaTime;
        angle += angleVelocity;
    }    

    void FailCheck()
    {
        if (dot <= dotFail)
        {

        }
    }

    void CamFollow()
    {
        cam.transform.position = camTarget.transform.position + camOffset;
    }
}
