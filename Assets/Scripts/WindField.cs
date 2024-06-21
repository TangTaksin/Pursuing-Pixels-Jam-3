using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindField : MonoBehaviour
{
    [Tooltip("possive is left, negative is right")]
    public float windStrenght;
    TightRopeWalker walker;

    private void OnTriggerEnter(Collider other)
    {
        walker = other.GetComponent<TightRopeWalker>();
        walker.SetInfluence(windStrenght);

        print("Entered Wind Zone");
    }

    private void OnTriggerExit(Collider other)
    {
        walker.SetInfluence(0);
        walker = null;

        print("Exited Wind Zone");
    }
}
