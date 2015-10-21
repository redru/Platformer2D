using UnityEngine;
using System.Collections;

public abstract class Attack {

    float attackDuration = 0f;
    float attackDurationCounter = 0f;
    float attackInterval = 0f;
    float attackIntervalCounter = 0f;

    bool executing = false;
    bool readyAttack = false;

    public void executeAttack(float attackDuration, float attackInterval) {
        if (!executing) {
            executing = true;
            this.attackDuration = attackDuration;
            this.attackInterval = attackInterval;
            doExecute();
        }
    }

    protected IEnumerator updateAttack() {
        while (attackDurationCounter <= attackDuration) {
            attackDurationCounter += Time.deltaTime;
            attackIntervalCounter += Time.deltaTime;

            if (attackIntervalCounter >= attackInterval) {
                doAttack();
                readyAttack = true;
            }

            yield return null;
        }

        finishAttack();        
    }

    private void finishAttack() {
        reset();
        doFinish();
    }

    private void reset() {
        executing = false;
        readyAttack = false;
        attackDuration = 0f;
        attackDurationCounter = 0f;
        attackInterval = 0f;
        attackIntervalCounter = 0f;
    }

    public bool isReadyAttack() {
        return readyAttack;
    }

    public void consumeReadyAttack() {
        readyAttack = false;
        attackIntervalCounter = 0f;
    }

    public bool isExecuting() {
        return executing;
    }

    protected abstract void doExecute();

    protected abstract void doAttack();

    protected abstract void doFinish();

}
