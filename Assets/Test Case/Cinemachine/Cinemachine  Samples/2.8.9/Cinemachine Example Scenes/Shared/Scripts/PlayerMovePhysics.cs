﻿using System;
using UnityEngine;

public class PlayerMovePhysics : MonoBehaviour
{
    public float speed = 5;
    public bool worldDirection = true;
    public bool rotatePlayer = true;

    public Action spaceAction;
    public Action enterAction;
    private Rigidbody rb;

    private void Start() => rb = GetComponent<Rigidbody>();

    private void OnEnable() => transform.position += new Vector3(10, 0, 0);

    private void FixedUpdate()
    {
        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //input = Vector3.forward;
        if (input.magnitude > 0)
        {
            var fwd = worldDirection
                ? Vector3.forward : transform.position - Camera.main.transform.position;
            fwd.y = 0;
            fwd = fwd.normalized;
            if (fwd.magnitude > 0.001f)
            {
                var inputFrame = Quaternion.LookRotation(fwd, Vector3.up);
                input = inputFrame * input;
                if (input.magnitude > 0.001f)
                {
                    rb.AddForce(speed * input);
                    if (rotatePlayer)
                    {
                        transform.rotation = Quaternion.LookRotation(input.normalized, Vector3.up);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && spaceAction != null)
        {
            spaceAction();
        }

        if (Input.GetKeyDown(KeyCode.Return) && enterAction != null)
        {
            enterAction();
        }
    }
}
