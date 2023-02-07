using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

[CustomEditor(typeof(NPC))]
public class NPCEditor : Editor
{
    private NPC _target;

    private void OnEnable()
    {
        _target = (NPC)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _target.autoSetWaitUntilPlayerAction = EditorGUILayout.Toggle("Automatically Set 'waitUntilPlayerAction'", _target.autoSetWaitUntilPlayerAction);
        if (_target.autoSetWaitUntilPlayerAction)
        {
            // SetWaitUntilPlayerActions();
        }

    }

    private void SetWaitUntilPlayerActions()
    {
        foreach (NPCConditionals npcConditional in _target.Conds)
        {
            for (int i = 0; i < npcConditional.dialogueChain.Count; i++)
            {
                DialogueData dialogueData = npcConditional.dialogueChain[i];
                bool isLast = i == npcConditional.dialogueChain.Count - 1;
                bool dontInterrupt = dialogueData.dontInterrupt;
                bool advanceDialogueManually = dialogueData.advanceDialogueManually;
                npcConditional.dialogueChain[i].waitUntilPlayerAction = !isLast && !dontInterrupt && !advanceDialogueManually;
            }
        }
    }
}