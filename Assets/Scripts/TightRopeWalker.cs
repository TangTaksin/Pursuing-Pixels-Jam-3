using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TightRopeWalker : MonoBehaviour
{
    bool fall;

    float angle = 0;
    float angleVelocity;
    float angleAcceleration;

    float influence;
    public Transform objectToRotate;

    public Camera cam;
    public Transform camTarget;

    Animator _animator;
    public AnimationCurve FlailCurve;

    public Rigidbody _rigidbody;

    Vector3 camOffset;

    [Range(.1f,2)] public float mouseSentivity = 1;

    [Range(0, 90)] public float FailAngle = 45f;
    public BalanceBar _balanceBar;

    public TextMeshProUGUI angletext;

    public delegate void PlayerEvent();
    public static PlayerEvent OnFall;

    private void OnEnable()
    {
        fall = false;

        _animator = GetComponent<Animator>();
        _animator.Play("Walking");

        _rigidbody.isKinematic = true;

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
        Drifting();

        objectToRotate.rotation = Quaternion.Euler(0, 0, angle);

        angletext.text = angle.ToString();

        FailCheck();
        Animate();
        CamFollow();
        _balanceBar?.UpdateBar(angle, FailAngle);
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

        //angle += -mXDelta * mouseSentivity * Time.deltaTime;

        Accelerate(-mXDelta * mouseSentivity);

    }    

    void Drifting()
    {
        var remaped = ExtensionMethod.Remap(angle, -90, 90, -1, 1);
        Accelerate(remaped);
    }

    void Accelerate(float amount = 0)
    {
        if (fall)
            return;

        angleAcceleration += amount + influence * Time.deltaTime; 
        angleVelocity = angleAcceleration * Time.deltaTime;
        angle += angleVelocity;

    }    

    void Animate()
    {
        var flailweight = FlailCurve.Evaluate(angle / FailAngle);

        _animator.SetLayerWeight(1, flailweight);
    }

    void FailCheck()
    {
        if (Mathf.Abs(angle) >= FailAngle && !fall)
        {
            if (angle < 0)
                angle = -FailAngle;
            else
                angle = FailAngle;

            fall = true;
            _animator.SetTrigger("Fall");

            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Vector3.down * angleVelocity * 2;

            OnFall?.Invoke();
        }
    }

    void CamFollow()
    {
        cam.transform.position = new Vector3(camTarget.transform.position.x, 0, camTarget.transform.position.z) + camOffset;
    }
}
