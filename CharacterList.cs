using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character List")]
public class CharacterList : ScriptableObject {
    public List<Character> characters;
}
