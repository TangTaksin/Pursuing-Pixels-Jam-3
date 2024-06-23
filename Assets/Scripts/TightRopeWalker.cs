using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TightRopeWalker : MonoBehaviour
{
    bool uncontrolabled = true;

    float angle = 0;
    float angleVelocity;
    float angleAcceleration;

    float influence;
    public Transform objectToRotate;

    Animator _animator;
    public AnimationCurve FlailCurve;

    public Rigidbody _rigidbody;

    [Range(.1f, 2)] public float mouseSentivity = 1;

    [Range(0, 90)] public float FailAngle = 45f;
    public BalanceBar _balanceBar;

    public delegate void PlayerEvent();
    public static PlayerEvent OnFall;

    private void OnEnable()
    {
        uncontrolabled = true;

        _animator = GetComponent<Animator>();

        _rigidbody.isKinematic = true;


        if (!objectToRotate)
        {
            objectToRotate = transform;
        }

        ParallelZone.onCountdownStart += OnCountdownStart;
        ParallelZone.onCountdownOver += OnCountdownEnd;
    }

    private void OnDisable()
    {
        ParallelZone.onCountdownStart -= OnCountdownStart;
        ParallelZone.onCountdownOver -= OnCountdownEnd;
    }

    private void Update()
    {
        Drifting();

        objectToRotate.rotation = Quaternion.Euler(0, 0, angle);

        FailCheck();
        Animate();

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
        if (uncontrolabled)
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
        if (Mathf.Abs(angle) >= FailAngle && !uncontrolabled)
        {
            if (angle < 0)
                angle = -FailAngle;
            else
                angle = FailAngle;

            uncontrolabled = true;
            _animator.SetTrigger("Fall");

            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Vector3.down * Mathf.Abs(angleVelocity) * 2;

            OnFall?.Invoke();
        }
    }

    void OnCountdownStart()
    {
        uncontrolabled = true;
        _animator.Play("Walking");
    }

    void OnCountdownEnd()
    {
        uncontrolabled = false;
    }
}
