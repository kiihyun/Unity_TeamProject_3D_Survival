using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepTerrain : MonoBehaviour
{
    public PlayerController controller;
    public FootStep footStep;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
        footStep = GetComponent<FootStep>();
    }
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.2f))
        {
            Terrain terrain = hit.collider.GetComponent<Terrain>();

            if (terrain != null)
            {
                int index = GetDominantTerrainTextureIndex(hit.point, terrain);
                string textureName = terrain.terrainData.terrainLayers[index].diffuseTexture.name;

                footStep.footStepClips = footStep.FootStepClipSwitch(textureName);
                footStep.curClip = footStep.footStepClips[Random.Range(0, footStep.footStepClips.Length)];
            }
        }
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
