using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    protected Coroutine corMove = null;
    protected Coroutine corRotate = null;
    public float moveSpeed = 1.0f;
    public float rotSpeed = 360.0f;

    protected void StopMoveCorutine()
    {
        if (corRotate != null)
        {
            StopCoroutine(corRotate);
            corRotate = null;
        }
        if (corMove != null)
        {
            StopCoroutine(corMove);
            corMove = null;
        }
    }

    public void MoveSelectUnit(Vector3 pos)
    {
        StopMoveCorutine();
        corMove = StartCoroutine(Moving(pos));
    }

    IEnumerator Moving(Vector3 pos)
    {
        Vector3 moveDir = pos - transform.position;
        float moveDist = moveDir.magnitude;
        moveDir.Normalize();

        corRotate = StartCoroutine(Rotating(moveDir));
        while(moveDist > 0.0f)
        {
            float delta = moveSpeed * Time.deltaTime;
            if (delta > moveDist) delta = moveDist;
            transform.Translate(moveDir * delta, Space.World);
            moveDist -= delta;
            yield return null;
        }
    }

    IEnumerator Rotating(Vector3 dir)
    {
        float rotAngle = Vector3.Angle(transform.forward, dir);
        float rotDir = Vector3.Dot(transform.right, dir) > 0.0f ? 1.0f : -1.0f;

        while (rotAngle > 0.0f)
        {
            float delta = rotSpeed * Time.deltaTime;
            if (delta > rotAngle) delta = rotAngle;
            transform.Rotate(Vector3.up * rotDir * delta);
            rotAngle -= delta;
            yield return null;
        }
    }
}
