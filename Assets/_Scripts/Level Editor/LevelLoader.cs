using UnityEngine;
using System.Collections.Generic; 

[System.Serializable] 
public class BlockMaterialMap
{
    public BlockType type;
    public Material material;
}

public class LevelLoader : MonoBehaviour
{
    [Header("Level File")]
    public LevelData levelToLoad; 

    [Header("Prefab & Materials")]
    public GameObject baseBoxPrefab; 
    public List<BlockMaterialMap> materialMap; 

    [Header("Grid Settings")]
    public float gridSpacing = 1.1f; 

    void Start()
    {
        LoadLevel();
    }

    void LoadLevel()
    {
        if (levelToLoad == null) return;

        for (int y = 0; y < levelToLoad.height; y++)
        {
            for (int x = 0; x < levelToLoad.width; x++)
            {
                int index = y * levelToLoad.width + x;
                BlockType type = levelToLoad.blocks[index];

                if (type == BlockType.Empty)
                {
                    continue; 
                }

                Material materialToApply = GetMaterialForBlock(type);
                
                if (materialToApply != null)
                {
                    Vector3 offset = new Vector3(x * gridSpacing, -y * gridSpacing, 0);
                    Vector3 position = transform.position + offset;
                    
                    GameObject newBox = Instantiate(baseBoxPrefab, position, Quaternion.identity);
                    
                    newBox.GetComponent<MeshRenderer>().material = materialToApply;
                    
                    //Tell the BoxEffect what color it is (for the particles)
                    BoxEffect effect = newBox.GetComponent<BoxEffect>();
                    if (effect != null)
                    {
                        effect.SetParticleColor(materialToApply.color);
                    }
                }
            }
        }
    }

    Material GetMaterialForBlock(BlockType type)
    {
        foreach (BlockMaterialMap mapping in materialMap)
        {
            if (mapping.type == type)
            {
                return mapping.material;
            }
        }
        return null;
    }
}