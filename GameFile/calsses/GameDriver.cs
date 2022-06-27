using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.Storage;
using Newtonsoft.Json;

namespace GameFile.calsses
{
    class GameDriver
    {
        Player _player;
        Canvas _canvas;
        Random _rnd = new Random();
        List<Enemy> _enemies = new List<Enemy>();
        private DispatcherTimer _tmr = new DispatcherTimer();
        public DispatcherTimer TimerOfTheGame
        {
            get { return _tmr; }
        }
        private AppBarButton _btnStartNewGame;
        private AppBarButton _btnResumeGame;
        private AppBarButton _btnPause;
        private AppBarButton _btnSaveFile;
        private AppBarButton _btnLoadFile;
        private List<SaveData> _savedDataList;

        private bool IsGameLoad = false;
        private bool IsSaved;
        private bool IsGameLose;
        private bool IsGameWin;
        public bool IsGameRunning;

        public bool isGameisLoaded { get; set; }
        public bool IsloadedMorethanOnce;
        private bool IsGameWasPlayedPreviously = false;
        public bool IsPaused;
        private int PausedTimes = 1;



        private int loadedTimes = 0;
        private int _playerTop;
        private int _playerLeft;



        ContentDialog _contentDialogLose = new ContentDialog();
        ContentDialog _contentDialogWin = new ContentDialog();
        ContentDialog _contentDialogPauseGame = new ContentDialog();

        public GameDriver(Canvas Pcanvas, AppBarButton btnStartNewGame, ContentDialog contentDialogLose, ContentDialog contentDialogWin, AppBarButton btnResumeGame, AppBarButton btnPause, AppBarButton btnSaveFile, AppBarButton btnLoadFile, ContentDialog contentDialogPauseGame, bool isGameisLoaded)
        {
            _tmr.Interval = new TimeSpan(0, 0, 0, 0, 150); //Set 150 milliseconds
            _tmr.Tick += OnTickHandler;


            this._canvas = Pcanvas;
            this._btnStartNewGame = btnStartNewGame;
            this._btnPause = btnPause;
            this._btnResumeGame = btnResumeGame;
            this._btnSaveFile = btnSaveFile;
            this._btnLoadFile = btnLoadFile;
            this.isGameisLoaded = isGameisLoaded;
            this._contentDialogLose = contentDialogLose;
            this._contentDialogWin = contentDialogWin;
            this._contentDialogPauseGame = contentDialogPauseGame;

        }

        internal void ResumeGame()
        {
            _tmr.Start();
            IsGameRunning = true;
        }

        internal void StopTimer()
        {
            _tmr.Stop();
            IsGameRunning = false;
        }

        public void NewGame()
        {
            CreateBoard();
        }
        private void OnTickHandler(object sender, object e)
        {
            ChaseAfterPlayer(_enemies);
            EnemyOnEnemy();
            PlayerTouches();
            CheckWin();
        }

        private void PlayerTouches()
        {
            //double xTop = _player.GetTop();
            //double yLeft = _player.GetLeft();
            double size = 70;
            for (int i = 0; i < _enemies.Count; i++)
            {
                if ((_player.GetTop() > _enemies[i].GetTop() - size && _player.GetTop() < _enemies[i].GetTop() + size)
                   && _player.GetLeft() > _enemies[i].GetLeft() - size && _player.GetLeft() < _enemies[i].GetLeft() + size)
                {
                    _tmr.Stop();
                    GameOverLoose();
                }
            }
        }
        private void CheckWin()
        {
            int _AmountOfAliveEnemies = 0; // start counter of enemies in arry
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].Alive)
                {
                    _AmountOfAliveEnemies++;
                }
            }
            if (_AmountOfAliveEnemies == 1)
            {
                //Less then 2 enemeys and game over
                //Game over
                GameOverWin(); //method to end game
            }
        }
        private void GameOverWin()
        {
            IsGameWin = true;
            _tmr.Stop(); IsGameRunning = false;

            YouWinMessage();
        }

        private async void YouWinMessage()
        {
            await _contentDialogWin.ShowAsync();
        }

        private void GameOverLoose()
        {
            IsGameRunning = false;
            IsGameLose = true;
            _tmr.Stop();
            ClearBoard();
            GameOverMessage();
        }

        private async void GameOverMessage()
        {
            await _contentDialogLose.ShowAsync();
        }

        private async void EnemyOnEnemy()
        {
            for (int i = 0; i < _enemies.Count; i++) //checks if enemy location is the same
            {
                for (int j = 0; j < _enemies.Count; j++)
                {
                    if (i != j && _enemies[i].GetTop() > _enemies[j].GetTop() - 70
                                && _enemies[i].GetTop() < _enemies[j].GetTop() + 70
                                && _enemies[i].GetLeft() > _enemies[j].GetLeft() - 70
                                && _enemies[i].GetLeft() < _enemies[j].GetLeft() + 70)
                    {
                        _enemies[i].Destroy();
                        _enemies.RemoveAt(i);
                    }
                }
            }
        }

        private void ChaseAfterPlayer(List<Enemy> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                int randEnemey = _rnd.Next(25);
                if (_player.GetTop() < _enemies[i].GetTop())
                {
                    if (randEnemey != 0)
                    {
                        _enemies[i].M_Up();
                    }
                }
                else if (_player.GetTop() > _enemies[i].GetTop())
                {
                    if (randEnemey != 0)
                    {
                        _enemies[i].M_Down();
                    }
                }
                if (_player.GetLeft() > _enemies[i].GetLeft())
                {
                    if (randEnemey != 0)
                    {
                        _enemies[i].M_Right();
                    }
                }
                else if (_player.GetLeft() < _enemies[i].GetLeft())
                {
                    if (randEnemey != 0)
                    {
                        _enemies[i].M_Left();
                    }
                }
            }
        }
        private void CreateBoard()
        {
            _tmr.Stop();
            ClearBoard();
            IsGameRunning = false;
            _player = new Player(_canvas);
            if (IsGameLoad == true)
            {
                CreateEnemey2();
                _player.Destroy();
                _player = new Player(_canvas);
                _tmr.Start();
                IsGameRunning = true;
                IsGameWasPlayedPreviously = true;
                IsGameLoad = false;
            }
            else
            {
                CreateEnemey();
                _tmr.Start();
                IsGameWasPlayedPreviously = true;
                IsGameRunning = true;
            }
        }

        private void CreateEnemey2()
        {
            int PlayerTop = 0;
            int PlayerLeft = 0;
            for (int i = 0; i < _savedDataList.Count; i++)
            {
                _enemies.Add(new Enemy(_canvas, _savedDataList[i]._EnemyStartTop, _savedDataList[i]._EnemyStartLeft, _savedDataList[i]._EnemyId));
                PlayerTop = _savedDataList[i]._PlayerStartTop;
                PlayerLeft = _savedDataList[i]._PlayerStartLeft;
            }
            _playerTop = PlayerTop;
            _playerLeft = PlayerLeft;
        }

        private void ClearBoard()
        {
            if (_player != null)
            {
                _player.Destroy();
            }
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i] != null)
                {
                    _enemies[i].Destroy();
                }
            }
            _enemies.Clear();
            IsGameRunning = false;
            IsGameLose = false;
            IsGameWin = false;
            loadedTimes = 0;
            IsloadedMorethanOnce = false;

        }
        internal async void LoadFile()
        {
            try
            {
                List<SaveData> _data = new List<SaveData>();
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("myconfig.json");

                string json_str = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

                _data = JsonConvert.DeserializeObject<List<SaveData>>(json_str);
                _savedDataList = _data;


                ClearBoard();
                IsGameLoad = true;
                CreateBoard();
                SetButtonsOnLoad();
                isGameisLoaded = true;
                loadedTimes++;
                if (loadedTimes > 1)
                {
                    IsloadedMorethanOnce = true;
                }
            }
            catch (Exception)
            {

                FileNotExistsMessage();
            }
        }
        internal async void SaveFile()
        {
            List<SaveData> _simpleEnemyDataList = new List<SaveData>();


            foreach (var CurrentEnemy in _enemies)
            {
                _simpleEnemyDataList.Add(new SaveData((int)CurrentEnemy.GetTop(), (int)CurrentEnemy.GetLeft(), CurrentEnemy.GetId(), (int)this._player.GetTop(), (int)this._player.GetLeft()));
            }
            try
            {
                string json = JsonConvert.SerializeObject(_simpleEnemyDataList.ToArray());

                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("myconfig.json");
                await FileIO.WriteTextAsync(file, json);
                IsSaved = true;
            }
            catch (Exception)
            {

                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("myconfig.json");

                string name = "myconfig.json";
                StorageFile manifestFile = await storageFolder.GetFileAsync(name);
                await manifestFile.DeleteAsync();
                IsSaved = false;

                string json = JsonConvert.SerializeObject(_simpleEnemyDataList.ToArray());

                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("myconfig.json");
                await FileIO.WriteTextAsync(file, json);
                IsSaved = true;

            }
        }
        internal async void PauseGame()
        {
            IsPaused = true;
            _tmr.Stop();
            if (IsPaused == true && PausedTimes == 1 && IsGameRunning)
            {
                IsGameRunning = false;
                PausedTimes++;
                await
                 _contentDialogPauseGame.ShowAsync();
            }
            else if (IsGameWasPlayedPreviously == true)
            {
                _tmr.Start();
                IsGameRunning = true;
                _contentDialogPauseGame.Hide();
                IsPaused = false;
                PausedTimes = 1;

            }
        }

        private async void FileNotExistsMessage()
        {
            MessageDialog msg = new MessageDialog("File Not Exists.");
            await msg.ShowAsync();
        }

        private void SetButtonsOnLoad()
        {
            _btnPause.Visibility = Visibility.Visible;
            _btnResumeGame.Visibility = Visibility.Collapsed;
            _btnStartNewGame.Visibility = Visibility.Visible;
            _btnSaveFile.Visibility = Visibility.Visible;
            _btnLoadFile.Visibility = Visibility.Visible;
        }

        private void CreateEnemey()
        {
            Random rnd = new Random();
            int _Startleftrandom = 0;
            int _StartTopRandom = 0;
            int _EnemyID = 0;
            for (int i = 0; i < 10; i++)
            {
                _Startleftrandom = rnd.Next(-100, 1400); //Random left
                _StartTopRandom = rnd.Next(1, 150); //Random Top
                _enemies.Add(new Enemy(_canvas, _Startleftrandom, _StartTopRandom, _EnemyID));
                _EnemyID++; //icrement the id for each new enemy
            }
        }
        public void MovePlayerUp()
        {
            _player.M_Up();
        }
        public void MovePlayerDown()
        {
            _player.M_Down();
        }
        public void MovePlayerLeft()
        {
            _player.M_Left();
        }
        public void MovePlayerRight()
        {
            _player.M_Right();
        }
        private void SetPlayButtonState()
        {

         
            _btnStartNewGame.Visibility = Visibility.Visible;
            _btnPause.Visibility = Visibility.Visible;
            _btnResumeGame.Visibility = Visibility.Collapsed;
            _btnSaveFile.Visibility = Visibility.Visible;
            _btnLoadFile.Visibility = Visibility.Visible;
        }
        private void SetButtonsOnWin()
        {
            _btnPause.Visibility = Visibility.Collapsed;
            _btnResumeGame.Visibility = Visibility.Collapsed;
            _btnStartNewGame.Visibility = Visibility.Visible;
            _btnSaveFile.Visibility = Visibility.Collapsed;
            _btnLoadFile.Visibility = Visibility.Visible;
        }
        private void SetButtonsOnLooseGame()
        {
            _btnPause.Visibility = Visibility.Collapsed;
            _btnResumeGame.Visibility = Visibility.Collapsed;
            _btnStartNewGame.Visibility = Visibility.Visible;
            _btnSaveFile.Visibility = Visibility.Collapsed;
            _btnLoadFile.Visibility = Visibility.Visible;
        }

        class SaveData
        {
            public int _EnemyStartTop { get; set; }
            public int _EnemyStartLeft { get; set; }
            public int _EnemyId { get; set; }
            public int _PlayerStartTop { get; set; }
            public int _PlayerStartLeft { get; set; }


            public SaveData(int EnemyStartTop, int EnemyStartLeft, int EnemyId, int PlayerStartTop, int PlayerStartLeft)
            {
                this._EnemyStartTop = EnemyStartTop;
                this._EnemyStartLeft = EnemyStartLeft;
                this._EnemyId = EnemyId;

                this._PlayerStartTop = PlayerStartTop;
                this._PlayerStartLeft = PlayerStartLeft;
            }
        }
    }
}
