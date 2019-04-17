# Virtual Prototyping of Industrial Robot Based on Unity3D
## *Description*
1. Aimed to build 3D model of the industrial robot based on the Unity 3D, and use the six-axis motion data of the industrial robot to drive the 3d model, so as to realize real-time synchronous motion between virtual model and real object.
2. Completed the CAD modeling and motion analysis of the mechanical arm. Wrote the related program of virtual prototype of mechanical arm and built the display interface.
3. Used Socket to connect the industrial robot with the upper computer. Wrote the network service program between the upper computer and the lower computer, etc..

![Effect Picture](https://github.com/BKAUTO/Virtual-Prototyping-of-Industrial-Robot/blob/master/Effect.JPG)
<img style="width:800px;height:600px" src="https://github.com/BKAUTO/Virtual-Prototyping-of-Industrial-Robot/blob/master/Effect.JPG"  alt="真棒" align=center />

## *Preliminary Effect Display*
https://youtu.be/GJ9KVLfeftc  
(Version 1.0.2 with control function from PC & better visualization  UI)  

https://youtu.be/v7NjdfjCTQ  
(First version with only simultaneous motion)
## *Instruction*
Folder '虚拟样机展示系统' includes the **.exe** to show the effect. You may need a 6DOF industrial robot to connect with this software.  

Folder 'Virtual Factory_Version 1.0.X' is a Unity3D project.   
Recommend to use a version of Unity3D above **2018.2** to open this project.   
You can find all the codes under the directory **'Virtual Factory/Assets/Scripts'**.  

The latest Version 1.0.3 append the control of manipulator (Robot hand) which is also realized by TCP/IP connection.  
As a result, you should play the control program (CohandDemo_Windows) of the manipulaor (developed by C++) **after the connection between industrial robot and Unity3D software**. 

I am also completing an essay about this research, find the very shabby first version in Chinese [here](https://drive.google.com/open?id=1fVowi8dBVpidwzCsw2xcJZWmn_ISRV2L).
