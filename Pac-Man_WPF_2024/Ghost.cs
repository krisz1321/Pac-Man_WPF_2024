using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Threading;

namespace Pac_Man_WPF_2024
{
    internal class Ghost
    {
        public int Id { get; set; }
        public Brush Color { get; set; }
        public Brush DefaultColor { get; set; }
        public int Speed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int[,] SpawnCoordinates { get; set; }
        public bool Eatable { get; set; }

        private Random random;
        private int lastDirection; // 0: up, 1: down, 2: left, 3: right

        public Ghost(int spawnX, int spawnY, Brush color, int speed)
        {
            X = spawnX;
            Y = spawnY;
            Color = color;
            Speed = speed;
            SpawnCoordinates = new int[,] { { spawnX, spawnY } };
            Eatable = false;
            DefaultColor = color;
            random = new Random();
            lastDirection = random.Next(4); // Initial random direction

        }

        public void Move(int[,] map)
        {
            // Check if the ghost is at an intersection
            if (IsAtIntersection(X, Y, map))
            {
                // At an intersection, pick a new direction from the available ones
                int newDirection = GetSmartDirection(map);
                MoveInDirection(newDirection);
                lastDirection = newDirection; // Update the direction
            }
            else if (CanMoveInDirection(X, Y, lastDirection, map))
            {
                // Continue moving in the same direction if possible
                MoveInDirection(lastDirection);
            }
            else
            {
                // If can't move in the same direction, pick a new direction
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

        private bool IsAtIntersection(int x, int y, int[,] map)
        {
            // Check if the current position is an intersection
            int possibleDirections = 0;
            if (CanMoveInDirection(x, y, 0, map)) possibleDirections++; // Up
            if (CanMoveInDirection(x, y, 1, map)) possibleDirections++; // Down
            if (CanMoveInDirection(x, y, 2, map)) possibleDirections++; // Left
            if (CanMoveInDirection(x, y, 3, map)) possibleDirections++; // Right

            // An intersection is where more than two directions are possible
            return possibleDirections > 2;
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
