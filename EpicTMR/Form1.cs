using System;
using System.Drawing;
using System.Windows.Forms;

namespace EpicTMR
{
    /*PLANNING TODO
     * Note: The audio cues are referred to as chimes.
     * 
     * #########################
     * FIXES/CHANGES:
     * 
     * Figure out why Windows Defender doesn't like me :D
     *  note: Could be because the program is not in their database and therefore is not whitelisted
     *
     *
     * #########################
     * FUTURE FEATURES:
     *
     * Figure out if WinForms needs to be replaced with something in order to turn this in to a proper program
     * 
     * Add proper audio to chimes to replace SystemSounds.Beep.Play();
     *  Hunt: Arrow hit sound
     *  Materials: Axe chopping?
     *  Training: Pages being turned?
     *  Adventure: A horn of some sort?
     *  Quest: An actual chime sound?
     *  
     * Add custom timer
     *  Counter for already created timers so created timers are properly aligned in UI
     *  Add option to delete custom timers
     *  Add option for custom chimes?
     * 
     * Chime queue so that two sounds do not overlap
     *  -> New class for handling audio?
     *      -> Custom audio?
     *
     * 
     * #########################
     * FUTURE UI IMPROVEMENTS:
     *
     * Remove checkbox and re-design button functionality to a 1-2-3 form
     *  1st press: Start
     *  2nd press: Stop
     *  3rd press: Reset
     *  -Figure out if an eventhandler can be added to "swiping" the button for quick Reset + Restart
     *  -Mousewheel up / down: Instantly increment by 1 (or 5?) seconds. Works while the timer is running
     *
     * Settings menu
     * -Move %cdr and extra interval
     *
     * Settings for each timer
     * -%cdr and extra intervals for each timer separately
     *  -Option to "Overwrite on main window changes"
     *  -Will have to edit constructor to accept double cdMod and int interval, default to main window if available, else default to 1, 0 respectively
     * 
     *
     * Automatic scaling
     *  X and Y scales / Figure out if there is a scale property in Windows.Forms
     *  Event handler to main window that is called on resize
     *  
     * Smart resize on-load. Maybe a saved variable?
     * 
     * re-designed and re-apply Recent Chimes -panel.
     *  -Open a secondary window?
     * 
     * New skin with EpicTMR theme
     *
     * "Always on top" -feature
     *
     *
     * #########################
     * DISTANT FUTURE FEATURES:
     *
     * Turn in to an all-around timer program
     *  -New icon. I like the design but perhaps make the colors Greyscale and the picture HD instead of pixel art
     *
     * Add a timer-package importer (to preserve EpicRPG functionality)
     *
     * Add the option to change skins / import them as standalone or with packages
     *
     * Store timer variables and compare to system time to allow tracking created timers even when program is not running
     *
     * Drag and drop timers
     *
     *
     * #########################
     * DISTANT FUTURE UI:
     *
     * Make the UI look clean and less Win95 -ish
     * 
     * Make it possible to drag timers out of the actual window
     *  -Always on top per timer
     *  -Opacity per timer
     *  -Opacity when hovered over
     *  -Click-through without shortcuts/modifiers (ctrl, alt, shift, etc)
     */
    public partial class Main : Form
    {
        #region Variables
        //Basic information
        //Basic cooldowns for timer creation
        private static readonly object[] CommandJournal = new object[]
        {
            new[]{"Hunt"      ,   "Materials",   "Training",   "Adventure",   "Quest"  },
            new double[]{1 * Min    ,   5 * Min   ,   15 * Min ,   1 * Hour  ,   6 * Hour}
        };
        //Constants to make hardcoded cooldowns more readable
        private const int Min = 60;
        private const int Hour = 60 * Min;

        //Variables for handling timers
        //Variable for altering startStopBtn functionality
        private static bool _timerIsRunning;
        //Keeps track of created arrays
        private static int MyVisualTimerCount = ((string[])CommandJournal[0]).Length;
        //Array for storing timers
        private static readonly MyVisualTimer[] Timers = new MyVisualTimer[MyVisualTimerCount];
        //Container for storing timers
        private static Panel _cdPanel;

        //User modified values
        //Modifier for Patreon cooldown reduction
        private static double _cdModifier = 1;
        //Modifier for added seconds to give extra reaction time to prompts
        private static double _intervalDelay = 5;
        #endregion

        /// <summary>
        /// Loads the main window and initializes MyVisualTimers using the hardcoded cooldowns
        /// </summary>
        public Main()
        {
            InitializeComponent();
            InitializeCdPanel();
        }

        #region Cooldown panel
        /// <summary>
        /// Initializes the panel in which generated MyVisualTimers are stored and then calls CreateTimers()
        /// </summary>
        private void InitializeCdPanel()
        {
            _cdPanel = new Panel
            {
                Location = new Point(this.ClientSize.Width / 2, 0),
                Name = "cdPanel",
                Size = new Size(this.ClientSize.Width / 2, Math.Max(this.ClientSize.Height / 2, 20 + CommandJournal.Length) * 23),
                TabIndex = 0,
            };
            this.Controls.Add(_cdPanel);

            Label cdHeader = new Label
            {
                Text = @"Cooldowns:",
                Location = new Point(0, 2),
                AutoSize = true
            };
            _cdPanel.Controls.Add(cdHeader);
            
            CreateTimers();
        }
        #endregion

        #region Start all / Stop all button handling
        /// <summary>
        /// Event handler for starting and stopping all generated MyVisualTimers at the same time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartStopButtonClick(object sender, EventArgs e)
        {
            if (!_timerIsRunning)
            {
                _timerIsRunning = true;
                StartHandler();
                startStopBtn.Text = @"Stop all timers";
            }
            else
            {
                _timerIsRunning = false;
                StopHandler();
                startStopBtn.Text = @"Start all timers";
            }
        }

        /// <summary>
        /// Stops all generated MyVisualTimers at the same time
        /// </summary>
        private static void StopHandler()
        {
            foreach (var timer in Timers)
            {
                timer.Stop();
            }
        }

        /// <summary>
        /// Resets and starts all generated MyVisualTimers at the same time
        /// </summary>
        private static void StartHandler()
        {
            foreach (var timer in Timers)
            {
                timer.ReStart();
            }
        }
        #endregion

        #region Verifying users input and updating settings
        /// <summary>
        /// Parses user input from CooldownTextBox (cdr) and validates it. Updates _cdModifier
        ///
        /// Validation: cdr = [0,100]
        /// Valid: _cdModifier = (100 - cdr) / 100
        /// Invalid: _cdModifier = 1 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CooldownTextBox_TextChanged(object sender, EventArgs e)
        {
            _cdModifier = 1;
            cooldownTextBox.BackColor = Color.LightPink;

            try
            {
                double cdr = double.Parse(cooldownTextBox.Text.Split('%')[0]);
                if (0 <= cdr && cdr <= 100)
                {
                    cooldownTextBox.BackColor = Color.White;
                    _cdModifier = (100 - cdr) / 100;
                }
            }
            catch (Exception)
            { 
                //
            }

            UpdateCooldowns();
        }

        /// <summary>
        /// Parses user input from intervalTextBox (interval) and validates it. Updates _intervalDelay.
        ///
        /// Validation: interval is a positive value
        /// Valid: _intervalDelay = interval
        /// Invalid: _intervalDelay = 5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IntervalTextBox_TextChanged(object sender, EventArgs e)
        {
            intervalTextBox.BackColor = Color.LightPink;
            _intervalDelay = 5;

            try
            {
                double interval = double.Parse(intervalTextBox.Text.Split('s')[0]);
                if (interval >= 0)
                {
                    intervalTextBox.BackColor = Color.White;
                    _intervalDelay = interval;
                }
            }
            catch (Exception)
            {
                //
            }

            UpdateCooldowns();
        }

        /// <summary>
        /// Calls MyVisualTimers' method ChangeCooldown() with updated cooldowns after user has altered _cdModifier or _intervalDelay values
        /// </summary>
        private static void UpdateCooldowns()
        {
            double[] cd = ((double[]) CommandJournal[1]);
            for (int i = 0; i < cd.Length; i++)
            {
                Timers[i].ChangeCooldown(cd[i] * _cdModifier + _intervalDelay);
            }
        }
        #endregion

        #region Timer logics
        /// <summary>
        /// Creates timers and sets their intervals based on cdValuesDouble
        /// </summary>
        private static void CreateTimers()
        {
            for (int i = 0; i < MyVisualTimerCount; i++)
            {
                //Logical
                string name = ((string[])CommandJournal[0])[i];
                double cooldown = ((double[])CommandJournal[1])[i] * _cdModifier + _intervalDelay;
                var tempMyTimer = new MyVisualTimer(name, cooldown);
                Timers[i] = tempMyTimer;

                //Visual
                Panel temp = tempMyTimer.GetPanel();
                temp.Location = new Point(0, 18 + i * 25);
                _cdPanel.Controls.Add(temp);
            }
        }
        #endregion

    }
}
