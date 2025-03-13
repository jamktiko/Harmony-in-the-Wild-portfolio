using UnityEditor;
using UnityEngine;

public class HarmonyLinks : Editor
{
    [MenuItem("HITW/Documentation", priority = 100, secondaryPriority = 100)]
    public static void OpenDocumentation() => Application.OpenURL("discord://discord.com/channels/1146760094583902338/1146760153882968175/1319271030879027230");
    
    [MenuItem("HITW/Daily Scrum", priority = 100, secondaryPriority = 100)]
    public static void OpenDailyScrum() => Application.OpenURL("discord://discord.com/channels/1146760094583902338/1153291650316128307");


    [MenuItem("HITW/Task list", priority = 200, secondaryPriority = 100)]
    public static void OpenTasklist() => Application.OpenURL("https://github.com/orgs/jamktiko/projects/22/views/6");
    
    [MenuItem("HITW/GitHub Repository", priority = 200, secondaryPriority = 100)]
    public static void OpenRepository() => Application.OpenURL("https://github.com/jamktiko/Harmony-in-the-Wild");
    
    [MenuItem("HITW/Report a bug", priority = 300, secondaryPriority = 100)]
    public static void OpenBugReport() => Application.OpenURL("https://github.com/jamktiko/Harmony-in-the-Wild/issues/new");
}
