using System;
using System.Drawing;
using System.Windows.Forms;

namespace EpicTMR
{
    /*PLANNING TODO
     * Note: The audio cues are referred to as chimes.
     * 
     * #########################
     * FIXES:
     * 
     * Figure out why Windows Defender doesn't like me :D
     *  note: Could be because the program is not in their database and therefor is not whitelisted
     *
     * Add proper documentation
     * 
     * #########################
     * FUTURE FEATURES:
     * 
     * Add proper audio to chimes
     *  Hunt: Arrow hit sound
     *  Materials: Axe chopping?
     *  Training: Pages being turned?
     *  Adventure: A horn of some sort?
     *  Quest: ?
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
     * UI IMPROVEMENTS:
     * 
     * Automatic scaling
     *  X and Y scales / Figure out if there is a scale property in Windows.Forms
     *  Event handler to main window that is called on resize
     *  
     * Smart resize on-load
     * 
     * re-designed Recent Chimes -panel
     * 
     * New skin with EpicTMR theme
     */
    public partial class Main : Form
    {
        #region class variables
        //Stop condition for timer
        private static bool _timerIsRunning;

        //Probably shouldn't have these here but helps me read what i'm doing
        private static readonly int Min = 60;
        private static readonly int Hour = 60 * Min;

        //Cooldown tracking
        private static readonly object[] CommandJournal = new object[]
        {
            new[]{"Hunt"      ,   "Materials",   "Training",   "Adventure",   "Quest"  },
            new double[]{1 * Min    ,   5 * Min   ,   15 * Min ,   1 * Hour  ,   6 * Hour}
        };

        //Variable for initializing arrays
        private static readonly int CommandJournalLength = ((string[])CommandJournal[0]).Length;

        //Modifier for patreon cooldown reduction
        private static double _cdModifier = 1;
        //Modifier for added seconds to give extra reaction time to prompts
        private static double _intervalDelay = 5;
        //Array for storing base cooldowns
        private static readonly MyVisualTimer[] Timers = new MyVisualTimer[CommandJournalLength];

        private static Panel _cdPanel;
        #endregion

        public Main()
        {
            InitializeComponent();
            InitializeCdPanel();
        }

        #region Cooldown panel
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
                Text = @"Cool-downs:",
                Location = new Point(0, 2),
                AutoSize = true
            };
            _cdPanel.Controls.Add(cdHeader);
            
            CreateTimers();
        }
        #endregion

        #region Button click handling
        private void StartStopButtonClick(object sender, EventArgs e)
        {
            if (!_timerIsRunning)
            {
                _timerIsRunning = true;
                StartHandler();
                btnStartTimers.Text = @"Stop all timers";
            }
            else
            {
                _timerIsRunning = false;
                StopHandler();
                btnStartTimers.Text = @"Start all timers";
            }

        }

        private void StopHandler()
        {
            foreach (var timer in Timers)
            {
                timer.Stop();
            }
        }

        private void StartHandler()
        {
            foreach (var timer in Timers)
            {
                timer.ReStart();
            }
        }
        #endregion

        #region Verifying settings
        private void CooldownTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double cdr = double.Parse(cooldownTextBox.Text.Split('%')[0]);
                if (cdr >= 0 && cdr <= 100)
                {
                    cooldownTextBox.BackColor = Color.White;
                    _cdModifier = (100 - cdr) / 100;
                    UpdateCooldowns();
                }
                else
                {
                    cooldownTextBox.BackColor = Color.LightPink;
                    _cdModifier = 1;
                }
            }
            catch (Exception)
            {
                cooldownTextBox.BackColor = Color.LightPink;
            }
        }

        private void IntervalTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double interval = double.Parse(IntervalTextBox.Text.Split('s')[0]);
                if (interval >= 0)
                {
                    IntervalTextBox.BackColor = Color.White;
                    _intervalDelay = interval;
                    UpdateCooldowns();
                }
                else
                {
                    IntervalTextBox.BackColor = Color.LightPink;
                    _intervalDelay = 5;
                }
            }
            catch (Exception)
            {
                IntervalTextBox.BackColor = Color.LightPink;
            }
        }

        private void UpdateCooldowns()
        {
            for (int i = 0; i < CommandJournalLength; i++)
            {
                double cd = ((double[])CommandJournal[1])[i] * _cdModifier + _intervalDelay;
                Timers[i].ChangeCooldown(cd);
            }
        }
        #endregion

        #region Timer logics

        //Creates timers and sets their intervals based on cdValuesDouble
        private void CreateTimers()
        {
            for (int i = 0; i < CommandJournalLength; i++)
            {
                //Logical
                string name = ((string[])CommandJournal[0])[i];
                double cooldown = ((double[])CommandJournal[1])[i] * _cdModifier + _intervalDelay;
                MyVisualTimer tempMyTimer = new MyVisualTimer(name, cooldown);
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
