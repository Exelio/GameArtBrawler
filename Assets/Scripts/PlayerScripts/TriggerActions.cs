using EZCameraShake;
using UnityEngine;

public class TriggerActions : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _playerBeh;

    [SerializeField] private Transform[] _instantiationPointAttackParticleEffect;
    [SerializeField] private ParticleSystem[] _attackParticleEffect;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip[] _attackSoundEffects;

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
        _playerBeh.DisableTriggers();
    }

    public void SetTriggers()
    {
        _playerBeh.EnableTriggers();
    }

    public void TriggerParticleEffect(int index)
    {
        if (_attackParticleEffect != null)
        {
            GameObject obj = Instantiate(_attackParticleEffect[index].gameObject, _instantiationPointAttackParticleEffect[index]);

            obj.transform.position = _instantiationPointAttackParticleEffect[index].transform.position;
            obj.transform.parent = null;

            Destroy(obj, _attackParticleEffect[index].main.duration);
        }
    }

    public void TriggerScreenShake(float magnitude)
    {
        CameraShaker.Instance.ShakeOnce(magnitude * 2, magnitude * 4, 0.15f, 0.5f);
    }

    public void TriggerAudio(int index)
    {
        if (_audioSource != null)
        {
            _audioSource.volume = 0.1f;
            _audioSource.PlayOneShot(_attackSoundEffects[index]);
        }
    }
}
