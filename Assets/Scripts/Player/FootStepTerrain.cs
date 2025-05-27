using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepTerrain : MonoBehaviour
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
    //추가

    public AudioClip[] footStepClips;

    private AudioSource audioSource;
    public PlayerController controller;

    private float time;

    private void Awake()
    {
        foot = this.gameObject.transform;
        audioSource = GetComponent<AudioSource>();
        controller = GetComponentInParent<PlayerController>();
    }
    void Update()
    {
        Ray ray = new Ray(foot.position, Vector3.down);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 0.2f))
        {
            Terrain terrain = hit.collider.GetComponent<Terrain>();

            if (terrain != null)
            {
                int index = GetDominantTerrainTextureIndex(hit.point, terrain);
                string textureName = terrain.terrainData.terrainLayers[index].diffuseTexture.name;

                footStepClips = FootStepClipSwitch(textureName);
                AudioClip curClip = footStepClips[Random.Range(0, footStepClips.Length)];

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
                    else if(controller.curSpeed == controller.sprintSpeed)      //달릴 때
                    {
                        if (time > runPeriod)
                        {
                            audioSource.PlayOneShot(curClip);
                            time = 0;
                        }
                    }
                }
            }
        }
    }

    //클립을 바꿔주는 기능
    private AudioClip[] FootStepClipSwitch(string _textureName)
    {
        string[] _textureNames = _textureName.Split('_'); //텍스처 이름을 '_'로 분리하여 배열로 만듭니다.
        switch (_textureNames[0])
        {
            //추가 및 텍스트 수정 필요
            case "dirt":
                return dirtClips;
            case "grass":
                return grassClips;
            default:
                return null;
        }
    }

    //점프 사운드
    public void JumpClipPlay()
    {
        audioSource.PlayOneShot(footStepClips[Random.Range(0, footStepClips.Length)]);
    }

    //터레인의 텍스처 감지
    int GetDominantTerrainTextureIndex(Vector3 hitPoint, Terrain terrain)
    {
        TerrainData terrainData = terrain.terrainData;

        Vector3 terrainPos = hitPoint - terrain.transform.position;

        int mapX = Mathf.FloorToInt(terrainPos.x / terrainData.size.x * terrainData.alphamapWidth);
        int mapZ = Mathf.FloorToInt(terrainPos.z / terrainData.size.z * terrainData.alphamapHeight);

        float[,,] alphaMap = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        int maxIndex = 0;
        float maxMix = 0;

        for (int i = 0; i < alphaMap.GetLength(2); i++)
        {
            if (alphaMap[0, 0, i] > maxMix)
            {
                maxMix = alphaMap[0, 0, i];
                maxIndex = i;
            }
        }

        return maxIndex;
    }
}
