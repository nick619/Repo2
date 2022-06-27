using GameFile.calsses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace GameFile
{

    public sealed partial class MainPage : Page
    {
        GameDriver _board;
        private bool _IsGameLoaded { get; }
        public MainPage()
        {
            this.InitializeComponent();
            _board = new GameDriver(CanvasPlayingArea, btnStartNewgame, GameOverContentDialog, YouWinContentDialog, btnPauseGame, btnResumeGame, btnSaveFile, btnLoadFile, PauseContentDialog, _IsGameLoaded);
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }
        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Up:
                    _board.MovePlayerUp();
                    break;
                case VirtualKey.Down:
                    _board.MovePlayerDown();
                    break;
                case VirtualKey.Left:
                    _board.MovePlayerLeft();
                    break;
                case VirtualKey.Right:
                    _board.MovePlayerRight();
                    break;
            }
        }
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            FreezeGoodie();
            UnFreezeGoodie();
            _board.NewGame();
            

        }

        private void UnFreezeGoodie()
        {
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void FreezeGoodie()
        {
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
        }

        private void PauseGameButton_Click(object sender, RoutedEventArgs e)
        {
            _board.IsPaused = true;
            _board.StopTimer();
            FreezeGoodie();
            SetButtonsToPauseState();
        }
        private void ResumeGame_Click(object sender, RoutedEventArgs e)
        {
            UnFreezeGoodie();
            _board.ResumeGame();
            btnResumeGame.Visibility = Visibility.Collapsed;
            btnPauseGame.Visibility = Visibility.Visible;
        }

        private void SetButtonsToPauseState()
        {
            btnStartNewgame.Visibility = Visibility.Visible;
            btnResumeGame.Visibility = Visibility.Visible;
            btnPauseGame.Visibility = Visibility.Collapsed;
            btnSaveFile.Visibility = Visibility.Visible;
            btnLoadFile.Visibility = Visibility.Visible;
        }
        private void SetButtonsToNewGameState()
        {
            btnResumeGame.Visibility = Visibility.Collapsed;
            btnPauseGame.Visibility = Visibility.Collapsed;
            btnLoadFile.Visibility = Visibility.Visible;
            btnSaveFile.Visibility = Visibility.Collapsed;
        }
        private void SetButtonsToPlayState()
        {
            btnStartNewgame.Visibility = Visibility.Collapsed;
            btnResumeGame.Visibility = Visibility.Collapsed;
            btnPauseGame.Visibility = Visibility.Visible;
            btnLoadFile.Visibility = Visibility.Visible;
        }
        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            _board.SaveFile();
        }
        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            _board.LoadFile();
            FreezeGoodie();
            UnFreezeGoodie();
        }
    }
}
