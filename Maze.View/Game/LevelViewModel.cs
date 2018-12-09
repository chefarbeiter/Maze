﻿using Maze.Core.Objects;
using Maze.View.CanvasObjects;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Maze.View.Game
{
    public class LevelViewModel : BindableBase
    {
        private Level level;
        private Robot robot;

        public LevelViewModel(Level level)
        {
            this.level = level;
            this.level.RobotChanged += OnRobotChanged;
            this.CanvasObjects = new ObservableCollection<CanvasObject>();

            this.CalculateLevelSize();
            this.CreateWalls();
            this.CreateRobot();
        }

        public ObservableCollection<CanvasObject> CanvasObjects { get; }
        public decimal LevelWidth { get; set; }
        public decimal LevelHeight { get; set; }

        private void OnRobotChanged()
        {
            this.robot.Refresh();
        }

        private void CalculateLevelSize()
        {
            this.LevelWidth = SizeConverter.Convert(this.level.Width);
            this.LevelHeight = SizeConverter.Convert(this.level.Height);
        }

        private void CreateWalls()
        {
            var walls = this.level.Walls;
            for (var x = 0; x < this.level.Width; x++)
            {
                walls.Add(new Wall
                (
                    new Point(x, 0),
                    new Point(x + 1, 0)
                ));

                walls.Add(new Wall
                (
                    new Point(x, this.level.Height),
                    new Point(x + 1, this.level.Height)
                ));
            }

            for (var y = 0; y < this.level.Height; y++)
            {
                walls.Add(new Wall
                (
                    new Point(0, y),
                    new Point(0, y + 1)
                ));

                walls.Add(new Wall
                (
                    new Point(this.level.Width, y),
                    new Point(this.level.Width, y + 1)
                ));
            }

            walls.Remove(this.level.Exit);

            this.CanvasObjects.AddRange(walls.Select(w => new Line(w)));
        }

        private void CreateRobot()
        {
            this.robot = new Robot(this.level);
            this.CanvasObjects.Add(this.robot);
        }
    }
}