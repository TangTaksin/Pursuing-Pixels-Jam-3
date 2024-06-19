using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float balanceSpeed = 5f;
    public float walkSpeed = 2f;
    public float windForce = 1f;
    public Transform balancePole;
    private Rigidbody rb;

    private float balanceInput;
    private bool isFallen = false;

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

            // Move forward
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);

            // Rotate balance pole based on input
            balancePole.localRotation = Quaternion.Euler(0, 0, -balanceInput * 30f);
        }
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
