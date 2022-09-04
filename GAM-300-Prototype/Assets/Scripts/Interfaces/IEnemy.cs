using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void IdleBehaviour();
    public void PatrolBehaviour();
    public void ChaseBehaviour();

    public void AttackBehaviour();

    public void Death();
}
