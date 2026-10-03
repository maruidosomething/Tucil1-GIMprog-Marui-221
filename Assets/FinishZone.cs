using UnityEngine;

public class FinishZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the finish zone is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            if (SpeedrunManager.Instance != null)
            {
                SpeedrunManager.Instance.FinishLevel();
            }
        }
    }
}