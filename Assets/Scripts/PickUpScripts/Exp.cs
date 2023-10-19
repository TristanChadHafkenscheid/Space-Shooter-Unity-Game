using UnityEngine;

namespace PickUps
{
    public class Exp : MonoBehaviour
    {
        [SerializeField] private int _expAmount = 1;

        public int ExpAmount
        {
            get { return _expAmount; }
        }
    }
}