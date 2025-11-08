using UnityEngine;
using UnityEditor;

public class LevelEditorWindow : EditorWindow
{
    private LevelData currentLevel;
    private BlockType selectedBlock = BlockType.RedBox;

    [MenuItem("Tools/Pixel Flow Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Pixel Flow Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Pixel Flow Level Editor", EditorStyles.boldLabel);

        // 1. The slot to drag your level file into
        currentLevel = (LevelData)EditorGUILayout.ObjectField("Level To Edit", currentLevel, typeof(LevelData), false);

        if (currentLevel == null)
        {
            EditorGUILayout.HelpBox("Please assign a Level Data file to edit.", MessageType.Warning);
            if (GUILayout.Button("Initialize New Level Grid"))
            {
                if(currentLevel != null) currentLevel.Initialize();
            }
            return;
        }

        // 2. The "Palette" to select your block type
        selectedBlock = (BlockType)EditorGUILayout.EnumPopup("Block to Paint", selectedBlock);

        GUILayout.Space(10);
        GUILayout.Label("Click to paint blocks:", EditorStyles.label);

        // 3. The Grid of Buttons
        for (int y = 0; y < currentLevel.height; y++)
        {
            GUILayout.BeginHorizontal(); 

            for (int x = 0; x < currentLevel.width; x++)
            {
                int index = y * currentLevel.width + x;

                if (currentLevel.blocks.Count != currentLevel.width * currentLevel.height)
                {
                    currentLevel.Initialize();
                }

                BlockType currentType = currentLevel.blocks[index];
                
                GUI.backgroundColor = GetColorForBlock(currentType);

                if (GUILayout.Button("", GUILayout.Width(40), GUILayout.Height(40)))
                {
                    currentLevel.blocks[index] = selectedBlock;
                    
                    EditorUtility.SetDirty(currentLevel);
                }
            }
            GUILayout.EndHorizontal();
        }

        GUI.backgroundColor = Color.white;
    }

    private Color GetColorForBlock(BlockType type)
    {
        switch (type)
        {
            case BlockType.RedBox:    return Color.red;
            case BlockType.GreenBox:  return Color.green;
            case BlockType.BlueBox:   return Color.blue;
            case BlockType.YellowBox: return Color.yellow;
            case BlockType.Empty:     return Color.grey;
            default:                  return Color.white;
        }
    }
}