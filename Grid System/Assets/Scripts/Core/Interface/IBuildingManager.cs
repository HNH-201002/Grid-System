namespace GridSystem.Core
{
    public interface IBuildingManager
    {
        public Building Build(BuildingType buildingType);
        public void Remove(Building building);
        public void Rotate(Building building, int angle);
    }

}