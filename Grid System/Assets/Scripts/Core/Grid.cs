using UnityEngine;

namespace GridSystem.Core
{
    public class Grid
    {
        public Vector3 Position { get; private set; }
        public Building BuiltObject { get; set; }

        public Grid(Vector3 position)
        {
            Position = position;
            BuiltObject = null;
        }
    }
}
