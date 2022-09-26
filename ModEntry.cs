
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
                    Game1.soundCategory.SetVolume(0.0f);
                    Game1.ambientCategory.SetVolume(0.0f);
                    Game1.footstepCategory.SetVolume(0.0f);
                }
                else
                {
                    Game1.currentSong.Resume();
                    if (volumeSaved)
                    {
                        Game1.soundCategory.SetVolume(soundVol);
                        Game1.ambientCategory.SetVolume(ambientVol);
                        Game1.footstepCategory.SetVolume(footstepVol);
                        volumeSaved = false;
                    }
                }
            }
        }
    }
}
