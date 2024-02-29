# streamerbot-yt-player
Play YouTube videos automatically in OBS using streamer.bot

## Usage  

### Api Key   

You will need Youtube API key in order to use this plugin. The plugin uses this to get video length in order to automatically hide the player.   
You can find information how to option api key from here: https://developers.google.com/youtube/registering_an_application      
Api key must be put to streamer.bot `Variables` section `Persistent Globals` with name `YOUTUBE_API_KEY`   

### Installation   

Create new action to Actions tab and add C# Execute Code Sub-Action. Copy contents of plugins.cs to Execute Code. 
Add following `References` in `Execute C# Code section`(right click somewhere in references tab to add):   
```
mscorlib.dll
System.XML.dll  
Google.Apis.YouTube.v3.dll   
Google.Apis.dll   
Google.Apis.Core.dll    
```
First two can usually be found in the folder what `Add reference from file` opens. Google API libraries can be found in streamer.bot installation directory. You can also try `Find refs` button.

Create following Argument type Sub-Actions:   
- `scene` this is name of your OBS scene    
- `source` this is name of your OBS browser source. Create it yourself in OBS. Script will not create it automatically.               
Example:     
   
![image](https://github.com/aslaki/streamerbot-yt-player/assets/15368361/f8724b41-82d2-4e12-8889-fbaa5c40d7d3)


