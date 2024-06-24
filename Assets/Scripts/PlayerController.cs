using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float balanceSpeed = 5f;
    public float walkSpeed = 2f;
    public float windForce = 1f;
    public Transform balancePole;
    public TextMeshProUGUI angleText, dotText;

    private Rigidbody rb;
    private float balanceInput;
    private bool isFallen = false;

    float angle = 0;
    float angleVelocity;
    float angleAcceleration;
    float dot = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isFallen)
        {
            // Get player input for balancing
            balanceInput = Input.GetAxis("Horizontal");

            // Apply balance force
            Vector3 balanceForce = transform.right * balanceInput * balanceSpeed;
            rb.AddForce(balanceForce);

            // Get player input for moving forward
            float moveInput = Input.GetAxis("Vertical");

            // Move forward only when input is detected
            if (moveInput > 0)
            {
                transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
            }

            // Balance angle update
            dot = Vector2.Dot(transform.up, Vector2.up);
            Drifting();

            if (angle > 90) angle = -90;
            if (angle < -90) angle = 90;

            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Update UI Texts
            angleText.text = angle.ToString();
            dotText.text = dot.ToString();
        }
    }

    public void OnBalance(InputValue inputValue)
    {
        var mXDelta = inputValue.Get<float>();
        Accelerate(-mXDelta / 2);
    }

    void Drifting()
    {
        var remapped = Remap(angle, -90, 90, -1, 1);
        Accelerate(remapped);
    }

    void Accelerate(float amount = 0)
    {
        angleAcceleration += amount;
        angleVelocity = angleAcceleration * Time.deltaTime;
        angle += angleVelocity;
    }

    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wind"))
        {
            // Apply wind force
            Vector3 windDirection = other.transform.forward * windForce;
            rb.AddForce(windDirection, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Player has fallen
            isFallen = true;
            // Implement logic for parallel universe shift
            // For now, just reload the scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
