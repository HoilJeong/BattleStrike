using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 현재 동작, 기본 동작, 오버라이딩 동작, 잠긴 동작, 마우스 이동값, 
/// 땅에 서있는지, GenericBehaviour를 상속받은 동작들을 업데이트 시켜준다.
/// </summary>
public class BehaviourController : MonoBehaviour
{
    private List<GenericBehaviour> behaviours; //동작들
    private List<GenericBehaviour> overrideBehaviours; //우선시 되는 동작
    private int currentBehaviour; //현재 동작 해시코드
    private int defaultBehaviour; //기본 동작 해시코드
    private int behaviourLocked; //잠긴 동작 해시코드

    //캐싱
    public Transform playerCamera;
    private Animator myAnimator;
    private Rigidbody myRigidbody;
    private ThirdPersonOrbitCam camScript;

    //
    private float h; //horizontal axis
    private float v; //vertical axis
    public float turnSmoothing = 0.06f; //카메라를 향하도록 움직일 때 회전속도
    private bool changedFOV; //달리기 동작이 카메라 시야각이 변경되었을 때 저장되었나
    public float sprintFOV = 100; //달리기 시야각
    private Vector3 lastDirection; //마지막 향했던 방향
    private bool sprint; //달리기 중인가?
    private int hFloat; //애니메이터 관련 가로축 값
    private int vFloat; //애니메이터 관련 세로축 값
    private int groundedBool; //애니메이터 지상에 있는가
    private Vector3 colExtents; //땅과의 충돌체크를 위한 충돌체 영역

    public float GetH 
    {
        get
        {
            return h;
        }
    }

    public float GetV
    {
        get
        {
            return v;
        }
    }

    public ThirdPersonOrbitCam GetCamScript
    {
        get
        {
            return camScript;
        }
    }

    public Rigidbody GetRigidbody
    {
        get
        {
            return myRigidbody;
        }
    }

    public Animator GetAnimator
    {
        get
        {
            return myAnimator;
        }
    }

    public int GetDefaultBehaviour
    {
        get
        {
            return defaultBehaviour;
        }
    }
}

public abstract class GenericBehaviour : MonoBehaviour
{
    protected int speedFloat;
    protected BehaviourController behaviourController;
    protected int behaviourCode;
    protected bool canSprint;

    private void Awake()
    {
        this.behaviourController = GetComponent<BehaviourController>();
        speedFloat = Animator.StringToHash(FC.AnimatorKey.Speed);
        canSprint = true;
        //동작 타입을 해시코드로 가지고 있다가 추후에 구별용으로 사용
        behaviourCode = this.GetType().GetHashCode();
    }

    public int GetBehaviourCode
    {
        get
        {
            return behaviourCode;
        }
    }

    public bool AllowSprint
    {
        get
        {
            return canSprint;
        }
    }

    public virtual void LocalLateUpdate()
    {

    }

    public virtual void LocalFixedUpdate()
    {
    
    }

    public virtual void OnOverride()
    {

    }
}

