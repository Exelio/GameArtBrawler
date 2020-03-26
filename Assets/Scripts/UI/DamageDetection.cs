using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetection : MonoBehaviour
{
    
    [SerializeField] private Transform _damagePopUpPreFab;
    public void HitNumber()
    {
        
       Transform damagePopUpTransform = Instantiate(_damagePopUpPreFab, Vector3.zero, Quaternion.identity);
       DamagePopUp damagePopUp = _damagePopUpPreFab.GetComponent<DamagePopUp>();
       damagePopUp.SetUp(300);
    }

    private IEnumerator LifeTime(GameObject newGameOject)
    {
        yield return new WaitForSeconds(1);
        Destroy(newGameOject);
    }
}
