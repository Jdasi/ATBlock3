using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LifeForce life { get { return life_; } }
    public PlayerInventory inventory { get; private set; }

    public CustomEvents.FloatEvent mana_percent_changed_events;
    public CustomEvents.FloatEvent mana_percent_changed_events_quiet;

    [Header("Movement Parameters")]
    [SerializeField] float speed = 6;
    [SerializeField] float turn_speed = 150;
    [SerializeField] float strafe_speed_modifier = 0.75f;
    [SerializeField] float back_speed_modifier = 0.75f;
    [SerializeField] float sprint_speed_modifier = 1.5f;

    [Header("Interaction Parameters")]
    [SerializeField] float interaction_distance = 2;

    [Space]
    [SerializeField] int max_mana = 100;
    [SerializeField] int attack_mana_cost = 5;
    [SerializeField] int mana_regen_per_second = 3;

    [Space]
    [SerializeField] int health_on_potion = 30;
    [SerializeField] int mana_on_potion = 30;

    [Header("References")]
    [SerializeField] Rigidbody rigid_body;
    [SerializeField] Transform eyes_transform;
    [SerializeField] PlayerStaff staff;
    [SerializeField] LifeForce life_;
    [SerializeField] ShakeModule shake;

    private float horizontal;
    private float vertical;

    private bool strafing;
    private bool sprinting;

    private int mana;
    private float mana_percent { get { return (float)mana / max_mana; } }
    private float mana_regen_timer;

    private float modified_speed
    {
        get { return speed * (1 + (sprinting ? sprint_speed_modifier : 0)); }
    }


    public void Damage(int _amount)
    {
        shake.Shake(0.1f, 0.1f);
    }


    void Awake()
    {
        inventory = this.gameObject.AddComponent<PlayerInventory>();
        mana = max_mana;
    }


    void Update()
    {
        strafing = Input.GetMouseButton(0);
        sprinting = Input.GetKey(KeyCode.LeftShift);

        staff.Sway(vertical * modified_speed);

        HandleAttack();
        HandleInteraction();
        HandlePotionConsumption();
        HandleManaRegen();
    }


    void FixedUpdate()
    {
        HandleMovement();
    }


    void HandleAttack()
    {
        if (!staff.can_shoot || mana < attack_mana_cost || !Input.GetKey(KeyCode.E))
            return;

        mana_regen_timer = 0;
        staff.Shoot();
    }


    void HandleInteraction()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(eyes_transform.position, eyes_transform.forward,
            out hit, interaction_distance, ~LayerMask.NameToLayer("Player")))
        {
            return;
        }

        var i = hit.collider.GetComponentInParent<DungeonInteractable>();
        if (i != null)
        {
            i.Activate();
        }
    }


    void HandleMovement()
    {
        horizontal = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
        vertical = Input.GetAxis("Vertical") * Time.fixedDeltaTime;

        Vector3 move = Vector3.zero;

        if (horizontal != 0)
        {
            if (strafing)
            {
                horizontal *= strafe_speed_modifier;
                move += transform.right * modified_speed * horizontal;
            }
            else
            {
                transform.Rotate(0, horizontal * turn_speed, 0);
            }
        }

        if (vertical != 0)
        {
            if (vertical < 0)
                vertical *= back_speed_modifier;

            move += transform.forward * modified_speed * vertical;
        }

        rigid_body.MovePosition(transform.position + move);
    }


    void HandlePotionConsumption()
    {
        if (inventory.health_potions > 0 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (life.IsFullyHealed())
            {
                // Can't drink, health full.
            }
            else
            {
                inventory.RemoveHealthPotion();
                life.Heal(health_on_potion);
            }
        }

        if (inventory.mana_potions > 0 && Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (mana >= max_mana)
            {
                // Can't drink, mana full.
            }
            else
            {
                mana += mana_on_potion;

                inventory.RemoveManaPotion();
                mana_percent_changed_events.Invoke(mana_percent);
            }
        }
    }


    void HandleManaRegen()
    {
        if (mana >= max_mana)
        {
            mana_regen_timer = 0;
            return;
        }

        mana_regen_timer += Time.deltaTime;

        if (mana_regen_timer >= 1)
        {
            mana_regen_timer = 0;

            mana += mana_regen_per_second;
            mana = Mathf.Clamp(mana, 0, max_mana);

            mana_percent_changed_events_quiet.Invoke(mana_percent);
        }
    }

}
