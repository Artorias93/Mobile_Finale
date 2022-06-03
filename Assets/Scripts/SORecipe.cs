using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct RecipeEntry
{
    public string m_minigameScene;
    public Difficulty m_difficulty;
}
[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]

public class SORecipe : ScriptableObject
{
    [SerializeField] private RecipeEntry[] m_entry;

    public RecipeEntry getEntry(int i) {
        return m_entry[i];
    }

    public int getLength()
    {
        return m_entry.Length;
    }
}
