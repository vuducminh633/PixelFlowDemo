using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Pixel Flow/Level Data")]
public class LevelData : ScriptableObject
{
    public int width = 9;
    public int height = 9;

    public List<BlockType> blocks = new();

    public void Initialize()
    {
        // Clear old data and create a new grid of 'Empty' blocks
        blocks.Clear();
        for(int i = 0; i < width * height; i++)
        {
            blocks.Add(BlockType.Empty);
        }
    }
}