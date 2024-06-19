using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    public float gustInterval = 5f;
    public float gustDuration = 2f;

    private bool isGusting = false;

    void Start()
    {
        StartCoroutine(GustRoutine());
    }

    IEnumerator GustRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(gustInterval);
            isGusting = true;
            yield return new WaitForSeconds(gustDuration);
            isGusting = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (isGusting && other.gameObject.CompareTag("Player"))
        {
            // Apply gust effect
            other.GetComponent<Rigidbody>().AddForce(transform.forward * 10f, ForceMode.Impulse);
        }
    }
}
