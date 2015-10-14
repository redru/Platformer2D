using UnityEngine;
using System.Collections;
using System;

public class EnergyBallController : MonoBehaviour, IDamageable {

    public int life = 5;

    private bool dead = false;

    int IDamageable.damage(int damageTaken) {
        life -= damageTaken;
        checkDead();

        return damageTaken;
    }

    void checkDead() {
        dead = life > 0 ? false : true;

        if (dead) {
            GameObject.Destroy(this.gameObject);
        }
    }

}
