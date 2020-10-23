using RPG.AI;
using System.Collections.Generic;

namespace RPG.Control
{
    public enum ManualActions
    {
        Attack,
        AttackTarget,
        Trade,
        Talk,
        Move,
        Open,
        PickUp,
        Inspect,
        Scan
    }

    public class Act
    {
        public Dictionary<ManualActions, SubGoal> to;
        
        public Act()
        {
            to = new Dictionary<ManualActions, SubGoal>();
            InitilizeCommands();
        }

        private void InitilizeCommands()
        {
            to.Add(ManualActions.Attack, new SubGoal("killEnemy", 1, true));
            to.Add(ManualActions.PickUp, new SubGoal("pickUp", 1, true));
            to.Add(ManualActions.Talk, new SubGoal("talk", 1, true));
            to.Add(ManualActions.Trade, new SubGoal("trade", 1, true));
            to.Add(ManualActions.AttackTarget, new SubGoal("attackMyTarget", 1, true));
            to.Add(ManualActions.Move, new SubGoal("moveTo", 1, true));

        }


        

    }

}