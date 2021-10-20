using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;

    public int finishAmount;
    public int currentAmount;

    public bool IsFinished()
    {
        return (currentAmount >= finishAmount);
    }

    public void EnemyKilled()
    {
        if(goalType == GoalType.Kill)
        {
            currentAmount++;
        }
    }
}

public enum GoalType
{
    Kill,
    Plot
}