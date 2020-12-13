# voice-commands
Inspired by CSGO in-game voice commands. This project allows you to do similar thing but in any other games. Technically, what this project does is that it captures global keyboard input and play the relevant voice commands.  

## Things to note
- All audios must be in .wav format. (Voicepacks are not included. You need to put your own audios.)  
- Usage:  
   + If you have multiple voicepacks, choose a voicepack with the combobox at the bottom.
   + Choose voice command to play with NUMPAD 1-9.  
   + Press NUMPAD 0 to return.  
   + Press NUMPAD + to exit.  
- Voicepacks must be placed in the same folder as the executable. And the folder structure must be "./[Voicepack]/[1. Command 1]/[1. Sub-command 1]/\*.wav". For example, the CSGO equivalent would be:  
```
voicepack 1/  
├── 1. Report Radio Message/  
│   ├── 1. Enemy Spotted/  
|   |   ├── any-name.wav  
|   |   ├── alternate-voice.wav  
|   |   ├── another-alternate-voice.wav  
|   |   └── ...  
│   ├── 2. Need Backup/  
|   |   └── ...  
│   ├── 3. Taking Fire, Need Assistance/  
|   |   └── ...  
│   └── ...  
├── 2. Standard Radio Message/  
│   └── ...  
└── 3. Command Radio Message/  
    └── ...  
```
Therefore, to play "Need Backup" voice, for instance, you need to press NUMPAD 1, then NUMPAD 2.  

## Prerequisite
On its own, this project is no different from an audio player. However, with VB Audio Virtual Cable drivers installed, this project will automatically play the audio on VB-Audio's "Cable Input" which you can pipe to VB-Audio's "Cable Output" to work as a recording device like your microphone.  
- VB Audio Virtual Cable: https://vb-audio.com/Cable/
