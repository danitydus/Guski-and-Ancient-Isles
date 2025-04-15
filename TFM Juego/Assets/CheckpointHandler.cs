using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    public List<bool> checkpoints = new List<bool>(new bool[10]);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Check"))
        {
            int index;
            if (int.TryParse(other.gameObject.name.Substring(5), out index) && index >= 1 && index <= 10)
            {
                checkpoints[index - 1] = true;
                Debug.Log($"Checkpoint {index} activado.");
            }
        }
    }
}
