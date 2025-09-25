# Survival Wars Server Auto Restart Tool

This tool is designed to provide automatic restart functionality for the "Survival Wars" game server, ensuring the server runs continuously and stably.

## Features

- **Automatic Restart:** Automatically restarts the service when service termination is detected.
- **Scheduled Restart:** Supports restarting the service at specified time points.
- **Interval Restart:** Supports restarting the service at specified time intervals.
- **Remote Command Sending:** Allows commands to be sent to the server via the network.

## Technologies Used

- Written in C#, compatible with the .NET Framework environment.
- Uses Windows Forms to build the graphical user interface.
- Supports configuration saving and loading.

## Key Components

- `Form1.cs`: Main form logic, including timer, process control, and other functionalities.
- `LinuxRestartTool/Program.cs`: Restart tool logic for Linux platforms.
- `Properties/AssemblyInfo.cs`: Configuration of assembly information.

## Installation and Running

Ensure that the .NET Framework runtime environment is installed on your system. After downloading the project, open the solution file using Visual Studio and compile/run it.

## Contribution Guide

Code contributions to improve this tool are welcome. Please submit Pull Requests or Issues to the project's Gitee page.

## License

This project is licensed under the MIT License. For details, please refer to the LICENSE file included in the project.

---

For further information, please consult the specific code files of the project.