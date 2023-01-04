# TRAVE_unity
[![Open in Visual Studio Code](https://img.shields.io/static/v1?logo=visualstudiocode&label=&message=Open%20in%20Visual%20Studio%20Code&labelColor=2c2c32&color=007acc&logoColor=007acc)](https://open.vscode.dev/krocky-cooky/TRAVE_unity)

TRAVE_unity is device operation interface for TRAVE.

TRAVE project is supported by [MITOU foundation.](https://www.ipa.go.jp/jinzai/mitou/outline.html#:~:text=%E3%80%8C%E6%9C%AA%E8%B8%8F%E3%80%8D%E3%81%AF%E3%80%81%E7%B5%8C%E6%B8%88%E7%94%A3%E6%A5%AD,%E8%82%B2%E3%81%A6%E3%82%8B%E3%81%9F%E3%82%81%E3%81%AE%E4%BA%8B%E6%A5%AD%E3%81%A7%E3%81%99%E3%80%82)

## install
Unity package is supported. 
Download unity package from [here](https://github.com/krocky-cooky/TRAVE_unity/releases) and import it.

## usage
### Place the prefab in a scene
![Inspector of the prefab](./Image/editor_sample.png)
 Set up communication parameters.
### Manipulate from scripts
``` C#
using UnityEngine;
using TRAVE;

public class SampleClass : MonoBehaviour
{
  TRAVEDevice device = TRAVEDevice.GetDevice();
  
  void Start()
  {
    //Make connection with TRAVE device
    device.MakeConnection();
    
    //Set up torque mode to 1.0
    device.SetTorqueMode(1.0f);
    
    //Apply changes to TRAVE device
    device.Apply();
  }
 }
```
## License
[MIT License](https://github.com/krocky-cooky/TRAVE_unity/blob/main/LICENSE)
