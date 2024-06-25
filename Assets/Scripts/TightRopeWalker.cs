using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TightRopeWalker : MonoBehaviour
{
    bool uncontrolabled = true;
    bool paused = false;
    bool gameStarted = false;

    float angle = 0;
    float angleVelocity;
    float angleAcceleration;

    float influence;
    public Transform objectToRotate;

    Animator _animator;
    float _animSpeedSave;
    public float walkSpeed = 3;
    float walkMod = 1;
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
        PauseSystem.OnPaused += OnPause;
    }

    private void OnDisable()
    {
        ParallelZone.onCountdownStart -= OnCountdownStart;
        ParallelZone.onCountdownOver -= OnCountdownEnd;
        PauseSystem.OnPaused -= OnPause;
    }

    private void Update()
    {
        Drifting();

        objectToRotate.rotation = Quaternion.Euler(0, 0, angle);

        FailCheck();
        Animate();
        MoveForward();

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
        if (uncontrolabled || paused)
            return;

        angleAcceleration += amount + influence * Time.deltaTime;
        angleVelocity = angleAcceleration * Time.deltaTime;
        angle += angleVelocity;

    }

    void MoveForward()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
            objectToRotate.transform.position += Vector3.forward * walkSpeed * Time.deltaTime * walkMod;
    }

    void Animate()
    {
        var flailweight = FlailCurve.Evaluate(Mathf.Abs(angle) / FailAngle);

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
            _animator.SetFloat("Side", angle.Remap(-90,90,-1,-1));
            _animator.Play("Fall");
            AudioManager.Instance.PlaySFX(AudioManager.Instance.nooooVoice);

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
        gameStarted = true;

        OnResume();
    }



    void OnPause()
    {
        if (gameStarted)
        {
            paused = true;
            _animator.speed = 0;
            walkMod = _animator.speed;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnResume()
    {
        paused = false;
        _animator.speed = 1;
        walkMod = _animator.speed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PlayFootstep()
    {
        AudioManager.Instance.PlayFootstepSFX();

    }
}
