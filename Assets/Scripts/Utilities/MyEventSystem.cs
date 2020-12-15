using UnityEngine;

public class MyEventSystem : MonoBehaviour
{
    public static MyEventSystem instance;
    public event System.Action penaltyTrigger;
    public event System.Action<int> updateSecond;
    public event System.Action<int> updateMinute;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateSecond(int second)
    {
        updateSecond?.Invoke(second);
    }
    public void UpdateMinute(int minute)
    {
        updateMinute?.Invoke(minute);
    }

    public void PenaltyTrigger()
    {
        penaltyTrigger?.Invoke();
    }
}
