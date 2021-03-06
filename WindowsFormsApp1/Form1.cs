using System;
using System.Drawing;
using System.Windows.Forms;

namespace EpicTMR
{
    /*PLANNING
     * Note: The audio cues are referred to as chimes.
     * 
     * #########################
     * FIXES:
     * 
     * Remove chime on manual resets
     * 
     * Replace current progress colors with only displaying red when near expiring and green for max(5, intervalSeconds) seconds after a chime
     * 
     * Figure out why Windows Defender doesn't like me :D
     *  note: Could be because the program is not in their database and therefor is not whitelisted
     *  
     *  Change title from "EpicRPG EpicTMR" to just "EpicTMR"
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
     *  X and Y scales / Figure out if there is a scale property in winforms
     *  Event handler to main window that is called on resize
     *  
     * Smart resize on-load
     * 
     * Flash count setting
     * 
     * re-designed Recent Chimes -panel
     * 
     * New skin with EpicTMR theme
     */
    public partial class Main : Form
    {
        #region class variables
        //Stop condition for timer
        private static bool timerIsRunning = false;

        //Probably shouldn't have these here but helps me read what i'm doing
        private static int _min = 60;
        private static int _hour = 60 * _min;

        //Cooldown tracking
        private static object[] commandJournal = new object[]
        {
            new string[]{"Hunt"      ,   "Materials",   "Training",   "Adventure",   "Quest"  },
            new double[]{1 * _min    ,   5 * _min   ,   15 * _min ,   1 * _hour  ,   6 * _hour}
        };

        //Variable for initializing arrays
        private static int cJournLength = ((string[])commandJournal[0]).Length;

        //Modifier for patreon cooldown reduction
        private static double cdModifier = 1;
        //Modifier for added seconds to give extra reaction time to prompts
        private static double intervalDelay = 5;
        //Array for storing base cooldowns
        private static MyVisualTimer[] timers = new MyVisualTimer[cJournLength];

        private static Panel cdPanel;
        #endregion

        public Main()
        {
            InitializeComponent();
            InitializeCdPanel();
        }

        #region Cooldown panel
        private void InitializeCdPanel()
        {
            cdPanel = new Panel
            {
                Location = new Point(this.ClientSize.Width / 2, 0),
                Name = "cdPanel",
                Size = new Size(this.ClientSize.Width / 2, Math.Max(this.ClientSize.Height / 2, 20 + commandJournal.Length) * 23),
                TabIndex = 0
            };
            this.Controls.Add(cdPanel);

            Label cdHeader = new Label
            {
                Text = "Cooldowns:",
                Location = new Point(0, 2),
                AutoSize = true
            };
            cdPanel.Controls.Add(cdHeader);

            CreateTimers();
        }
        #endregion

        #region Button click handling
        private void StartStopButtonClick(object sender, EventArgs e)
        {
            if (!timerIsRunning)
            {
                timerIsRunning = true;
                StartHandler();
            }
            else
            {
                timerIsRunning = false;
                StopHandler();
            }

        }

        private void StopHandler()
        {
            foreach (MyVisualTimer timer in timers)
            {
                timer.Reset();
                timer.Pause();
            }
        }

        private void StartHandler()
        {
            foreach (MyVisualTimer timer in timers)
            {
                timer.Start();
            }
        }
        #endregion

        #region Text Box handling
        private void CooldownTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double cdr = double.Parse(cooldownTextBox.Text.Split('%')[0]);
                if (cdr >= 0 && cdr <= 100)
                {
                    cooldownTextBox.BackColor = Color.White;
                    cdModifier = (100 - cdr) / 100;
                    UpdateCooldowns();
                }
                else
                {
                    cooldownTextBox.BackColor = Color.LightPink;
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
                    intervalDelay = interval;
                    UpdateCooldowns();
                }
                else
                {
                    IntervalTextBox.BackColor = Color.LightPink;
                }
            }
            catch (Exception)
            {
                IntervalTextBox.BackColor = Color.LightPink;
            }
        }

        private void UpdateCooldowns()
        {
            for (int i = 0; i < cJournLength; i++)
            {
                double cd = ((double[])commandJournal[1])[i] * cdModifier + intervalDelay;
                timers[i].ChangeCooldown(cd);
            }
        }
        #endregion

        #region Timer logics

        //Creates timers and sets their intervals based on cdValuesDouble
        private void CreateTimers()
        {
            for (int i = 0; i < cJournLength; i++)
            {
                //Logical
                string name = ((string[])commandJournal[0])[i];
                double cooldown = ((double[])commandJournal[1])[i] * cdModifier + intervalDelay;
                MyVisualTimer tempMyTimer = new MyVisualTimer(name, cooldown);
                timers[i] = tempMyTimer;

                //Visual
                Panel temp = tempMyTimer.GetPanel();
                temp.Location = new Point(0, 18 + i * 25);
                cdPanel.Controls.Add(temp);
            }
        }
        #endregion

    }
}
