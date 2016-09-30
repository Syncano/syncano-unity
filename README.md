# Syncano Unity Library

## Overview
---

Syncano's Unity Library is written in C# and provides communication with [Syncano](http://www.syncano.io/) via HTTPS RESTful interface.
The full source code can be found on [Github](https://github.com/Syncano/syncano-unity/) - feel free to browse or contribute.

## Unity QuickStart Guide
---
To install Syncano Unity library download `SyncanoLibrary.1.0.0.unitypackage` and import it from Unity.

Alternative method requires downloading both Syncano.Unity.dll and Newtonsoft.Json.dll files and placing them anywhere under /Assets/ folder. The library uses Newtonsoft.Json.dll to process JSON files.
The latest version of Newtonsoft Json library can be downloaded from [here](https://github.com/SaladLab/Json.Net.Unity3D/releases).


For more detailed information on how to use Syncano and its features - our [Developer Manual](http://docs.syncano.com/docs/getting-started-with-syncano/?utm_source=github&utm_medium=readme) should be very helpful.
In case you need help working with the library - email us at [libraries@syncano.com](mailto:libraries@syncano.com) - we're always happy to help!

Happy coding!

## Multiplatform support & handling build errors
---
Syncano Unity Library supports all platforms available in Unity. Note some platforms for instance `WebGL` may require to add an external `link.xml` file to `/Assets/` folder which is available [here](https://gist.github.com/ssztangierski/6a2801882124f311d409770eb6b23fc0). Otherwise you may encounter build errors.

## Documentation
---
You can find more on using Syncano in our [documentation](http://docs.syncano.io/docs/android/?utm_source=github&utm_medium=readme&utm_campaign=syncano-unity).

## Contributing
---
We love contributions! Those who want to help us improve our Syncano library -- contribute to code, documentation, adding tests or making any other improvements -- please [create a Pull Request](https://github.com/Syncano/syncano-unity/pulls) with proposed changes.

## License
---
Syncano's Android library is available under the MIT license. 
See the [LICENSE](https://github.com/Syncano/syncano-unity/blob/master/LICENSE) file for more info.
