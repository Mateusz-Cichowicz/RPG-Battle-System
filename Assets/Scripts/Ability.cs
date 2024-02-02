using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ability : ScriptableObject
{
    public Sprite icon;
    public string abilityName;
    public Texture2D cursor;

    public virtual void Activate() { }
    public virtual void Activate(Character character) { }

}
