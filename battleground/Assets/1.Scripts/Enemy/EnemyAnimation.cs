using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour
{
    [HideInInspector] public Animator anim;
    [HideInInspector] public float currentAimingAngleGap;
    [HideInInspector] public Transform gunMuzzle;
    [HideInInspector] public float angularSpeed; //Enemy가 너무 조준을 잘하면 어려우므로 조정

    private StateController controller;
    private NavMeshAgent nav;
    private bool pendingAim; //조준을 기다리는 시간 (자연스러운 IK 회전을 위함)
    private Transform hips, spine; //bone transform
    private Vector3 initialRootRotation;
    private Vector3 initialHipsRotation;
    private Vector3 initialSpineRotation;
    private Vector3 lastRotation;
    private float timeCountAim, timeCountGuard; //원하는 회전값으로 돌리기 위한 타임카운트 (현재 rotation값이랑 원하는 rotation값을 얻기위해)
    private readonly float turnSpeed = 25f; //strafing turn speed

    private void Awake()
    {
        //setup
        controller = GetComponent<StateController>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nav.updateRotation = false; //회전은 우리가 제어한다.

        hips = anim.GetBoneTransform(HumanBodyBones.Hips);
        spine = anim.GetBoneTransform(HumanBodyBones.Spine);

        initialRootRotation = (hips.parent == transform) ? Vector3.zero : hips.parent.localEulerAngles;
        initialHipsRotation = hips.localEulerAngles;
        initialSpineRotation = spine.localEulerAngles;

        anim.SetTrigger(FC.AnimatorKey.ChangeWeapon);
        anim.SetInteger(FC.AnimatorKey.Weapon, (int)System.Enum.Parse(typeof(WeaponType), controller.classStats.WeaponType)); //무기타입을 얻어온다.

        //총구 셋팅
        foreach (Transform child in anim.GetBoneTransform(HumanBodyBones.RightHand))
        {
            gunMuzzle = child.Find("Muzzle");
            if (gunMuzzle != null)
            {
                break;
            }
        }
        //가끔 장착하는 무기에 rigidbody가 있는 경우가 있다. 그럴땐 꺼준다.
        foreach (Rigidbody member in GetComponentsInChildren<Rigidbody>())
        {
            member.isKinematic = true;
        }
    }
}
