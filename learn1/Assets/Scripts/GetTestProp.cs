using System;
using UnityEngine;
using TMPro;

public class GetTestProp : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI display = default;
    [SerializeField]
    GameObject testObject = default;
    // Start is called before the first frame update
    void Start()
    {
        display.SetText("ERROR");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.zero;
        dir = GetSphereVelocity();
        display.SetText("Val\n{0:3}\n{1:3}\n{2:3}", dir.x, dir.y, dir.z);
    }

    Vector3 GetSphereVelocity()
    {
        MovingSphere movScript = null;
        RigidbodySphere rigScript = null;
        movScript = testObject.GetComponent(typeof(MovingSphere)) as MovingSphere;
        if (movScript != null)
        {
            return movScript.velocity;
        }
        rigScript = testObject.GetComponent(typeof(RigidbodySphere)) as RigidbodySphere;
        if (rigScript != null)
        {
            return rigScript.velocity;
        }
        return Vector3.zero;
    }
}
