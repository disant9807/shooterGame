using UnityEngine;

namespace Assets.Core.Action
{
    public interface IFindAction
    {
        void Find(); // Просто поиск

        void FindTarget(Vector3 target, float range); //Поиск около области target - область, возле которой будет идти поиск, range - как далеко будет идти поиск возле точки

        void FindForward(Vector3 target, float range); // Поиск по направлению

        void EndFind(); //конец поиска

        bool CheckTarget();

    }
}
