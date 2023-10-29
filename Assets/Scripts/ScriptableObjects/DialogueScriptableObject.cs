using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Narration/Dialogue")]
public class DialogueScriptableObject : ScriptableObject
{
    public DialogueKey DialogueKey;

    [Header("Dialogue Text")]
    [TextArea(3, 10)]
    public string Text;
    
    [Header("Dialogue Audio")]
    public AudioClip AudioClip;
}

[Serializable]
public struct DialogueKey 
{
    public State State;
    public Keyword Keyword;

    public DialogueKey(State state, Keyword keyword)
    {
        State = state;
        Keyword = keyword;
    }

    public override bool Equals(object obj)
    {
        if (obj is DialogueKey)
        {
            return (State == ((DialogueKey)obj).State) && (Keyword == ((DialogueKey)obj).Keyword);
        }

        return false;
    }

    public static bool operator ==(DialogueKey a, DialogueKey b)
    {
        return (a.State == b.State) && (a.Keyword == b.Keyword);
    }

    public static bool operator !=(DialogueKey a, DialogueKey b)
    {
        return (a.State != b.State) || (a.Keyword != b.Keyword);
    }

    public override int GetHashCode()
    {
        return State.GetHashCode() ^ Keyword.GetHashCode();
    }
}