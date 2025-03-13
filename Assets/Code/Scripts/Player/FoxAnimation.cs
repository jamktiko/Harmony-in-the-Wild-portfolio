using System;
using System.Collections.Generic;
using UnityEngine;
public class FoxAnimation : MonoBehaviour
{
    public bool MovementRestricted => Restrictors.Count > 0;
    public List<object> Restrictors = new List<object>();
    internal float speed;
    private FoxAnimationState _currentState;
    private FoxAnimationState _requestedState;

    // Using a cached hash is more efficient than using a string
    private readonly static int _stateHash = Animator.StringToHash("State"); // int
    private readonly static int _statePreviousHash = Animator.StringToHash("PreviousState"); // int
    private readonly static int _setStateHash = Animator.StringToHash("SetState"); // trigger

    private readonly static int _vertMoveHash = Animator.StringToHash("VerticalMove"); // float
    private readonly static int _horMoveHash = Animator.StringToHash("HorizontalMove"); // float
    private readonly static int _upMoveHash = Animator.StringToHash("UpMove"); // float

    private readonly static int _jumpHash = Animator.StringToHash("Jump"); // trigger
    private readonly static int _headButtHash = Animator.StringToHash("HeadButt"); // trigger
    private readonly static int _freezingHash = Animator.StringToHash("Freezing"); // Trigger
    private readonly static int _snowDiveHash = Animator.StringToHash("SnowDive"); // Trigger
    private readonly static int _collectingFromBushHash = Animator.StringToHash("CollectingFromBush"); // Trigger
    private readonly static int _collectingFromGroundHash = Animator.StringToHash("CollectingFromGround"); // Trigger
    private readonly static int _collectingFirefliesHash = Animator.StringToHash("CollectingFireflies"); // Trigger

    private readonly static int _isGroundedHash = Animator.StringToHash("isGrounded"); // Bool
    private readonly static int _isChargingJumpHash = Animator.StringToHash("isChargingJump"); // Bool
    private readonly static int _isSittingHash = Animator.StringToHash("isSitting"); // Bool
    private readonly static int _isLayingHash = Animator.StringToHash("isLaying"); // Bool
    private readonly static int _isGlidingHash = Animator.StringToHash("isGliding"); // Bool

    // Are these used?
    private readonly static int _isJumpingHash = Animator.StringToHash("isJumping"); // Bool
    private readonly static int _goingLeftHash = Animator.StringToHash("GoingLeft"); // Bool
    private readonly static int _isSwimmingHash = Animator.StringToHash("isSwimming"); // Bool
    private readonly static int _isReadyToShake = Animator.StringToHash("isReadyToShake"); // Bool
    private readonly static int _isReadyToSwim = Animator.StringToHash("isReadyToSwim"); // Bool
    private readonly static int _isSnowDivingHash = Animator.StringToHash("isSnowDiving"); // Bool
    private readonly static int _isFreezingHash = Animator.StringToHash("isFreezing"); // Bool

    private Animator _animator;
    public Animator Animator { get => _animator != null ? _animator : GetAnimator(); }
    private List<AnimatorControllerParameter> animatorBools = new List<AnimatorControllerParameter>();

    #region Unity Callbacks
    private void Awake()
    {
        var _ = Animator; // Cache the animator
    }

    private void Start()
    {
        foreach (AnimatorControllerParameter item in _animator.parameters)
        {
            if (item.type == AnimatorControllerParameterType.Bool)
            {
                animatorBools.Add(item);
            }
        }
    }
    private void Update()
    {
        if (_currentState == _requestedState)
        {
            return;
        }
        _animator.SetInteger(_stateHash, (int)_requestedState);
        _animator.SetInteger(_statePreviousHash, (int)_currentState);
        _animator.SetTrigger(_setStateHash);
        _currentState = _requestedState;
    }
    #endregion

    private Animator GetAnimator()
    {
        _animator = GetComponent<Animator>();
        return _animator;
    }

    /// <summary>
    /// Drives animator vars. State, PreviousState and SetState
    /// </summary>
    internal FoxAnimationState State { get => _currentState; set => _requestedState = value; }

    // Locomotion(0) state specific variables
    internal float HorizontalMove { get => _animator.GetFloat(_horMoveHash); set => _animator.SetFloat(_horMoveHash, value * (Sprinting ? 2f : 1f)); }
    internal float VerticalMove { get => _animator.GetFloat(_vertMoveHash); set => _animator.SetFloat(_vertMoveHash, value * (Sprinting ? 2f : 1f)); }
    internal float UpMove { get => _animator.GetFloat(_upMoveHash); set => _animator.SetFloat(_upMoveHash, value); }
    public bool Sprinting { get; internal set; } // Multiplies the movement values
    public void SetMovement(float horizontalInput, float verticalInput)
    {
        _animator.SetFloat(_horMoveHash, horizontalInput * (Sprinting ? 2f : 1f));
        _animator.SetFloat(_vertMoveHash, verticalInput * (Sprinting ? 2f : 1f));
    }
    public void SetMovement(float horizontalInput, float verticalInput, float dampTime, float deltaTime)
    {
        _animator.SetFloat(_horMoveHash, horizontalInput * (Sprinting ? 2f : 1f), dampTime, deltaTime);
        _animator.SetFloat(_vertMoveHash, verticalInput * (Sprinting ? 2f : 1f), dampTime, deltaTime);
    }


    // Resting(2) state specific variables
    public bool Sitting { get => _animator.GetBool(_isSittingHash); internal set => _animator.SetBool(_isSittingHash, value); }
    public bool Laying { get => _animator.GetBool(_isLayingHash); internal set => _animator.SetBool(_isLayingHash, value); }

    // Actions
    internal void Jump() => _animator.SetTrigger(_jumpHash);
    internal void ChargingJump(bool v) => _animator.SetBool(_isChargingJumpHash, v);

    [Obsolete("Use Action() instead")]
    internal void HeadButt() => _animator.SetTrigger(_headButtHash);
    [Obsolete("Use Action() instead")]
    internal void Freezing() => _animator.SetTrigger(_freezingHash);
    [Obsolete("Use Action() instead")]
    internal void SnowDive() => _animator.SetTrigger(_snowDiveHash);
    [Obsolete("Use Action() instead")]
    internal void CollectFromBush() => _animator.SetTrigger(_collectingFromBushHash);
    [Obsolete("Use Action() instead")]
    internal void CollectFromGround() => _animator.SetTrigger(_collectingFromGroundHash);

    /// <summary>
    /// Triggers the animation for the given action
    /// </summary>
    /// <param name="action"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal void Action(FoxAction action)
    {
        _animator.SetTrigger(action switch
        {
            FoxAction.HeadButt => _headButtHash,
            FoxAction.Freezing => _freezingHash,
            FoxAction.SnowDive => _snowDiveHash,
            FoxAction.CollectFromBush => _collectingFromBushHash,
            FoxAction.CollectFromGround => _collectingFromGroundHash,
            FoxAction.CollectFireFlies => _collectingFirefliesHash,
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        });
    }

    // Conditions
    public bool IsGrounded { get => _animator.GetBool(_isGroundedHash); internal set => _animator.SetBool(_isGroundedHash, value); }
    public bool IsGliding { get => _animator.GetBool(_isGlidingHash); internal set => _animator.SetBool(_isGlidingHash, value); }
    public bool IsChargingJump { get => _animator.GetBool(_isChargingJumpHash); internal set => _animator.SetBool(_isChargingJumpHash, value); }
    public bool ReadyToSwim { get => _animator.GetBool(_isReadyToSwim); internal set => _animator.SetBool(_isReadyToSwim, value); }

    #region Backwards compatibility
    internal void SetBool(Parameter parameter, bool v) => _animator.SetBool(ParameterToHash(parameter), v);
    internal bool GetBool(Parameter parameter) => _animator.GetBool(ParameterToHash(parameter));
    internal void SetFloat(Parameter parameter, float value) => _animator.SetFloat(ParameterToHash(parameter), value);
    internal void SetFloat(Parameter parameter, float value, float dampTime, float deltaTime) => _animator.SetFloat(ParameterToHash(parameter), value, dampTime, deltaTime);
    internal void SetTrigger(Parameter parameter) => _animator.SetTrigger(ParameterToHash(parameter));

    [Obsolete("This is for backwards compatibility reasons and will be removed.")]
    internal void SetBool(string parameterName, bool v)
    {
        Debug.LogWarning("Using Animator with string (" + parameterName + ")");
        _animator.SetBool(parameterName, v);
    }

    private static int ParameterToHash(Parameter parameter)
    {
        return parameter switch
        {
            Parameter.state => _stateHash,
            Parameter.statePrevious => _statePreviousHash,
            Parameter.setState => _setStateHash,
            Parameter.vertMove => _vertMoveHash, // Still needed by SetFloat
            Parameter.horMove => _horMoveHash, // Still needed by SetFloat
            Parameter.upMove => _upMoveHash, // Still needed by SetFloat
            Parameter.jump => _jumpHash,
            Parameter.doHeadButt => _headButtHash,
            Parameter.freezing => _freezingHash,
            Parameter.snowDive => _snowDiveHash,
            Parameter.collectingBerry => _collectingFromBushHash,
            Parameter.collectingPinecone => _collectingFromGroundHash,
            Parameter.isGrounded => _isGroundedHash,
            Parameter.isChargingJump => _isChargingJumpHash,
            Parameter.isSitting => _isSittingHash,
            Parameter.isLaying => _isLayingHash,
            Parameter.isGliding => _isGlidingHash,
            Parameter.isJumping => _isJumpingHash,
            Parameter.isSwimming => _isSwimmingHash,
            Parameter.isReadyToSwim => _isReadyToSwim,
            Parameter.isReadyToShake => _isReadyToShake,
            Parameter.isSnowDiving => _isSnowDivingHash,
            Parameter.isFreezing => _isFreezingHash,
            Parameter.goingLeft => _goingLeftHash,
            _ => throw new NotImplementedException("ParameterToHash " + parameter.ToString()),
        };
    }

    [Obsolete("This is for backwards compatibility reasons and will be removed.")]
    public enum Parameter
    {
        state,
        statePrevious,
        setState,
        horMove,
        vertMove,
        upMove,
        jump,
        doHeadButt,
        freezing,
        snowDive,
        collectingBerry,
        collectingPinecone,
        isGrounded,
        isChargingJump,
        isSitting,
        isLaying,
        isGliding,
        isJumping,
        isSwimming,
        isReadyToSwim,
        isReadyToShake,
        isSnowDiving,
        isFreezing,
        goingLeft
    }
    #endregion

}

public enum FoxAnimationState
{
    Default = 0,
    Swimming = 1,
    Rest = 2
}