using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLib
{
    public static void CKMove(this CharacterController cc,
        Vector3 direction,
        CharacterStat stat)
    {
        Transform t = cc.transform;

        Vector3 deltaMove = Vector3.zero;
        Vector3 moveDir = direction;
        moveDir.y = 0.0f;
        if (moveDir != Vector3.zero)
        {
            t.rotation = Quaternion.RotateTowards(
                t.rotation,
                Quaternion.LookRotation(moveDir),
                stat.TurnSpeed * Time.deltaTime);
        }

        deltaMove = direction * stat.MoveSpeed * Time.deltaTime;
        if (stat.gameObject.tag == "Player")
            if (stat.gameObject.GetComponent<FSMManager>().Walk)
                deltaMove = deltaMove * 0.3f;
        deltaMove += Physics.gravity * Time.deltaTime;
        cc.Move(deltaMove);
    }

    public static bool DetectCharacter(Camera sight, CharacterController cc)
    {
        if (cc)
        {
            Plane[] ps = GeometryUtility.CalculateFrustumPlanes(sight);
            return GeometryUtility.TestPlanesAABB(ps, cc.bounds);
        }
        return false;
    }

    public static void RotateFromTo(Transform from, Transform to)
    {
        Vector3 moveDir = to.position - from.position;
        moveDir.y = 0.0f;
        if (moveDir != Vector3.zero)
        {
            from.rotation = Quaternion.RotateTowards(
                from.rotation,
                Quaternion.LookRotation(moveDir),
                from.GetComponent<CharacterStat>().TurnSpeed);
        }
    }

    public static void RotateFromTo(Transform from, Vector3 to)
    {
        Vector3 moveDir = to - from.position;
        moveDir.y = 0.0f;
        if (moveDir != Vector3.zero)
        {
            from.rotation = Quaternion.RotateTowards(
                from.rotation,
                Quaternion.LookRotation(moveDir),
                from.GetComponent<CharacterStat>().TurnSpeed);
        }
    }
}