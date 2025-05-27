using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class FootStep : MonoBehaviour
{
    [Header("Settings")]
    public Transform foot;

    [Header("Walk")]
    public float walkPeriod = 0.8f;

    [Header("Sprint")]
    public float runPeriod = 0.4f;

    [Header("Clips")]
    public AudioClip[] dirtClips;
    public AudioClip[] grassClips;
    public AudioClip[] woodClips;
    //추가

    public FootStepTerrain footStepTerrain;
    public FootStepMaterial footStepMaterial;

    public AudioClip[] footStepClips;
    public AudioClip curClip;
    private AudioSource audioSource;
    private PlayerController controller;

    private float time = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        foot = this.gameObject.transform;
        footStepTerrain = GetComponent<FootStepTerrain>();
        footStepMaterial = GetComponent<FootStepMaterial>();
        audioSource = GetComponent<AudioSource>();
        controller = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (controller.IsGrounded())
        {
            time += Time.deltaTime;
            if (controller.curSpeed == controller.walkSpeed)  //걸을 때
            {
                if (time > walkPeriod)
                {
                    audioSource.PlayOneShot(curClip);
                    time = 0;
                }
            }
            else if (controller.curSpeed == controller.sprintSpeed)      //달릴 때
            {
                if (time > runPeriod)
                {
                    audioSource.PlayOneShot(curClip);
                    time = 0;
                }
            }
        }
    }

    //클립을 바꿔주는 기능
    public AudioClip[] FootStepClipSwitch(string _textureName)
    {
        string[] _textureNames = _textureName.Split('_'); //텍스처 이름을 '_'로 분리하여 배열로 만듭니다.
        
        switch (_textureNames[0])
        {
            //추가 및 텍스트 수정 필요
            case "dirt":
                return dirtClips;
            case "grass":
                return grassClips;
            case "wood (Instance)":
                return woodClips;
            default:
                return null;
        }
    }

    //점프 사운드
    public void JumpClipPlay()
    {
        audioSource.PlayOneShot(footStepClips[Random.Range(0, footStepClips.Length)]);
    }
}
