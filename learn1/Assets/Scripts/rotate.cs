using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    const float hourDegree = -30f, minuteDegree = -6f, secondDegree = -6f;
    [SerializeField]
    Transform hourPivot = default, minutePivot = default, secondPivot = default;
    TimeSpan time;
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time = DateTime.Now.TimeOfDay;
        hourPivot.localRotation = Quaternion.Euler(0f, 0f, hourDegree * (float)time.TotalHours);
        minutePivot.localRotation = Quaternion.Euler(0f, 0f, minuteDegree * (float)time.TotalMinutes);
        secondPivot.localRotation = Quaternion.Euler(0f, 0f, secondDegree * (float)time.TotalSeconds);
    }
}
