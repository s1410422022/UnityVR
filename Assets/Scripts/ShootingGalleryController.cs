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

    public Collider spawnCollider;
    public ObjectPool targetObjectPool;//優化
    public float spawnProbabilty = 0.7f;
    public float spawnInterval = 1f;


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
        float spawnTimer = 0f;

        while (gameTimer > 0f)
        {
            if (spawnTimer <= 0f)
            {
                if (Random.value<spawnProbabilty)
                {
                    spawnTimer = spawnInterval;
                    Spawn();
                }
            }
            yield return null;
            gameTimer -= Time.deltaTime;
            spawnTimer-= Time.deltaTime;
            timerBar.fillAmount = gameTimer / gameDuration;
            
        }
        IsPlaying = false;
        yield return StartCoroutine(uiController.HidePlayerUI());


    }

    private void Spawn()
    {
        GameObject target = targetObjectPool.GetGameObjectFromPool();
        target.transform.position = SpawnPosition();
    }
    private Vector3 SpawnPosition()
    {
        Vector3 center = spawnCollider.bounds.center;
        Vector3 extent = spawnCollider.bounds.extents;
        float x = Random.Range(center.x - extent.x, center.x + extent.x);
        float y = Random.Range(center.y - extent.y, center.y + extent.y);
        float z = Random.Range(center.z - extent.z, center.z + extent.z);
        return new Vector3(x, y, z);
  

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
