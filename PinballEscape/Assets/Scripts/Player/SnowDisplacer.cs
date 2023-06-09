using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowDisplacer : MonoBehaviour
{
    [SerializeField] Shader drawShader;
    [SerializeField] GameObject[] terrain;
    int currTerrainIndex = -1;
    Material[] snowMaterials;

    [Header("Brush Settings")]
    [SerializeField] LayerMask layermask;
    [Range(0, 10)]
    [SerializeField] float _brushSize;
    [Range(0, 2)]
    [SerializeField] float _brushStrength;

    [Header("SplatMap Settings")]
    [Range(64, 1024)]
    [SerializeField] int splatMapSize = 512;

    RenderTexture splatMap;
    Material snowMaterial, drawMaterial;

    RaycastHit groundHit;
    RenderTexture temp;


    // Start is called before the first frame update
    void Start()
    {
        drawMaterial = new Material(drawShader);
        drawMaterial.SetFloat("_Strength", _brushStrength);
        drawMaterial.SetFloat("_Size", _brushSize);
        snowMaterials = new Material[terrain.Length];
        for (int i = 0; i < terrain.Length; i++)
        {
            SetSplatTexture(i);
        }
    }

    void SetSplatTexture(int index)
    {
        snowMaterials[index] = terrain[index].GetComponent<MeshRenderer>().material;
        splatMap = new RenderTexture(splatMapSize, splatMapSize, 0, RenderTextureFormat.ARGBFloat);
        snowMaterials[index].SetTexture("_Splat", splatMap);
    }

    void FindCorrectTerrainObject(GameObject o)
    {
        for (int i = 0; i < terrain.Length; i++)
        {
            if (terrain[i] == o)
            {
                currTerrainIndex = i;
                splatMap = (RenderTexture)snowMaterials[i].GetTexture("_Splat");
            }
        }
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out groundHit, 1f, layermask))
        {   // very costly on performace need optimization
            if (currTerrainIndex < 0 || groundHit.collider.gameObject != terrain[currTerrainIndex]) FindCorrectTerrainObject(groundHit.collider.gameObject);
            drawMaterial.SetVector("_Coordinate", new Vector4(groundHit.textureCoord.x, groundHit.textureCoord.y, 0, 0));
            temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(splatMap, temp);
            Graphics.Blit(temp, splatMap, drawMaterial);
            RenderTexture.ReleaseTemporary(temp); // release render texture to not take space in memory
        }
    }
}
