using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cutscene))]
public class CutsceneEdtor : Editor
{
    public override void OnInspectorGUI()
    {
        var cutScene = target as Cutscene;

        using (var space = new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Dialouge"))
                cutScene.AddAction(new DialogueAction());
            else if (GUILayout.Button("Move Actor"))
                cutScene.AddAction(new MoveActorAction());
            else if (GUILayout.Button("Turn Actor"))
                cutScene.AddAction(new TurnActorAction());
        }

        using (var space = new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Teleport Object"))
                cutScene.AddAction(new TeleportObjectAction());
            else if (GUILayout.Button("Enable Object"))
                cutScene.AddAction(new EnableObjectAction());
            else if (GUILayout.Button("Disable Object"))
                cutScene.AddAction(new DisableObjectAction());
        }

        using (var space = new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("NPC Interact"))
                cutScene.AddAction(new NPCInteractAction());
            else if (GUILayout.Button("Fade In"))
                cutScene.AddAction(new FadeInAction());
            else if (GUILayout.Button("Fade Out"))
                cutScene.AddAction(new FadeOutAction());
        }
        base.OnInspectorGUI();
    }
}
