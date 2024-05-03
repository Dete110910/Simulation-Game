using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Fighter
{
    Vector2 control;
    void Update()
    {
        control = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (control.x != 0)
            sr.flipX = control.x < 0;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(Punch());
        }

        isGuard = Input.GetKey(KeyCode.X);
        anim.SetBool("IsGuard", isGuard);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Punch")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("GetPunch")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("Guard"))
        {
            anim.SetBool("IsWalking", control.magnitude != 0);
            rb.velocity = new Vector2(control.x * horizontalSpeed, control.y * verticalSpeed);

            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, LimitsY.y, LimitsY.x), transform.position.z);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }


    }
}
