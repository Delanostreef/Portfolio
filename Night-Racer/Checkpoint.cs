using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int cpIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<KartLap>())
        {
            KartLap kart = other.GetComponent<KartLap>();

            if (kart.CheckpointIndex == cpIndex - 1)
            {
                kart.CheckpointIndex = cpIndex;
                UIManager.Instance.UpdateCheckpointUI();
            }
        }
    }
}
