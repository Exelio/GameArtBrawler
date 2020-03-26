using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    private TextMeshPro _textMesh;
    private float _lifeTimer = 1f;
    private Color _textColor;

    private const float LIFE_TIMER_MAX = 1f;

    private static int SORTING_ORDER;

    private void Start()
    {
        _textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void SetUp(int damageAmount)
    {
        _textMesh.text = damageAmount.ToString();
        _textColor = _textMesh.color;

        _lifeTimer = 10f;

        SORTING_ORDER++;
        _textMesh.sortingOrder = SORTING_ORDER;
    }

    private void Update()
    {

        _lifeTimer -= Time.deltaTime;

        if (_lifeTimer > LIFE_TIMER_MAX / 2)
        {
            transform.localScale += Vector3.one * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * Time.deltaTime;
        }

        if (_lifeTimer < 0)
        {
            float dissapearSpeed = 3f;
            _textColor.a -= dissapearSpeed * Time.deltaTime;
            _textMesh.color = _textColor;

            if (_textColor.a < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
