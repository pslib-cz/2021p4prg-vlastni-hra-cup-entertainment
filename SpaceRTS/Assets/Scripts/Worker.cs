using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour, IUnit
{

    private enum State
    {
        Idle,
        Moving,
        Animating,
    }

    private const float speed = 30f;

    private Vector3 targetPosition;
    private float stopDistance;
    private Action onArrivedAtPosition;
    private State state;


    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                
                break;
            case State.Moving:
                HandleMovement();
                break;
            case State.Animating:
                break;
        }
    }

    private void HandleMovement()
    {
        if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
        {
            Vector3 moveDir = (targetPosition - transform.position).normalized;

            float distanceBefore = Vector3.Distance(transform.position, targetPosition);
            transform.position = transform.position + moveDir * speed * Time.deltaTime;
        }
        else
        {
            // Arrived
            if (onArrivedAtPosition != null)
            {
                Action tmpAction = onArrivedAtPosition;
                onArrivedAtPosition = null;
                state = State.Idle;
                tmpAction();
            }
        }
    }


    public void SetTargetPosition(Vector3 targetPosition)
    {
        targetPosition.z = 0f;
        this.targetPosition = targetPosition;
    }

    public bool IsIdle()
    {
        return state == State.Idle;
    }

    public void MoveTo(Vector3 position, float stopDistance, Action onArrivedAtPosition)
    {
        SetTargetPosition(position);
        this.stopDistance = stopDistance;
        this.onArrivedAtPosition = onArrivedAtPosition;
        state = State.Moving;
    }

    public void PlayAnimationMine(Vector3 lookAtPosition, Action onAnimationCompleted)
    {
        state = State.Animating;

        if (onAnimationCompleted != null)
        {
            Action tmpAction = onAnimationCompleted;
            onAnimationCompleted = null;
            state = State.Idle;
            tmpAction();
        }
    }

}