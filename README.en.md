# SCNET_Restart_Tool

## Project Introduction

SCNET_Restart_Tool is a professional automatic restart management tool designed specifically for SCNET server programs. It ensures the continuous, stable, and reliable operation of SCNET server programs through multiple monitoring mechanisms and flexible restart strategies, reducing manual intervention and improving service availability.

## Features

### 1. Automatic Fault Detection and Restart
- Monitors the running status of SCNET server programs in real-time
- Automatically detects service exceptions, crashes, or unexpected terminations
- Initiates restart procedures immediately upon detecting issues to minimize service downtime

### 2. Timed Scheduled Restart
- Supports setting daily or weekly scheduled restart plans
- Allows configuring multiple time points for restart operations
- Provides countdown reminders before scheduled restarts to ensure users are informed

### 3. Interval Cycle Restart
- Supports configuring fixed interval restart cycles (hours/minutes)
- Automatically calculates the next restart time after each operation
- Maintains service freshness and prevents potential memory leaks or performance degradation

### 4. Remote Command Management
- Built-in network communication module for sending remote commands
- Supports execution of predefined commands on the server
- Ensures operational flexibility without direct server access

### 5. Configuration Persistence
- Automatically saves user configurations to ensure settings are retained after program restart
- Supports exporting and importing configuration files for backup and migration

### 6. Cross-Platform Support
- Contains both Windows Forms-based GUI version and Linux command-line version
- Ensures consistent functionality across different operating environments

## Technical Architecture

### Core Technologies
- **Development Language:** C#
- **Framework:** .NET Framework 4.7.2 (Windows) / .NET 8.0 (Linux)
- **GUI Framework:** Windows Forms
- **Process Management:** .NET Process Class
- **Configuration Storage:** XML Serialization

### Main Components
- **Form1.cs:** Core logic of the main form, including timer management, process control, and user interaction
- **LinuxRestartTool/Program.cs:** Command-line version for Linux platforms
- **Properties/AssemblyInfo.cs:** Program assembly information and metadata configuration
- **App.config:** Application configuration file for runtime settings

## Installation and Configuration

### System Requirements
- **Windows Version:** Windows 7/8/10/11, .NET Framework 4.7.2 or higher
- **Linux Version:** Any Linux distribution with .NET 8.0 Runtime installed
- **Hardware Requirements:** Minimal system resources (CPU: 1GHz+, RAM: 512MB+)

### Installation Steps
1. **Windows Version Installation:**
   - Ensure .NET Framework 4.7.2 or higher is installed on the system
   - Download the compiled executable file or compile the source code using Visual Studio
   - Run the executable file directly or create a shortcut for convenient access

2. **Linux Version Installation:**
   - Ensure .NET 8.0 Runtime is installed on the system
   - Download the compiled Linux version executable or compile from source code
   - Grant executable permissions and run via the command line

### Configuration Instructions
1. **Windows Version Configuration:**
   - After running the program, configure the SCNET server path, restart parameters, etc., through the graphical interface
   - Click the "Save Settings" button to save the configuration
   - The configuration is automatically saved to the program directory

2. **Linux Version Configuration:**
   - Edit the configuration file (if applicable) or pass parameters through the command line
   - Configure monitoring options, restart intervals, etc.

## Usage Guide

### Using the Windows Version
1. **Start the Program:** Double-click the executable file to launch the application
2. **Set the Server Path:** Click the "Browse" button to select the SCNET server executable file
3. **Configure Monitoring Options:**
   - Enable automatic restart monitoring
   - Set scheduled restart times (if needed)
   - Configure interval restart cycle (if needed)
4. **Start Monitoring:** Click the "Start Monitoring" button to begin the monitoring process
5. **Remote Command Usage:** Enter commands in the command input box and click "Send Command"

### Using the Linux Version
1. **Open Terminal:** Access the system terminal
2. **Navigate to Program Directory:** Use the `cd` command to go to the program location
3. **Run the Program:** Execute `dotnet LinuxRestartTool.dll` with appropriate parameters
4. **Monitoring Operation:** The program will run in the background or terminal, monitoring the SCNET server status

### Remote Command Description
- The remote command function allows sending predefined commands to the SCNET server
- Ensure that the network connection between the client and server is normal
- Commands are executed in the context of the SCNET server process

## Frequently Asked Questions (FAQ)

**Q: What should I do if the program fails to detect the SCNET server?**
A: Please check if the server path is correctly set and ensure that the SCNET server can be started normally.

**Q: How to set multiple scheduled restart times?**
A: In the Windows version, you can set multiple time points through the settings interface. For the Linux version, refer to the command line parameters documentation.

**Q: Is it possible to run multiple monitoring instances simultaneously?**
A: Yes, but each instance should be configured to monitor different SCNET server instances to avoid conflicts.

**Q: How to view the program running log?**
A: The Windows version provides a log display area in the main interface. The Linux version outputs logs to the terminal by default.

**Q: Will the program automatically restart after system reboot?**
A: You need to add the program to the system startup items manually or configure it as a system service.

## Development and Contribution

### Development Environment Setup
1. **Required Software:**
   - Visual Studio 2022 or higher
   - .NET Framework 4.7.2 Development Tools
   - .NET 8.0 SDK (for Linux version development)

2. **Project Import:**
   - Clone the project repository using Git
   - Open the solution file (.sln) with Visual Studio
   - Restore NuGet packages (if needed)

3. **Compilation Process:**
   - Select the appropriate build configuration (Debug/Release)
   - Click "Build Solution" to compile the project
   - The compiled files will be generated in the `bin` directory

### Contribution Guidelines
- Fork the project repository and create your own branch for development
- Ensure that the code follows the existing coding standards
- Submit comprehensive test cases for new features
- Create a Pull Request describing the changes made and their purpose

## Disclaimer

- This tool is provided "as is" without any warranty. The author shall not be liable for any direct or indirect damages arising from the use of this tool.
- Users are responsible for backing up their data and configurations before using this tool.
- It is recommended to conduct sufficient testing in a non-production environment before applying this tool to production systems.

---

For more detailed technical information, please refer to the source code and comments within the project files.