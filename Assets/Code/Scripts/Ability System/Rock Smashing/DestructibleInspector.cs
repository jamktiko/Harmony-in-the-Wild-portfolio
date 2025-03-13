using UnityEngine;
using UnityEditor;
using Unity;
namespace Code.Scripts.Ability_System.Rock_Smashing
{
    [CustomEditor( typeof(Destructible) )]
    public class DestructibleInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            Destructible destructible = target as Destructible;
            base.OnInspectorGUI();
            
            destructible.IsQuestRock = EditorGUILayout.Toggle("IsQuestRock", destructible.IsQuestRock);
            if (destructible.IsQuestRock)
            {
                destructible.MinRequiredForce = EditorGUILayout.FloatField("Minimum Required Force", destructible.MinRequiredForce );
                destructible.MaxRequiredForce = EditorGUILayout.FloatField("Maximum Required Force", destructible.MaxRequiredForce );
            }
            
            if (GUI.changed)
                EditorUtility.SetDirty(destructible);
        }
    }
}