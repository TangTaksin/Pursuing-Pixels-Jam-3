using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalanceBar : MonoBehaviour
{
    public Image NeedleImage;
    public float angleOffset;

    public void UpdateBar(float angle, float failAngle)
    {
        var converted = ExtensionMethod.Remap(angle, -failAngle, failAngle, -45, 45);

        NeedleImage.transform.rotation = Quaternion.Euler(0, 0, angleOffset + converted);
    }
}
