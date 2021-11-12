using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private StateMachine _stateMachine;
    private StandingState _standing;
    private JumpingState _jumping;
    private AttackState _attacking;
    private Animator _animator;
    private SpriteRenderer _renderer;
    private Rigidbody2D _rigidbody;
    private bool _grounded;

    public StateMachine StateMachine => _stateMachine;
    public StandingState Standing => _standing;
    public JumpingState Jumping => _jumping;
    public AttackState Attacking => _attacking;
    public bool Grounded => _grounded;
    public Animator Animator => _animator;


    private void Start()
    {
        _stateMachine = new StateMachine();

        _standing = new StandingState(this, _stateMachine);
        _jumping = new JumpingState(this, _stateMachine);
        _attacking = new AttackState(this, _stateMachine);

        _stateMachine.Initialize(_standing);

        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _stateMachine.CurrentState.HandleInput();
        _stateMachine.CurrentState.LogicUpdate();
    }

    public void Move(float direction)
    {
        _renderer.flipX = (direction < 0) ? true : false;

        transform.Translate(new Vector3(direction * _speed * Time.deltaTime, 0));
    }

    public void Jump()
    {
        transform.Translate(Vector2.up * 0.2f);
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _grounded = false;
    }


    public void AnimationPlay(string stateName)
    {
        _animator.Play(stateName);
    }

    public void AnimationStop()
    {
        _animator.StopPlayback();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Ground ground))
            _grounded = true;
    }
}