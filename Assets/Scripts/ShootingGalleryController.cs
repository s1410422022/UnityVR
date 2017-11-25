using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;
using UnityEngine.UI;//命名空間
public class ShootingGalleryController : MonoBehaviour//類別
{
    public UIController uiController;//欄位
    public Reticle reticle;
    public SelectionRadial selectionRadial;
    public SelectionSlider selectionSlider;


    public Image timerBar;
    public float gameDuration = 30f;
    public float endDeplay = 1.5f;
    public bool IsPlaying//屬性
    {
        private set;
        get;
    }

    private IEnumerator Start()//方法
    {
        SessionData.SetGameType(SessionData.GameType.SHOOTER180);
        while (true)
        {
            yield return StartCoroutine(StartPhase());
            yield return StartCoroutine(PlayPhase());
            yield return StartCoroutine(EndPhase());
        }
    }

    private IEnumerator StartPhase()
    {
        yield return StartCoroutine(uiController.ShowIntroUI());
        reticle.Show();
        selectionRadial.Hide();
        yield return StartCoroutine(selectionSlider.WaitForBarToFill());
        yield return StartCoroutine(uiController.HideIntroUI());


    }

    private IEnumerator PlayPhase()
    {
        yield return StartCoroutine(uiController.ShowPlayerUI());
        
        IsPlaying = true;
        reticle.Show();
        SessionData.Restart();
        float gameTimer = gameDuration;
        while (gameTimer > 0f)
        {
            
            yield return null;
            gameTimer -= Time.deltaTime;
            timerBar.fillAmount = gameTimer / gameDuration;
            
        }
        IsPlaying = false;
        yield return StartCoroutine(uiController.HidePlayerUI());


    }

    private IEnumerator EndPhase()
    {
        reticle.Hide();
        yield return StartCoroutine(uiController.ShowOutroUI());
        yield return new WaitForSeconds(endDeplay);
        yield return StartCoroutine(selectionRadial.WaitForSelectionRadialToFill());
        yield return StartCoroutine(uiController.HideOutroUI());


    }
}
