using UnityEngine;

public class MainAttack : Attack {

    private Boss1Controller controller;

    public MainAttack(Boss1Controller controller) {
        this.controller = controller;
    }

    protected override void doExecute() {
        controller.anim.SetBool("Running", false);
        controller.anim.SetTrigger("IdleShotting");
        controller.rangeAttackPosition.GetComponent<SpriteRenderer>().enabled = true;
        controller.StartCoroutine(updateAttack());
    }

    protected override void doAttack() {
        
    }

    protected override void doFinish() {
        controller.anim.SetTrigger("ReturnIdle");
        controller.rangeAttackPosition.GetComponent<SpriteRenderer>().enabled = false;
    }
    
}
