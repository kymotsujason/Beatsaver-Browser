Beatsaver-Browser

![alt text](https://i.imgur.com/s14jyi6.png)

This is a standalone application for managing custom songs.
It keeps track of downloaded and installed songs, so you know what maps you've got and what maps to get.
Built for Beatsaver.com but can be extended to other websites if needed.

Requirements:
Song Loader: Download the recommended package here https://github.com/Umbranoxio/BeatSaberModInstaller/releases

Benchmarks (dependent on internet, scores from 75mbps internet + nvme SSD):

Load speed from internet: ~375 maps/minute

Load speed from disk: ~500 maps/second

Download speed: ~85 maps/minute

Estimated size: ~215 maps/GB

Install/Uninstall/Delete speed: Instant

Memory usage: ~2500 maps/GB

Current features:
- Browse songs
- Download songs (loading/downloading may take a while, but will be worth it later on)
- Install songs
- Uninstall songs
- Keep track of downloaded/installed files
- Remember song info and load at startup (loading is hyper fast!)
- Apply status to maps (downloaded/installed)
- Song Preview (listen to music before deciding to install andplay it)
- Adjustable map loading range (default is latest, checked on startup)

TODO (important):
- Implement an actual GUI (eventually)

TODO (lower priority):
- Existing song detection (possible, but may require workarounds)
- Display only needed information (or implement an actual GUI)
- More safety checks, once more conflicts/issues arise