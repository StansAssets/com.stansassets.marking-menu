#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "MarkingMenuActions", menuName = "ScriptableObjects/MarkingMenuActions", order = 1)]
public class MarkingMenuActions : ScriptableObject
{
    public void RegisterPlayAction()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = !EditorApplication.isPlaying;
#endif
    }    
    
    public void RegisterBuildSettingsAction()
    {
#if UNITY_EDITOR
        EditorApplication.ExecuteMenuItem("File/Build Settings...");
#endif
    }
    
    public void RegisterPlayerSettingsAction()
    {
#if UNITY_EDITOR
        SettingsService.OpenProjectSettings("Project/Player");
#endif
    }  
    
    public void RegisterMarkingMenuSettingsAction()
    {
#if UNITY_EDITOR
        EditorApplication.ExecuteMenuItem("Stan's Assets" + "/" + "Marking Menu" + "/" + "Settings");
#endif
    }   
    
    public void RegisterGitHubPageAction()
    {
#if UNITY_EDITOR
        Application.OpenURL("https://github.com/StansAssets/com.stansassets.marking-menu");
#endif
    }
}
