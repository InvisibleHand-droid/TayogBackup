using UnityEngine;

[CreateAssetMenu(fileName = "StringArray", menuName = "ScriptableVariables/StringArray", order = 0)]
public class StringArray : ScriptableGameObject
{
    [TextArea(3, 10)]
    public string[] lines;
}