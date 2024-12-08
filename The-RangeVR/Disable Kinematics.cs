using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableKinematics : MonoBehaviour
{
    private Rigidbody rb_;
    public void disableKinematic()
    {
        rb_ = this.gameObject.GetComponent<Rigidbody>();
        rb_.isKinematic = false;
    }
}
