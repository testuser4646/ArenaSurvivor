using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    Dictionary<int, Character> characters = new();
    public void Register(Character ch)
    {
        characters[ch.GetInstanceID()] = ch;
    }

    public void UnRegister(Character ch)
    {
        characters.Remove(ch.GetInstanceID());
    }

    public Dictionary<int, Character> Characters => characters;
}
