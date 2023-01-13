using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static GameController;

public class MatchesPool : MonoBehaviour
{

    public GameObject prefab;
    public Transform parent;
    public TMP_Text pooltype;


    public void SetValueToObject(int _entryFee, int _poolId, Dictionary<string, Prizevalues> prize, int _prizePool, int _slotsFilled, int _totalSlots ,string _poolType)
    {
        pooltype.text = _poolType;
        GameObject mPoolPrizePrefab = Instantiate(prefab, parent);
        mPoolPrizePrefab.name = _poolType;
        mPoolPrizePrefab.GetComponent<MatchPoolType>().SetValueToPoolObject(_entryFee, _poolId,prize,_prizePool, _slotsFilled, _totalSlots);
    }
}
