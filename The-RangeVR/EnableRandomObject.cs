using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRandomObject : MonoBehaviour
{
    public List<GameObject> targets; // List of target prefabs
    private GameObject activeTarget; // Currently active target
    private GameObject previousTarget; // Previously active target

    void Start()
    {
        // Activate a random target from the list
        ActivateRandomTarget();
    }

    // Function to activate a random target from the list
    public void ActivateRandomTarget()
    {
        // Deactivate current target if it exists
        if (activeTarget != null)
        {
            activeTarget.SetActive(false);
            previousTarget = activeTarget;
        }

        // Select a random target from the list, excluding the previous target
        do
        {
            int randomIndex = Random.Range(0, targets.Count);
            activeTarget = targets[randomIndex];
        } while (activeTarget == previousTarget);

        // Activate the selected target
        activeTarget.SetActive(true);
    }

    // Function to be called when a target is hit
    public void TargetHit(GameObject target)
    {
        // Deactivate the hit target
        target.SetActive(false);

        // Activate a new random target
        ActivateRandomTarget();
    }
}

