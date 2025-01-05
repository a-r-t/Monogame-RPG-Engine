using Monogame_RPG_Engine.Engine.Core;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_RPG_Engine.Engine.Sound
{
    // A wrapper class for the build in MediaPlayer class
    public static class SongPlayer
    {
        public static Song CurrentSong { get; set; }

        // set to true when a song has been attempted to be played using the "play" method
        private static bool SongHasBeenPlayed = false;

        // note: if play is called while a song is playing, it will stop song and play new song
        // if same song is already playing when play is called again on it, it will restart the song
        public static void Play()
        {
            SongHasBeenPlayed = false;
            if (CurrentSong != null)
            {
                MediaPlayer.Play(CurrentSong);
                SongHasBeenPlayed = true;
            }
        }

        public static void Pause()
        {
            MediaPlayer.Pause();
        }

        public static void Resume()
        {
            MediaPlayer.Resume();
        }

        public static void Stop()
        {
            MediaPlayer.Stop();
        }

        // same thing as calling Play a second time while the same song is currently playing
        // just adding this method to make the code more self documenting :)
        public static void Restart()
        {
            Play();
        }

        public static void Mute()
        {
            MediaPlayer.IsMuted = true;
        }

        public static void Unmute()
        {
            MediaPlayer.IsMuted = false;
        }

        // volume is number between 0 (muted) and 1 (loudest)
        public static void SetVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public static void SetPlayerMode(SongPlayerMode playerMode)
        {
            if (playerMode == SongPlayerMode.PLAY_ONCE)
            {
                MediaPlayer.IsRepeating = false;
            }
            else if (playerMode == SongPlayerMode.PLAY_LOOP)
            {
                MediaPlayer.IsRepeating = true;
            }
        }

        // assuming the song actually played successfully AND the player is not in LOOP mode, this will return when a song has finished playing once
        // if "stop" is called manually, it will still treat that as having played once
        public static bool HasSongPlayedOnce
        {
            get
            {
                return CurrentSong != null && SongHasBeenPlayed && MediaPlayer.State == MediaState.Stopped;
            }
        }

        // resets MediaPlayer to defeault values
        public static void ResetPlayer()
        {
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = false;
            MediaPlayer.IsMuted = false;
            MediaPlayer.Volume = 1.0f;
            SongHasBeenPlayed = false;
            CurrentSong = null;
        }
    }
}
