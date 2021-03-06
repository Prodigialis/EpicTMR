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

        /// <summary>
        /// Creates a single MyVisualTimer with controls to ReStart and Stop the timer. Also includes GetPanel() for altering the appearance
        /// </summary>
        /// <param name="name">Name of timer</param>
        /// <param name="cooldown">Amount of time between reminders</param>
        public MyVisualTimer(string name, double cooldown)
        {
            _name = name;
            _baseCooldown = cooldown;
            _currentCooldown = cooldown;
            Initialize();
            UpdateLabel();
            ScaleContainer();
        }

        /// <summary>
        /// Calls methods to create a container, a toggle button, a reset button and the functionality for a MyVisualTimer
        /// </summary>
        private void Initialize()
        {
            InitializeContainer();
            InitializeToggle();
            InitializeCooldownButton();
            InitializeTimer();
        }
        #endregion

        #region Properties
        //Properties
        private readonly string _name;
        private double _baseCooldown;
        private double _currentCooldown;
        private bool _isRunning;
        private int _runTime;

        //Components
        //Container for _timerToggle and _cooldownButton
        private Panel _myTimerContainer;
        private CheckBox _timerToggle;
        private Button _cooldownButton;
        //Functionality to update _cooldownButton.Text
        private Timer _timer;

        //Variables for displaying time left in _cooldownButton.Text
        //Conditionally formatted _currentCooldown (hh"h":mm"m":ss"s")
        private string _cooldownFormatted;
        //Text displayed on the reset button. _name + _currentCooldown (formatted hh:mm:ss)
        private string _timerText;
        #endregion

        #region Methods
        /// <summary>
        /// Updates the timers cooldown to a new value after user altered interval or cooldown in Main
        /// </summary>
        /// <param name="cd">Value to update to</param>
        internal void ChangeCooldown(double cd)
        {
            _baseCooldown = cd;
            if (!_timer.Enabled) _currentCooldown = cd;
            UpdateLabel();
        }
        
        /// <summary>
        /// Method for getting the container panel to visually alter and position it
        /// </summary>
        /// <returns>_myTimerContainer</returns>
        public Panel GetPanel()
        {
            return _myTimerContainer;
        }
        #endregion

        #region User Interface, UI
        #region UI updates
        /// <summary>
        /// Changes the text of the reset button and then calls UpdateLabelProgressColor()
        /// </summary>
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

        /// <summary>
        /// Changes the color of the reset button depending on time left
        /// </summary>
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
        #region UI initializing
        /// <summary>
        /// Creates the container for the timers on-off toggle and reset button
        /// </summary>
        private void InitializeContainer()
        {
            _myTimerContainer = new Panel
            {
                Location = new Point(0, 0),
                AutoSize = true,
                TabIndex = 0
            };
        }

        /// <summary>
        /// Creates the timer's on-off toggle and adds it to container
        /// Adds an event handler that calls ToggleChanged()
        /// </summary>
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

        /// <summary>
        /// Creates the timer's reset button and adds it to container
        /// Adds an event handler that calls OnCooldownButtonClick()
        /// </summary>
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

        /// <summary>
        /// Changes containers height to 23
        /// </summary>
        private void ScaleContainer()
        {
            _myTimerContainer.Size = new Size(_myTimerContainer.Size.Width, 23);
        }
        #endregion
        #endregion

        #region Timer controls
        /// <summary>
        /// Resets the timer to it's default _baseCooldown and instantly starts it. Then calls UpdateLabel()
        /// </summary>
        public void ReStart()
        {
            _isRunning = true;
            _timer.Enabled = true;
            _timerToggle.Checked = true;
            _currentCooldown = _baseCooldown;
            UpdateLabel();
        }

        /// <summary>
        /// Resets the timer to it's default _baseCooldown without re-starting the timer. Then calls UpdateLabel()
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _timer.Enabled = false;
            _timerToggle.Checked = false;
            _currentCooldown = _baseCooldown;
            UpdateLabel();
        }

        /// <summary>
        /// Starts or stops the timer based on _timerToggle's state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleChanged(object sender, EventArgs e)
        {
            if (_timerToggle.Checked) ReStart();
            else Stop();
        }

        /// <summary>
        /// Calls ReStart() to restart the timer and sets _runTime back to 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCooldownButtonClick(object sender, EventArgs e)
        {
            ReStart();
            _runTime = 0;
        }
        #endregion

        #region Timer logics
        /// <summary>
        /// Creates a Windows Forms timer to handle tracking time
        /// Creates an event handler that calls OnTick()
        /// </summary>
        private void InitializeTimer()
        {
            _timer = new Timer
            {
                Interval = 1000
            };
            _timer.Tick += OnTick;
        }

        /// <summary>
        /// Reduces current cooldown by 1 second and then calls UpdateLabel()
        ///
        /// If _current cooldown would drop to 0, restarts the timer and plays an audio cue. Calls ReStart()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Plays an audio cue
        /// </summary>
        private void PlayChime()
        {
            SystemSounds.Beep.Play();
        }
        #endregion
    }
}