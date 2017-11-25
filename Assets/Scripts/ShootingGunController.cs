using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.VR;
public class ShootingGunController : MonoBehaviour
{
    public AudioSource audiosource;
    public VRInput vrInput;
    public float defaultLineLength = 70f; //雷射長度
    public ParticleSystem flareParticle;
    public Transform gunEnd;
    public LineRenderer gunFlare;
    public float gunFlareVisibleSeconds = 0.07f;
    //......................................
    public Transform cameraTransform;
    public Reticle reticle;
    public Transform gunContainer;
    public float damping = 0.5f;
    public float dampingCoef = 20f;
    public float gunContainerSmooth = 10f;
    //......................................
    private void OnEnable()
    {
        vrInput.OnDown += HandleDown;//事件用法+=

    }
    private void OnDisable()
    {
        vrInput.OnDown -= HandleDown;//有+=就要有-=
    }

    private void HandleDown()       //方法
    {
       
        StartCoroutine(Fire());
       
    }
    private IEnumerator Fire()
    {
        audiosource.Play(); 
    
        float lineLength = defaultLineLength;   //todo判斷有無射到東西
        flareParticle.Play();                   //粒子
        gunFlare.enabled = true;
        yield return StartCoroutine(MoveLineRenderer(lineLength)); //先回去上面，在往下
        gunFlare.enabled = false;
    }
    private IEnumerator MoveLineRenderer(float lineLength)
            {
            float timer = 0f;
            while (timer < gunFlareVisibleSeconds)
            {
                gunFlare.SetPosition(0, gunEnd.position);                              //起點
                gunFlare.SetPosition(1, gunEnd.position + gunEnd.forward*lineLength);  //正方向
                yield return null;
                timer += Time.deltaTime;
            }
        }

    private void Update()
    {
        transform.rotation=Quaternion.Slerp(transform.rotation,InputTracking.GetLocalRotation(VRNode.Head),damping * (1 - Mathf.Exp(dampingCoef * Time.deltaTime)));
        transform.position = cameraTransform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(reticle.ReticleTransform.position - gunContainer.position);
        gunContainer.rotation = Quaternion.Slerp(gunContainer.rotation, lookAtRotation, gunContainerSmooth * Time.deltaTime);
    }



}











