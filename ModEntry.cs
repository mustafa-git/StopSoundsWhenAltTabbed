
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StopSoundsWhenAltTabbed
{
    public class ModEntry : Mod
    {
        // Store current settings to restore later.
        float soundVol;
        float ambientVol;
        float footstepVol;
        bool volumeSaved = false;
        bool pauseOptionValue;
        //use it to unpause for 3 ticks.
        int pauseCounter = 0;
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicking += this.OnUpdateTicking;
        }
        void OnUpdateTicking(object? sender, UpdateTickingEventArgs e)
        {
            if (Context.IsWorldReady)
            {
                if (!Game1.game1.IsActive)
                {
                    Game1.currentSong.Pause();
                    if (!volumeSaved)
                    {
                        soundVol = Game1.options.soundVolumeLevel;
                        ambientVol = Game1.options.ambientVolumeLevel;
                        footstepVol = Game1.options.footstepVolumeLevel;
                        volumeSaved = true;
                    }
                    Game1.options.ambientVolumeLevel = 0.0f;
                    Game1.options.soundVolumeLevel = 0.0f;
                    Game1.options.footstepVolumeLevel = 0.0f;
                    if (pauseCounter < 3)
                    {   
                        // Don't overwrite with wrong value.
                        if (pauseCounter < 1)
                            pauseOptionValue = Game1.options.pauseWhenOutOfFocus;
                        Game1.options.pauseWhenOutOfFocus = false;
                        pauseCounter += 1;
                    }
                    else
                    {
                        Game1.options.pauseWhenOutOfFocus = pauseOptionValue;
                    }
                }
                else
                {
                    Game1.currentSong.Resume();
                    if (volumeSaved)
                    {
                        Game1.options.soundVolumeLevel = soundVol;
                        Game1.options.ambientVolumeLevel = ambientVol;
                        Game1.options.footstepVolumeLevel = footstepVol;
                        volumeSaved = false;
                    }
                    if (pauseCounter == 3)
                    {
                        Game1.options.pauseWhenOutOfFocus = pauseOptionValue;
                        pauseCounter = 0;
                    }
                }
            }
        }
    }
}
