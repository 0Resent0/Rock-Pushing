using UnityEngine;

[System.Serializable] // Just a plain data class, no MonoBehaviour
public class SceneTokenConfig
{
    public string sceneName;       // Name of the scene in Build Settings
    public int tokensToAward = 1;  // Default 1 token
}