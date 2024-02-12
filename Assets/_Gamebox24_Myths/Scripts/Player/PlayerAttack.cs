using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float _damageAfterTime = .4f;
    [SerializeField] private int _damage;
    [SerializeField] private AttackArea _attackArea;

    private Animator _animator;

    private bool _attacked = false;
    private bool _pointerCheck = false;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _pointerCheck = EventSystem.current.IsPointerOverGameObject();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.canceled) return;

        Attack();
    }

    private async void Attack()
    {
        if (_attacked) return;
        if (_pointerCheck) return;

        _pointerCheck = false;
        _attacked = true;
        _animator.SetTrigger(AnimationConsts.ANIMATION_SIMPLE_ATTACK);
        await Hit();
        _attacked = false;
    }

    private async UniTask Hit()
    {
        float time = GetAnimationTime();
        await UniTask.WaitForSeconds(_damageAfterTime);
        foreach (IDamagable enemy in _attackArea.DamageablesInRange)
        {
            enemy.Damage(_damage);
        }

        await UniTask.WaitForSeconds(time - _damageAfterTime);
    }

    private float GetAnimationTime()
    {
        AnimationClip[] animationClips = _animator.runtimeAnimatorController.animationClips;
        AnimationClip animClip = animationClips.First((ac) => ac.name == AnimationConsts.ANIMATION_SIMPLE_ATTACK);

        return animClip.length;
    }
}