using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerBehaviourTesting : MonoBehaviour
{
    [SerializeField] private Animator _animController;
    public Animator AnimController { get => _animController; set { _animController = value; } }

    [SerializeField] private GameObject[] _attackTriggers;
    public GameObject[] AttackTriggers { get => _attackTriggers; set { _attackTriggers = value; } }

    private int _playerNumber = 1;
    public int PlayerNumber { get => _playerNumber; set { _playerNumber = value; } }
    private float _currentHP = 0;
    public float CurrentHP => _currentHP;
    private bool _isDamageDone = false;
    public bool IsDamageDone { get => _isDamageDone; set { _isDamageDone = value; } }
    private bool _isAttacking = false;
    public bool IsAttacking { get => _isAttacking; set { _isAttacking = value; } }
    private bool _isBlocking = false;
    public bool IsBlocking { get => _isBlocking; set { _isBlocking = value; } }

    public event EventHandler OnChangeCurrentHealth;

    private CharacterController _characterController;
    private GameController _gameController;
    private Vector3 _direction;

    private string _horizontalAxis, _normalAttackButton, _heavyAttackButton, _blockButton;
    private bool _canBlock = true;
    private bool _hasInitialized = false;
    private float _doDamageValue = 0;

    private Vector2 _screenBounds;
    private float _objectWidth;

    private Vector3 impact;
    
    [SerializeField]
    private float _characterSpeed;
    [SerializeField]
    private float _timeUntilNextBlock;

    private void Start()
    {
        transform.parent = null;
        tag = "Player" + _playerNumber;

        _horizontalAxis = "Player" + _playerNumber + "_HorizontalAxis";
        _normalAttackButton = "Player" + _playerNumber + "_AButton";
        _heavyAttackButton = "Player" + _playerNumber + "_BButton";
        _blockButton = "Player" + _playerNumber + "_XButton";

        foreach (var trigger in _attackTriggers)
        {
            trigger.SetActive(false);
        }

        _characterController = GetComponent<CharacterController>();

        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        _objectWidth = GetComponent<Collider>().bounds.size.x;
    }

    void Update()
    {
        if (Input.GetButtonDown(_normalAttackButton) && !_isAttacking)
        {
            _animController.SetTrigger("FastAttack");
            _isAttacking = true;
        }
        
        if (Input.GetButtonDown(_heavyAttackButton) && !_isAttacking)
        {
            _animController.SetTrigger("HeavyAttack");
            _isAttacking = true;
        }
        
        if (Input.GetButtonDown(_blockButton) && !_isAttacking && _canBlock)
        {
            _animController.SetTrigger("IsBlocking");
            StartCoroutine(TimeTillBlock());
        }
        
        _direction = new Vector3(Input.GetAxis(_horizontalAxis), 0, 0);

        if (impact.magnitude > 0.2) _characterController.Move(impact * Time.deltaTime);
        
        impact = Vector3.Lerp(impact, Vector3.zero, 2 * Time.deltaTime);
    }

    private void TriggerFightEnd()
    {
        _animController.SetTrigger("HasFainted");
    }

    private IEnumerator TimeTillBlock()
    {
        _canBlock = false;
        yield return new WaitForSeconds(_timeUntilNextBlock);
        _canBlock = true;
    }

    public void TakeDamage(float damage, float force, Vector3 direction)
    {
        if (!_isBlocking)
        {
            if (_currentHP > 0)
                _animController.SetTrigger("Hit");
            else
                TriggerFightEnd();

            if (_currentHP < 0) _currentHP = 0;

            OnChangeCurrentHealth?.Invoke(this, EventArgs.Empty);

            impact += direction * force;
        }
    }

    private void DoDamage(GameObject go, Vector3 direction)
    {
        if (!_isDamageDone)
        {
            go?.GetComponent<PlayerBehaviour>()?.TakeDamage(_doDamageValue, 40, direction);
            _isDamageDone = true;
        }
    }

    private void FixedUpdate()
    {
        if (!_isAttacking && !_isBlocking)
            WalkAndIdle();
    }

    private void WalkAndIdle()
    {
        Vector3 speed = _direction * _characterSpeed * Time.deltaTime;

        if (!_characterController.isGrounded)
            speed -= transform.up * 9.81f * Time.deltaTime;

        _characterController.Move(speed);

        if (_playerNumber % 2 == 0)
            _animController.SetFloat("WalkSpeed", -_direction.x);
        else
            _animController.SetFloat("WalkSpeed", _direction.x);

        Clamp();
    }

    private void Clamp()
    {
        Vector3 viewPos = transform.position;

        viewPos = new Vector3
        (
            FloatClamp(viewPos.x, _screenBounds.x + _objectWidth, -(_screenBounds.x + _objectWidth)),
            viewPos.y,
            viewPos.z
        );

        transform.position = viewPos;
    }

    private float FloatClamp(float value, float min, float max)
    {
        return (value <= min) ? min : (value >= max) ? max : value;
    }
}