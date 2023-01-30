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


    public void SetValueToObject(int _entryFee, int _poolId, Dictionary<string, Prizevalues> prize, Dictionary<string, Dictionary<string, string>> leader, int _prizePool, int _slotsFilled, int _totalSlots ,string _poolType)
    {
        pooltype.text = _poolType;
        PoolItems mprefabObj = PoolManager.Instance.GetPoolObject("PoolType");
        mprefabObj.transform.SetParent(parent);
        mprefabObj.gameObject.SetActive(true);
        mprefabObj.name = _poolType;
        mprefabObj.GetComponent<MatchPoolType>().SetValueToPoolObject(_entryFee, _poolId, prize,leader, _prizePool, _slotsFilled, _totalSlots, _poolType);
    }
}
