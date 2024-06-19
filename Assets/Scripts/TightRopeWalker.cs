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

    float dot = 0;

    public TextMeshProUGUI angletext, dotText;

    private void Update()
    {
        

        //Update Angle Debug
        

        dot = Vector2.Dot(transform.up, Vector2.up);
        Drifting();

        if (angle > 90)
            angle = -90;
        if (angle < -90)
            angle = 90;


        transform.rotation = Quaternion.Euler(0, 0, angle);

        angletext.text = angle.ToString();
        dotText.text = dot.ToString();
    }

    public void OnBalance(InputValue inputValue)
    {
        var mXDelta = inputValue.Get<float>();
        print(mXDelta);

        Accelerate(-mXDelta /2);

    }    

    void Drifting()
    {
        var remaped = ExtensionMethod.Remap(angle, -90, 90, -1, 1);
        Accelerate(remaped);
    }

    void Accelerate(float amount = 0)
    {
        angleAcceleration += amount; 
        angleVelocity = angleAcceleration * Time.deltaTime;
        angle += angleVelocity;
    }    

}
