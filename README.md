# RConsole

This is a command interpreter that I made for my PC to control it in various ways from a single point.

Though many better tools out there exist, I made this more so to challenge myself.

The idea for the structure of this command interpreter was to make it modular. This allowed me to easily register added commands by only having to add the single file that the logic for the command exists in. If I wanted to remove a command, I would only have to delete the command's class file without needing to remove extra code elsewhere.

This command interpreter can also interface through an Arduino Micro that is connected to the PC through USB. The Arduino is connected to two physical buttons which make the Arduino send messages back to the console to activate the user-specified commands.

Bluetooth is also an option to send messages to the interpreter. Currently, I have made an Android app that can connect and send commands wirelessly.
