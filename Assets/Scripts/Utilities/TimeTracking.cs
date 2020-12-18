using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTracking : MonoBehaviour
{
    public int minute = 3;
    public int second = 0;
    float temp = 1;
    private void Start()
    {
        StartCoroutine(Wait());
        StartCoroutine(Clock());
    }

    IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        MyEventSystem.instance.UpdateSecond(second);
        MyEventSystem.instance.UpdateMinute(minute);
    }
    bool stop;
    IEnumerator Clock()
    {
        stop = false;
        while (!stop)
        {
            temp -= Time.deltaTime;
            if (temp <= 0)
            {
                second--;
                temp += 1;
                MyEventSystem.instance.UpdateSecond(second);
            }

            if (second < 0)
            {
                minute--;
                second = 59;
                MyEventSystem.instance.UpdateMinute(minute);
                MyEventSystem.instance.UpdateSecond(second);
            }
            
            if (minute == 0 && second == 0)
                stop = true;

            yield return null;
        }

        GameManager.instance.dead = true;
        JobTask[] tasks = FindObjectsOfType<JobTask>();
        foreach (JobTask task in tasks)
            task.StopTaskReduction();
        yield return new WaitForSeconds(3f);
        GameManager.instance.ReturnToStartMenu();
    }
}
