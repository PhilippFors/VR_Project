using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskSpawner : MonoBehaviour
{
    int timeCount = 0;
    public int nextTaskCount = 30;

    int currentArrIndex;
    public GameObject[] nextTasks;
    void Start()
    {
        MyEventSystem.instance.updateSecond += CountUp;
    }

    void CountUp(int second)
    {
        timeCount++;

        if (timeCount == nextTaskCount)
        {
            AddTask();
            timeCount = 0;
        }
    }
    
    void AddTask()
    {
        if (currentArrIndex < nextTasks.Length)
        {
            Vector3 newPos = InteractUtilities.instance.FindRandominArea();
            if (Physics.CheckBox(newPos + new Vector3(0, 0.1f, 0), new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Ignore))
            {
                while (Physics.CheckBox(newPos + new Vector3(0, 0.1f, 0), new Vector3(0.3f, 0.1f, 0.3f), Quaternion.identity, InteractUtilities.instance.mask, QueryTriggerInteraction.Ignore))
                {
                    newPos = InteractUtilities.instance.FindRandominArea();
                    Debug.Log("Find me that area");
                }
            }
            Instantiate(nextTasks[currentArrIndex], newPos, Quaternion.Euler(0, Random.Range(0f, 20f), 0));

            currentArrIndex++;
        }
    }
}
