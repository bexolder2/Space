using Space.Model.Enums;
using Space.Model.Modules;

namespace Space.Infrastructure.Helpers
{
    public class IntersectionHelper
    {
        public static CellType GetObjectsIntersection(Cell cell)
        {
            CellType result = CellType.Player;

            switch (cell.CellType)
            {
                case CellType.Asteroid:
                    result = CellType.PlayerAndAsteroid;
                    break;
                case CellType.Empty:
                    result = CellType.Player;
                    break;
                case CellType.Planet1:
                    result = CellType.PlayerAndPlanet1;
                    break;
                case CellType.Planet2:
                    result = CellType.PlayerAndPlanet2;
                    break;
                case CellType.Station:
                    result = CellType.PlayerAndStation;
                    break;
                default:
                    result = CellType.Player;
                    break;
            }

            return result;
        }

        public static CellType GetCellTypeWitoutPlayer(Cell cell)
        {
            CellType result = CellType.Empty;

            switch (cell.CellType)
            {
                case CellType.PlayerAndAsteroid:
                    result = CellType.Asteroid;
                    break;
                case CellType.Player:
                    result = CellType.Empty;
                    break;
                case CellType.PlayerAndPlanet1:
                    result = CellType.Planet1;
                    break;
                case CellType.PlayerAndPlanet2:
                    result = CellType.Planet2;
                    break;
                case CellType.PlayerAndStation:
                    result = CellType.Station;
                    break;
                default:
                    result = CellType.Empty;
                    break;
            }

            return result;
        }
    }
}
