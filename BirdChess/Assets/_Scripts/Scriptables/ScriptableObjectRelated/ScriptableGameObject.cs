using UnityEngine;

public class ScriptableGameObject : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField] private string DeveloperNote;
}