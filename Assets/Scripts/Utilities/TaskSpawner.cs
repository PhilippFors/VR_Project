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
        if (timeCount == nextTaskCount & second != -1)
        {
            AddTask();
            timeCount = 0;
        }
        timeCount++;
    }

    void AddTask()
    {
        if (currentArrIndex < nextTasks.Length)
        {
            Vector3 newPos = InteractUtilities.instance.FindRandominArea();
            if (Physics.CheckBox(newPos + new Vector3(0, 0.2f, 0), new Vector3(0.3f, 0.35f, 0.3f), Quaternion.identity, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Ignore))
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Physics.CheckBox(newPos + new Vector3(0, 0.2f, 0), new Vector3(0.3f, 0.38f, 0.3f), Quaternion.identity, InteractUtilities.instance.mask, QueryTriggerInteraction.Ignore))
                        newPos = InteractUtilities.instance.FindRandominArea();
                }
            }

            GameObject obj = Instantiate(nextTasks[currentArrIndex], newPos + new Vector3(0, 0.1f, 0), Quaternion.identity);
            obj.transform.LookAt(new Vector3(Camera.main.transform.position.x, obj.transform.position.y, Camera.main.transform.position.z));

            currentArrIndex++;
        }
    }
}
