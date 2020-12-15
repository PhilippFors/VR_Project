using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool dead = false;
    [SerializeField] UIManager uiManager;
    public Animation anim;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        anim.Play("FadeIn");
    }

    public void ReturnToStartMenu()
    {
        SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
    }

    public void Death()
    {
        dead = true;
        uiManager.TurnOnDeathScreen();
        StartCoroutine(ReturnToDeath());
    }

    IEnumerator ReturnToDeath()
    {
        yield return new WaitForSeconds(3f);
        anim.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        ReturnToStartMenu();
    }

}
