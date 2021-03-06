using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace EpicTMR
{
    //TODO: Add automatic scaling
    internal class MyVisualTimer
    {
        #region Constructor

        public MyVisualTimer(string name, double cooldown)
        {
            _name = name;
            _baseCooldown = cooldown;
            _currentCooldown = cooldown;
            Initialize();
            UpdateLabel();
            ScaleContainer();
        }

        private void Initialize()
        {
            InitializeContainer();
            InitializeToggle();
            InitializeCooldownButton();
            InitializeTimer();
        }

        #endregion

        #region Properties

        //Variable to get a baseline for scales
        private readonly string _name;
        private double _baseCooldown;
        private double _currentCooldown;
        private string _timerText;
        private bool _isRunning;
        private int _runTime;

        private Panel _myTimerContainer;
        private CheckBox _timerToggle;
        private Button _cooldownButton;
        private Timer _timer;

        private string _cooldownFormatted;

        #endregion

        #region Methods

        internal void ChangeCooldown(double cd)
        {
            _baseCooldown = cd;
            if (!_timer.Enabled) _currentCooldown = cd;
            UpdateLabel();
        }

        #region UI Methods

        public Panel GetPanel()
        {
            return _myTimerContainer;
        }

        #endregion

        #endregion

        //If you can see it, it's here.

        #region User Interface, UI

        //Handles any visual updates that are made as the timer is running

        #region UI updates

        private void UpdateLabel()
        {
            var t = TimeSpan.FromSeconds(_currentCooldown);

            _cooldownFormatted = 
                (t.Hours > 0 ? $"{t.Hours:##h}{t.Minutes:00m}" : 
                t.Minutes > 0 ? $"{t.Minutes:##m}" : "")
                + $"{t.Seconds:00s}";

            _timerText = $"{_name}: {_cooldownFormatted}";

            _cooldownButton.Text = _timerText;

            UpdateLabelProgressColor();
        }

        private void UpdateLabelProgressColor()
        { 
            var forceDefaultColor = _runTime >= 5 && _isRunning;
            if (_baseCooldown - _currentCooldown <= 5 && !forceDefaultColor) _cooldownButton.BackColor = Color.LightGreen;
            else if (_currentCooldown <= 5 && !forceDefaultColor)
            {
                var p = _currentCooldown / 5;
                //ARGB yellow = FFFFFF00, red FFFF0000. Common -> FFFF  00 -> 255, 255, ,255
                _cooldownButton.BackColor = Color.FromArgb(255, 255, (int)(p * 255), 0);
                /*
                (int)(Color.Yellow.R * p + Color.Red.R * (1 - p)), //red
                (int)(Color.Yellow.G * p + Color.Red.G * (1 - p)), //green
                (int)(Color.Yellow.B * p + Color.Red.B * (1 - p))); //blue*/
            }
            else _cooldownButton.ResetBackColor();
        }

        #endregion

        //Handles things you only need to do when initializing the timer UI

        #region UI initializing

        private void InitializeContainer()
        {
            _myTimerContainer = new Panel
            {
                Location = new Point(0, 0),
                AutoSize = true,
                TabIndex = 0
            };
        }

        private void InitializeToggle()
        {
            _timerToggle = new CheckBox
            {
                Size = new Size(18, 18),
                Checked = false
            };
            _timerToggle.CheckedChanged += ToggleChanged;
            _myTimerContainer.Controls.Add(_timerToggle);
        }

        private void InitializeCooldownButton()
        {
            _cooldownButton = new Button
            {
                Location = new Point(17, 0)
            };
            _cooldownButton.Size = new Size((int) (_cooldownButton.Size.Width * 1.75), _cooldownButton.Size.Height);
            _cooldownButton.Text = _name;
            _cooldownButton.Click += OnCooldownButtonClick;
            _myTimerContainer.Controls.Add(_cooldownButton);
        }

        private void ScaleContainer()
        {
            _myTimerContainer.Size = new Size(_myTimerContainer.Size.Width, 23);
        }

        #endregion

        #endregion

        //ReStart, Stop, etc.

        #region Timer controls

        public void ReStart()
        {
            _isRunning = true;
            _timer.Enabled = true;
            _timerToggle.Checked = true;
            _currentCooldown = _baseCooldown;
            UpdateLabel();
        }

        public void Stop()
        {
            _isRunning = false;
            _timer.Enabled = false;
            _timerToggle.Checked = false;
            _currentCooldown = _baseCooldown;
            UpdateLabel();
        }

        private void ToggleChanged(object sender, EventArgs e)
        {
            if (_timerToggle.Checked) ReStart();
            else Stop();
        }

        private void OnCooldownButtonClick(object sender, EventArgs e)
        {
            ReStart();
            _runTime = 0;
        }

        #endregion

        //Handles initializing a forms timer that reduces _currentCooldown and calls for label updates
        #region Timer logics

        private void InitializeTimer()
        {
            _timer = new Timer
            {
                Interval = 1000
            };
            _timer.Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (_currentCooldown > 1)
            {
                _currentCooldown--;
                UpdateLabel();
            }
            else
            {
                PlayChime();
                ReStart();
            }

            _runTime++;
        }

        private void PlayChime()
        {
            SystemSounds.Beep.Play();
        }

        #endregion
    }
}