using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Pac_Man_WPF_2024
{
    internal class Ghost
    {
        public int Id { get; set; }
        public Brush Color { get; set; }
        public int Speed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int[,] SpawnCoordinates { get; set; }

        private Random random;
        private int lastDirection; // 0: up, 1: down, 2: left, 3: right

        public Ghost(int spawnX, int spawnY, Brush color, int speed)
        {
            X = spawnX;
            Y = spawnY;
            Color = color;
            Speed = speed;
            SpawnCoordinates = new int[,] { { spawnX, spawnY } };

            random = new Random();
            lastDirection = random.Next(4); // Initial random direction
        }

        public void Move(int[,] map)
        {
            // Check if the ghost can continue moving in the same direction
            if (CanMoveInDirection(X, Y, lastDirection, map))
            {
                MoveInDirection(lastDirection);
            }
            else
            {
                // At a junction, pick a new direction from the available ones
                int newDirection = GetSmartDirection(map);
                MoveInDirection(newDirection);
                lastDirection = newDirection; // Update the direction
            }
        }

        private void MoveInDirection(int direction)
        {
            switch (direction)
            {
                case 0: // Move Up
                    Y--;
                    break;
                case 1: // Move Down
                    Y++;
                    break;
                case 2: // Move Left
                    X--;
                    break;
                case 3: // Move Right
                    X++;
                    break;
            }
        }

        private bool CanMoveInDirection(int x, int y, int direction, int[,] map)
        {
            switch (direction)
            {
                case 0: // Up
                    return CanMove(x, y - 1, map);
                case 1: // Down
                    return CanMove(x, y + 1, map);
                case 2: // Left
                    return CanMove(x - 1, y, map);
                case 3: // Right
                    return CanMove(x + 1, y, map);
                default:
                    return false;
            }
        }

        private bool CanMove(int x, int y, int[,] map)
        {
            // Check if the position is valid and not a wall (assuming 1 represents a wall)
            return x >= 0 && y >= 0 && x < map.GetLength(1) && y < map.GetLength(0) && map[y, x] != 1;
        }

        // This method checks for available directions and makes a smarter choice
        private int GetSmartDirection(int[,] map)
        {
            // Find all possible directions the ghost can move
            List<int> availableDirections = new List<int>();

            // Add the possible directions to the list
            if (CanMoveInDirection(X, Y, 0, map)) availableDirections.Add(0); // Up
            if (CanMoveInDirection(X, Y, 1, map)) availableDirections.Add(1); // Down
            if (CanMoveInDirection(X, Y, 2, map)) availableDirections.Add(2); // Left
            if (CanMoveInDirection(X, Y, 3, map)) availableDirections.Add(3); // Right

            // If there's more than one available direction, we pick one randomly
            if (availableDirections.Count > 1)
            {
                // If there are multiple options, choose one randomly
                return availableDirections[random.Next(availableDirections.Count)];
            }
            else if (availableDirections.Count == 1)
            {
                // If there's only one direction, continue that way
                return availableDirections[0];
            }

            // If no direction is available, just return the last direction to avoid freezing
            return lastDirection;
        }
    }
}