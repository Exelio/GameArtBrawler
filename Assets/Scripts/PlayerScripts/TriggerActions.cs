using UnityEngine;

public class TriggerActions : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _playerBeh;

    [SerializeField] private Transform[] _instantiationPointAttackParticleEffect;
    [SerializeField] private ParticleSystem[] _attackParticleEffect;

    public void ResetAnimTrigger()
    {
        _playerBeh.AnimController.ResetTrigger("FastAttack");
        _playerBeh.AnimController.ResetTrigger("HeavyAttack");
        _playerBeh.AnimController.ResetTrigger("Hit");
        _playerBeh.AnimController.ResetTrigger("IsBlocking");

        _playerBeh.IsDamageDone = false;
        _playerBeh.IsAttacking = false;
        _playerBeh.IsBlocking = false;
    }

    public void SetBlocking()
    {
        _playerBeh.IsBlocking = true;
    }

    public void ResetBlocking()
    {
        _playerBeh.IsBlocking = false;
    }

    public void ResetTriggers()
    {
        foreach (var trigger in _playerBeh.AttackTriggers)
            trigger.SetActive(false);
    }

    public void SetTriggers()
    {
        foreach (var trigger in _playerBeh.AttackTriggers)
            trigger.SetActive(true);
    }

    public void TriggerParticleEffect()
    {
        if (_attackParticleEffect != null)
        {
            GameObject obj = Instantiate(_attackParticleEffect.gameObject, _instantiationPointAttackParticleEffect);

            obj.transform.position = _instantiationPointAttackParticleEffect.transform.position;
            obj.transform.parent = null;

            Destroy(obj, _attackParticleEffect.main.duration);
        }
    }

    public void TriggerAudio()
    {

    }
}
