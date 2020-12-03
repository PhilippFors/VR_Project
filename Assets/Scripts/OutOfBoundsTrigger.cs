using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<JobTask>())
            other.GetComponent<JobTask>().ResetOgPosition();
    }
}
