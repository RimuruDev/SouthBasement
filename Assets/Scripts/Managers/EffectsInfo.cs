using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EffectsInfo : MonoBehaviour
{
    public Sprite burnIndicator;
    public Sprite bleedndicator;
    public Sprite poisonedIndicator;
    public Sprite regenerationIndicator;
}
    
public enum EffectsList
{
    None,
    Burn,
    Bleed,
    Poisoned,
    Regeneration
}