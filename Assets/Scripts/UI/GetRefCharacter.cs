﻿using UnityEngine;

public class GetRefCharacter : MonoBehaviour
{
    [Range(1,2)] [SerializeField] int _playerNumber;

    public void ChangeCharacter()
    {
        DestroyAllChildobjects();

        if(GameController.PlayerCharacter[_playerNumber] != null)
            Instantiate(GameController.PlayerCharacter[_playerNumber], transform);
    }

    private void DestroyAllChildobjects()
    {
        GameObject[] childObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects[i] = transform.GetChild(i).gameObject;
        }
        foreach (var child in childObjects)
        {
            Destroy(child);
        }
    }
}
