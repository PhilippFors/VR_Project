using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenu : MonoBehaviour
{
    public Animation anim;

    private void Start()
    {
        anim.Play("FadeIn");
    }
    
    public void StartGame()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        anim.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync("Main", LoadSceneMode.Single);
    }
}
