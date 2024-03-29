﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MainsweeperGame;
using System.Windows.Shapes;
using System.Windows;

namespace MainsweeperDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        public Mainsweeper Game;
        public const int CellSize = 35;
        public GamePage(int w, int h, int mines)
        {
            InitializeComponent();
            ButtonsWP.Height = h * CellSize;
            ButtonsWP.Width = w * CellSize;
            Game = new Mainsweeper(w, h, mines);
            UpdateField();
            DataContext = Game;

        }

        void OnCellOpen(object sender, RoutedEventArgs e)
        {
            if (Game.Attempts == 0)
            {
                Game.GenerateMap((sender as Button).Tag as MainsweeperGame.Point);
            }
            var x = (sender as Button).Tag as MainsweeperGame.Point;
            Game.OpenPoints(x);
            UpdateField();
            if (Game.IsGameOver)
            {
                GameOverGrid.Visibility = Visibility.Visible;
            }
        }

        public void UpdateField()
        {
            ButtonsWP.Children.Clear();
            for (int i = 0; i < Game.Width; i++)
            {
                for (int j = 0; j < Game.Height; j++)
                {
                    var tmpPoint = Game.GetPointByPosition(new MainsweeperGame.Point(i, j, false));
                    var tmpButton = new Button()
                    {
                        Width = CellSize,
                        Height = CellSize,
                        Tag = tmpPoint ?? new MainsweeperGame.Point(i, j, false)
                    };

                    if (tmpPoint != null)
                    {
                        if (tmpPoint.MineAround == 0 && !tmpPoint.IsMine)
                            tmpButton.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        if (tmpPoint.MineAround != 0)
                            tmpButton.Content = tmpPoint.MineAround;
                        if (tmpPoint.IsMine)
                            tmpButton.Content = "MINE";
                    }
                    tmpButton.Click += OnCellOpen;
                    ButtonsWP.Children.Add(tmpButton);
                }
            }
        }
        void OnRestartGame(object sender, RoutedEventArgs e)
        {
            GameOverGrid.Visibility = Visibility.Collapsed;
            Game.GameField = new List<MainsweeperGame.Point>();
            Game.Attempts = 0;
            UpdateField();
        }
    }
}
