using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyEvent : UnityEngine.Events.UnityEvent { }


[System.Serializable]
public class MyStringEvent : UnityEngine.Events.UnityEvent<string> { }

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Fighter : MonoBehaviour
{

    protected static Vector2 LimitsY = new Vector2(-0.172f, -0.821f);

    [SerializeField]
    protected int _lives = 3;
    [SerializeField]
    protected MyEvent whenDie;
    [SerializeField]
    protected MyStringEvent whenStaminaChange;
    [SerializeField]
    protected MyStringEvent whenLivesChange;
    [SerializeField]
    protected int _stamina = 100;
    [SerializeField]
    protected float verticalSpeed;
    [SerializeField]
    protected float horizontalSpeed;
    [SerializeField]
    protected Transform leftPunch;
    [SerializeField]
    protected Transform rightPunch;
    [SerializeField]
    protected float punchRadius = 0.1f;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator anim;

    protected bool isGuard = false;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        whenStaminaChange.Invoke(_stamina.ToString());
        whenLivesChange.Invoke(_lives.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (leftPunch == null || rightPunch == null)
            return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(leftPunch.position, punchRadius);
        Gizmos.DrawWireSphere(rightPunch.position, punchRadius);
    }

    protected IEnumerator Punch()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Punch")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("GetPunch")
            && !anim.GetCurrentAnimatorStateInfo(0).IsName("Guard"))
        {
            anim.SetTrigger("SendPunch");
            yield return new WaitForSeconds(0.8f);

            Vector2 punchPosition = sr.flipX ? leftPunch.position : rightPunch.position;

            var ob = Physics2D.CircleCast(punchPosition, punchRadius, Vector2.up);
            if (ob.collider != null && ob.collider.gameObject != gameObject)
            {
                ob.collider.SendMessage("GetPunch");
            }
        }
    }

    void GetPunch()
    {
        if (isGuard)
        {
            stamina -= 10;
            return;
        }
        anim.SetTrigger("GetPunch");
        lives--;
        if (_lives <= 0)
            whenDie.Invoke();
    }

    protected int lives
    {
        get { return _lives; }
        set
        {
            _lives = value;
            whenLivesChange.Invoke(_lives.ToString());
        }
    }

    protected int stamina
    {
        get { return _stamina; }
        set
        {
            _stamina = value;
            whenStaminaChange.Invoke(_stamina.ToString());
        }
    }

    public void AutoDestoy()
    {
        Destroy(gameObject);
    }
}
