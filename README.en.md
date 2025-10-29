# SCNET_Restart_Tool

## Project Overview

SCNET_Restart_Tool is an advanced monitoring and automatic restart tool specifically designed for SCNET server programs (such as Survivalcraft servers). By real-time monitoring of service running status, providing multiple intelligent restart mechanisms and remote control functions, this tool ensures continuous and stable operation of server programs, minimizes manual intervention, and improves service availability and operational efficiency.

![Main Interface Preview](Res/index.png)

## Core Features

### Multi-Server Management
- Support for configuring and managing multiple server instances simultaneously
- Each server can be independently configured with monitoring parameters and restart strategies
- Intuitive list interface displaying all servers' running status and basic information

### Intelligent Fault Detection and Auto-Restart
- Real-time monitoring of specified service process status, supporting dual verification mechanism (process name + path)
- Automatic restart when service unexpectedly terminates to ensure service continuity
- Built-in intelligent retry mechanism to handle startup failures
- Detailed operation logs for problem tracking and system analysis

### Flexible Restart Plan Strategies
- **Scheduled Restart:** Support setting daily fixed time points for automatic service restart
- **Interval Restart:** Configurable time interval (hours) for automatic service restart
- Send graceful shutdown commands before restart to ensure normal service termination
- Restart countdown prompts to keep users informed of system status changes

### Remote Command Control System
- TCP protocol interface for sending custom commands to the server remotely
- Password verification before command sending to ensure system security
- Real-time display of command execution results for remote operation monitoring

### Convenient Folder Management
- Integrated quick access to server-related folders
- Support for directly opening configuration, logs, plugins, mods, textures, worlds and other common folders
- Icon-based folder operation interface for improved management efficiency

### Comprehensive Log System
- Real-time recording of all operations and status changes
- Support for log file saving for later review and analysis
- Customizable log save path and recording level

### Program Self-Check and Permission Management
- Automatic detection of administrator privileges to ensure normal program operation
- Permission elevation options to avoid functionality limitations due to insufficient permissions

### Cross-Platform Support
- Windows version: Provides a friendly graphical user interface with simple and intuitive operation
- Linux version: Command-line interface suitable for server environment background operation

## Technical Architecture

### Development Technology Stack
- **Development Language:** C#
- **Framework:** .NET 9.0 (Windows version) / .NET 8.0 (Linux version)
- **GUI Framework:** Windows Forms (Windows version)
- **Network Communication:** Socket TCP/IP
- **Process Management:** System.Diagnostics, WMI
- **Configuration Management:** XML configuration file serialization
- **Log System:** Custom LogManager component

### Main Component Description

1. **ToolMain.cs** (`界面/Main/`)
   - Windows version's main form interface and core logic
   - Manages multiple server configurations and monitor instances
   - Handles user interaction, configuration management, and function scheduling

2. **ServerMonitor.cs**
   - Core component for server monitoring
   - Implements process monitoring, automatic restart, scheduled tasks and other functions
   - Provides interfaces for process detection, startup, stop, and command sending

3. **ServerConfig.cs**
   - Data structure definition for server configuration
   - Contains server basic information, monitoring parameters, and running status
   - Provides convenient access methods for server-related folder paths

4. **LogManager.cs**
   - Log management component designed with singleton pattern
   - Supports real-time log display and file saving functionality
   - Supports differentiated processing of different log levels (information, warning, error, debug)

5. **ServerStatus.cs**
   - Server status enumeration and status management class
   - Defines various running states of the server and their transition logic

6. **FolderManagerForm.cs** (`界面/FolderManagerCode/`)
   - Folder management interface and operation logic
   - Provides quick access to server-related folders

7. **SettingsForm.cs** (`界面/Settings/`)
   - Program global settings interface
   - Allows users to configure global options such as logs and permissions

## Installation and Configuration

### System Requirements
- **Windows Version:** Windows 10/11, .NET 9.0 Runtime or higher
- **Linux Version:** Linux distributions supporting .NET 8.0 (such as Ubuntu 22.04+, CentOS 9+, etc.)
- **Hardware Requirements:** Minimal system resources (CPU: 1GHz+, RAM: 512MB+)

### Windows Version Installation Steps
1. Ensure .NET 9.0 Runtime or higher is installed on the system
2. Download the compiled executable file or compile the source code using Visual Studio
3. Run the executable file directly or create a shortcut for easy access
4. The first time you run the program, the system will automatically check for administrator privileges to ensure normal program operation

### Linux Version Installation Steps
1. Ensure .NET 8.0 Runtime is installed on the system
2. Download the compiled Linux version executable file or compile from source code
3. Grant executable permissions and run via command line
4. Configure monitoring options, restart intervals, etc. according to prompts

## Configuration Guide

### Server Basic Configuration
1. Click the "Add Server" button on the main interface
2. Fill in server name, server program path and process name
3. Set monitoring parameters: detection interval, restart timeout, etc.
4. Configure restart strategy: scheduled restart or interval restart
5. Save configuration settings

### Remote Command Settings
1. Enable remote command function in server configuration
2. Set communication port and access password
3. Configure allowed command list
4. Save and apply settings

### Global Settings
1. Click the "Settings" button to enter the global settings interface
2. Configure log save path and recording level
3. Set program startup options (such as startup with system)
4. Configure interface theme and display options

## Usage Guide

### Basic Operation Process
1. **Add Server:** Click the "Add Server" button and fill in server information
2. **Start Monitoring:** Select the configured server and click the "Start Monitoring" button
3. **View Status:** View server running status and logs in real-time on the main interface
4. **Remote Control:** Use the remote command function to send instructions to the server
5. **Manage Folders:** Click the "Folder Management" button to quickly access server-related directories

### Scheduled Restart Settings
1. Select the server to be configured
2. Enable "Scheduled Restart" function
3. Set daily restart time points (multiple can be set)
4. Configure whether to send shutdown commands before restart and waiting time

### Interval Restart Settings
1. Select the server to be configured
2. Enable "Interval Restart" function
3. Set restart interval time (hours)
4. Configure preparation operations before restart

### Viewing Logs
1. Click the "Log" tab on the main interface
2. View real-time log records
3. Filter displayed content by log level
4. Click the "Export Log" button to save log files

## Project Structure

```
SCNET_Restart_Tool/
├── 界面/                  # User interface related code
│   ├── About/             # About dialog
│   ├── FolderManagerCode/ # Folder management related code
│   ├── Main/              # Main interface code
│   └── Settings/          # Settings interface code
├── Res/                   # Resource files
│   └── png/               # Icon image resources
├── App.config             # Application configuration
├── LogItem.cs             # Log item data structure
├── LogManager.cs          # Log management class
├── ProcessHelper.cs       # Process operation helper class
├── Program.cs             # Program entry point
├── ServerConfig.cs        # Server configuration class
├── ServerConfigManager.cs # Server configuration management class
├── ServerMonitor.cs       # Server monitoring core class
├── ServerStatus.cs        # Server status enumeration class
├── SCNET_Restart_Tool.csproj # Project file
└── SCNET_Restart_Tool.sln # Solution file
```

## Notes

1. **Administrator Privileges:** The program requires administrator privileges to correctly monitor and manage system processes, please ensure running as administrator
2. **Configuration File Backup:** Regular backup of program configuration files is recommended to prevent configuration loss
3. **Network Security:** When using remote command functionality, please ensure strong passwords are set and access IPs are restricted
4. **Log Management:** Regularly clean log files to avoid occupying too much disk space
5. **System Compatibility:** Windows version requires .NET 9.0 Runtime, Linux version requires .NET 8.0 Runtime

## Frequently Asked Questions

**Q: What to do if the program cannot detect the server process?**
A: Please check if the server program path and process name are correct, and ensure the server program is indeed running.

**Q: Why isn't the scheduled restart function working?**
A: Please check if the system time is accurate and if the scheduled restart settings are correctly saved.

**Q: How to make the program run automatically with system startup?**
A: Enable the "Run at Startup" option in global settings, or manually add the program shortcut to system startup items.

## Update Log

### v1.0.0
- Initial version release
- Implementation of basic server monitoring and automatic restart functions
- Support for multi-server management
- Windows graphical interface version provided

## License

This project is licensed under the MIT License - see the LICENSE file for details

## Contact Us

For questions or suggestions, please contact us through:
- Project Maintainer: [Project Maintainer Name]
- Feedback Email: [Feedback Email Address]
- GitHub Repository: [GitHub Repository Link]
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