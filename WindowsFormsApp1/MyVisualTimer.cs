using System;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace EpicTMR
{
    //TODO: Add automatic scaling
    class MyVisualTimer
    {
        #region Properties
        //Variable to get a baseline for scales
        string name;
        double baseCooldown;
        double currentCooldown;
        double flashCount;

        private Panel myTimerContainer;
        private CheckBox timerToggle;
        private Button cooldownButton;
        private Timer timer;

        private string cooldownFormatted;
        #endregion

        #region Constructor
        public MyVisualTimer(string name, double cooldown, double flashCount = 4)
        {
            this.name = name;
            this.baseCooldown = cooldown;
            this.currentCooldown = cooldown;
            this.flashCount = flashCount;

            InitializeContainer();
            InitializeToggle();
            InitializeCooldownButton();
            InitializeTimer();
            UpdateLabel();
            ScaleContainer();
        }

        #endregion

        #region Methods
        internal void ChangeCooldown(double cd)
        {
            this.baseCooldown = cd;
            if (!timer.Enabled) this.currentCooldown = cd;
            UpdateLabel();
        }

        #region UI Methods
        public Panel GetPanel()
        {
            return myTimerContainer;
        }
        #endregion
        #endregion

        //If you can see it, it's here.
        #region User Interface, UI
        //Handles any visual updates that are made as the timer is running
        #region UI updates
        private void UpdateLabel()
        {
            TimeSpan t = TimeSpan.FromSeconds(currentCooldown);

            if (currentCooldown >= 3600)
            {
                cooldownFormatted = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                    t.Hours,
                    t.Minutes,
                    t.Seconds);
            }
            else if (currentCooldown >= 60)
            {
                cooldownFormatted = string.Format("{0:D2}m:{1:D2}s",
                    t.Minutes,
                    t.Seconds);
            }
            else
            {
                cooldownFormatted = string.Format("{0:D2}s",
                    t.Seconds);
            }

            cooldownButton.Text = name + " " + cooldownFormatted;

            UpdateLabelProgressColor();
        }

        private void UpdateLabelProgressColor()
        {
            //TODO: Changing cooldown while timers are running is causing and issue where currentCooldown/baseCooldown > 1 ---> RGB values greater than 255
            if (baseCooldown - currentCooldown < flashCount*2 && cooldownButton.BackColor != Color.Yellow && timer.Enabled) cooldownButton.BackColor = Color.Yellow;
            else
            {
                double p = currentCooldown / baseCooldown;
                cooldownButton.BackColor = Color.FromArgb(
                    (int)(Color.LightGreen.R * p + Color.LightPink.R * (1 - p)), //red
                    (int)(Color.LightGreen.G * p + Color.LightPink.G * (1 - p)), //green
                    (int)(Color.LightGreen.B * p + Color.LightPink.B * (1 - p))); //blue
            }
        }

        #endregion

        //Handles things you only need to do when initializing the timer UI
        #region UI initializing
        private void InitializeContainer()
        {
            myTimerContainer = new Panel
            {
                Location = new Point(0, 0),
                AutoSize = true,
                TabIndex = 0
                
            };
        }

        private void InitializeToggle()
        {
            timerToggle = new CheckBox
            {
                Size = new Size(18, 18),
                Checked = false
            };
            timerToggle.CheckedChanged += new EventHandler(ToggleChanged);
            myTimerContainer.Controls.Add(timerToggle);
        }

        private void InitializeCooldownButton()
        {
            cooldownButton = new Button();
            cooldownButton.Location = new Point(17, 0);
            cooldownButton.Size = new Size((int)(cooldownButton.Size.Width * 1.75), cooldownButton.Size.Height);
            cooldownButton.Text = name;
            cooldownButton.Click += new EventHandler(OnCooldownButtonClick);
            myTimerContainer.Controls.Add(cooldownButton);
        }

        private void ScaleContainer()
        {
            myTimerContainer.Size = new Size(myTimerContainer.Size.Width, 23);
        }
        #endregion
        #endregion

        //Start, Stop, Reset, etc.
        #region Timer controls
        public void Start()
        {
            timer.Enabled = true;
            timerToggle.Checked = true;
            PlayChime();
        }

        private void Stop()
        {
            this.timer.Enabled = false;
            timerToggle.Checked = false;
        }

        internal void Pause()
        {
            this.timer.Enabled = false;
        }

        public void Reset()
        {
            currentCooldown = baseCooldown;
            timer.Enabled = true;
            timerToggle.Checked = true;
            UpdateLabel();
            PlayChime();
        }

        private void ToggleChanged(object sender, EventArgs e)
        {
            if (timerToggle.Checked) Start();
            else Stop();
        }

        private void OnCooldownButtonClick(object sender, EventArgs e)
        {
            Reset();
        }
        #endregion

        //Handles initializing a forms timer that handles MyVisualTimer, calls for label changes, etc.
        #region Timer logics
        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(OnTick);
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (currentCooldown > 1)
            {
                currentCooldown--;
                UpdateLabel();
            }
            else
            {
                Reset();
            }
        }

        private void PlayChime()
        {
            System.Media.SystemSounds.Beep.Play();
        }
        #endregion
    }
}
