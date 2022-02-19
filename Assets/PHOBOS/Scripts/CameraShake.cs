using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float ShakeAmount; //카메라가 흔들리는 힘
    public float ShakeTime;
    Vector3 initalPosition;

    // Start is called before the first frame update
    void Start()
    {
        initalPosition = new Vector3(0.0f, 0.0f, -5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(ShakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * ShakeAmount + initalPosition;
            ShakeTime -= Time.deltaTime;
        } else
        {
            ShakeTime = 0.0f;
            transform.position = initalPosition;
        }
    }
}
