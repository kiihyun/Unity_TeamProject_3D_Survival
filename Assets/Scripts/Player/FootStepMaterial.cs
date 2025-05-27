using UnityEngine;

public class FootStepMaterial : MonoBehaviour
{
    public PlayerController controller;
    public FootStep footStep;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
        footStep = GetComponent<FootStep>();
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.2f))
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                string materialName = material.name;
                footStep.footStepClips = footStep.FootStepClipSwitch(materialName);
                footStep.curClip = footStep.footStepClips[Random.Range(0, footStep.footStepClips.Length)];
            }
        }
    }
}
