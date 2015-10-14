using UnityEngine;
using System.Collections;
using System;

public class Boss1Controller : MonoBehaviour, IDamageable
{

    public int life = 100;
    public float speed = 5f;
    public float jumpForce = 50f;
    public int attackPower = 25;
    public Transform attackPoint;

    private bool dead = false;

	void Update () {
        checkDeath();
    }

    void FixedUpdate() {

    }

    public int damage(int damageTaken) {
        life -= damageTaken;

        return damageTaken;
    }

    public void checkDeath() {
        if (life <= 0) {
            dead = true;
            GameObject.Destroy(this.gameObject);
        }
    }

}
