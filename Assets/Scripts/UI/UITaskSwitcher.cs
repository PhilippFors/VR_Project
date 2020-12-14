using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITaskSwitcher : IInteractable
{
    [SerializeField] IInteractable task;
    [SerializeField] TMPro.TextMeshProUGUI taskText;
    string click = "C", hold = "H", look = "L", drag = "D", throwing = "T";

    [Tooltip("[C]lick -> [H]old -> [L]ook -> [D]rag -> [T]hrow -> ...")]
    [SerializeField] string[] interact = { "C", "H", "L", "D", "T" };
    string originalT;
    [SerializeField] bool stopCoroutine;
    private void Start()
    {
        if (task.clickable)
        {
            taskText.text = click;
            originalT = click;
        }
        else if (task.holdable)
        {
            taskText.text = hold;
            originalT = hold;
        }
        else if (task.lookable)
        {
            taskText.text = look;
            originalT = look;
        }
        else if (task.draggable)
        {
            taskText.text = drag;
            originalT = drag;
        }
        else if (task.throwable)
        {
            taskText.text = throwing;
            originalT = throwing;
        }
    }

    bool check = false;
    private void Update()
    {
        if (StressManager.instance.currentStress.Value >= 70f)
        {
            if (!check)
                GetComponent<Canvas>().enabled = true;

            check = true;
        }
        else
        {
            if (check)
            {
                task.clickable = false;
                task.holdable = false;
                task.lookable = false;
                task.draggable = false;
                task.throwable = false;

                if (originalT.Equals(click))
                {
                    task.clickable = true;
                    taskText.text = click;
                }
                else if (originalT.Equals(hold))
                {
                    task.holdable = true;
                    taskText.text = hold;
                }

                else if (originalT.Equals(look))
                {
                    taskText.text = look;
                    task.lookable = true;
                }

                else if (originalT.Equals(drag))
                {
                    task.draggable = true;
                    taskText.text = drag;
                }
                else if (originalT.Equals(throwing))
                {
                    task.throwable = true;
                    taskText.text = throwing;
                }

                GetComponent<Canvas>().enabled = false;
                check = false;
            }
        }
    }

    public override void PointerClick()
    {
        CheckTask();
    }

    void CheckTask()
    {
        if (task.clickable)
        {
            task.clickable = false;
            CheckNextInteraction(1);
        }
        else if (task.holdable)
        {
            task.holdable = false;
            CheckNextInteraction(2);
        }
        else if (task.lookable)
        {
            task.lookable = false;
            CheckNextInteraction(3);
        }
        else if (task.draggable)
        {
            task.draggable = false;
            CheckNextInteraction(4);
        }
        else if (task.throwable)
        {
            task.throwable = false;
            CheckNextInteraction(0);
        }
    }

    void CheckNextInteraction(int startindex)
    {
        string temp = "";
        int i = startindex;
        while (temp == "")
        {
            if (interact[i] != null)
            {
                if (interact[i].Equals(hold))
                {
                    task.rb.useGravity = false;
                    task.holdable = true;
                    taskText.text = hold;
                }
                else if (interact[i].Equals(look))
                {
                    task.rb.useGravity = false;
                    task.lookable = true;
                    taskText.text = look;
                }
                else if (interact[i].Equals(drag))
                {
                    task.rb.useGravity = true;
                    task.draggable = true;
                    taskText.text = drag;
                }
                else if (interact[i].Equals(throwing))
                {
                    task.rb.useGravity = true;
                    task.throwable = true;
                    taskText.text = throwing;
                }
                else if (interact[i].Equals(click))
                {
                    task.rb.useGravity = false;
                    task.clickable = true;
                    taskText.text = click;
                }
                temp = interact[i];
            }

            if (i < interact.Length - 1)
                i++;
            else
                i = 0;
        }
    }
}
