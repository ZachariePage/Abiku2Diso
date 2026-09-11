using System.Collections.Generic;
using UnityEngine;

public class EncoreUITracker : MonoBehaviour
{
    private List<GameObject> childs = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in transform)
        {
            childs.Add(child.gameObject);
        }

        PlayerBattleStats.Instance.onMomentumChanged += RefreshEncore;
    }

    private void RefreshEncore()
    {
        int momentum = Mathf.Clamp(PlayerBattleStats.Instance.GetEncoreCharges(), 0, childs.Count);

        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].SetActive(i < momentum);
        }
    }
    
    void OnDestroy()
    {
        if (PlayerBattleStats.Instance != null)
        {
            PlayerBattleStats.Instance.onMomentumChanged -= RefreshEncore;
        }
    }
    
}
