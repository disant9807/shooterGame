using UnityEngine;

namespace Assets.Core.Action
{
    public interface IMovedAction
    {
        void RandomMove();

        void AttackMove();

        void EscapeMove();
    }
}