using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTracking : MonoBehaviour
{
    public int minute;
    public int second;
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

    IEnumerator Clock()
    {
        while (true)
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
                yield break;

            yield return null;
        }
    }
}
