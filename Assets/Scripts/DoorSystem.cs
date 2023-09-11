using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    private HingeJoint joint;
    private JointLimits limits;
    public bool unlocked;

    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<HingeJoint>();
        limits = joint.limits;
    }

    void Update()
    {
        if(unlocked) {
                limits.min = -90;
                limits.max = 90;
                joint.limits = limits;
            }
            else {
                limits.min = 0;
                limits.max = 0;
                joint.limits = limits;
            }
    }
}
