using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Storage))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _lookRange = 2f;
    [SerializeField] private Transform _lookTransform;
    [SerializeField] private LayerMask _buildingLayer;
    [SerializeField] private LayerMask _harvestLayer;

    private Animator _animator;
    private Vector2 _move;
    private Matrix4x4 _matrix;
    private InputsManager _inputMapping;

    private List<IInteractableObject> _lastInteractableObjects = new();

    private static Storage _storage;
    public static Storage PlayerStorage => _storage;

    public delegate void PlayerMove();

    public static event PlayerMove AfterPlayerMove;

    private void Awake()
    {
        _inputMapping = new InputsManager();
        _storage = GetComponent<Storage>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

        Plant.OnCollectItem += OnCollectPlant;
        Wood.OnCollectItem += OnCollectTree;
        //Ore.OnCollectItem += OnCollectOre;

        transform.position = SaveSerial.PlayerPosition;
        transform.rotation = SaveSerial.PlayerRotation;
    }

    private void Update()
    {
        Move();
        LookAtObjects();
    }

    private void OnEnable()
    {
        _inputMapping.Enable();
    }

    private void OnDisable()
    {
        _inputMapping.Disable();
    }

    private void Move()
    {
        Vector3 previous = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 movement = new Vector3(_move.x, 0f, _move.y);
        Vector3 skewedMovement = _matrix.MultiplyPoint3x4(movement);

        Rotate(skewedMovement);
        transform.Translate(
            (transform.forward * skewedMovement.magnitude) * _speed * Time.deltaTime,
            Space.World
        );

        float velocity = (transform.position - previous).magnitude / Time.deltaTime;
        _animator.SetFloat(AnimationConsts.VELOCITY, velocity);
        _animator.SetFloat(AnimationConsts.VELOCITY_MULT, velocity / _speed);

        if (previous != transform.position)
        {
            SaveSerial.SavePlayerPosition(transform.position);

            AfterPlayerMove?.Invoke();
        }
    }

    private void Rotate(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            Quaternion rotation = Quaternion.Slerp(
                transform.rotation, Quaternion.LookRotation(movement), _rotationSpeed * Time.deltaTime
            );
            transform.rotation = rotation;

            SaveSerial.SavePlayerRotation(transform.rotation);
        }
    }

    private void LookAtObjects()
    {
        Ray lookRay = new Ray(_lookTransform.position, _lookTransform.forward / 2);

        List<IInteractableObject> tempItems = new();
        RaycastHit[] hits = Physics.SphereCastAll(lookRay, 1f, _lookRange, _harvestLayer);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent(out IInteractableObject item))
                {
                    if (_lastInteractableObjects.Contains(item))
                    {
                        _lastInteractableObjects.Remove(item);
                    }

                    item.ShownToItem();
                    tempItems.Add(item);
                }
            }
        }

        ClearLastInteractableObjects();
        if (tempItems.Count > 0)
        {
            foreach (var item in tempItems)
            {
                _lastInteractableObjects.Add(item);
            }
        }
    }

    private void ClearLastInteractableObjects()
    {
        if (_lastInteractableObjects.Count <= 0) return;
        foreach (var item in _lastInteractableObjects)
        {
            item.NotShownToItem();
        }

        _lastInteractableObjects.Clear();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    private void OnCollectPlant(bool startCollect, float timeCollect)
    {
        _animator.SetBool(AnimationConsts.GATHER_PLANT, startCollect);

        AnimationClip[] animationClips = _animator.runtimeAnimatorController.animationClips;
        AnimationClip animClip = animationClips.First((ac) => ac.name == AnimationConsts.ANIMATION_GATHER_PLANT_NAME);

        if (startCollect)
        {
            _animator.SetFloat(AnimationConsts.GATHER_PLANT_MULT, animClip.length / timeCollect);
        }
    }

    private void OnCollectTree(bool startCollect, float timeCollect)
    {
        _animator.SetBool(AnimationConsts.CHOP_TREE, startCollect);

        AnimationClip[] animationClips = _animator.runtimeAnimatorController.animationClips;
        AnimationClip animClip = animationClips.First((ac) => ac.name == AnimationConsts.ANIMATION_CHOP_TREE_NAME);

        if (startCollect)
        {
            _animator.SetFloat(AnimationConsts.CHOP_TREE_MULT, animClip.length / timeCollect);
        }
    }
}