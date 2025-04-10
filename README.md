Helios AutoSeeder

Helios AutoSeeder is a Windows desktop application built with C# WinForms that automates the process of joining and seeding Hell Let Loose servers at scheduled times. It includes time-based launching, macro-based server joining, and dynamic settings for multiple regions (Helios and Australian modes).

Features

🕛 Scheduled Seeding

Automatically launches Hell Let Loose at specific times:

Weekdays: e.g., 5:00 PM (ET)

Weekends: e.g., 11:00 AM (ET)

Override mode to start immediately

🌍 Regional Modes

Helios Mode (Default): Standard NA/EU settings

Australian Mode:

Different logo, color theme, and server name

Alternate time zone (AEST)

⚖️ Macro-Based Server Joiner

Waits 90 seconds for the game to fully load

Automatically:

Presses ESC to skip reconnect popups

Opens the server browser

Searches and joins the configured server

📊 Player Monitoring

Periodically checks server population via public API

Exits the game and optionally shuts down the PC when a threshold is reached (e.g., 80 players)

⚙️ Settings Interface

Set different seed times for weekdays and weekends

Manage and edit multiple servers

Save separate configurations per mode

Restore defaults independently for each region

ℹ️ Info Panel

Easy-to-access info button shows how everything works with macro timings and full descriptions

Configuration

settings.json

Used to store all saved configuration including:

Weekday & weekend seed times

Server list and selected server

UI theme settings (color, logo)

Current mode and time zone

The file is automatically created and updated by the application. You can also ship a default version.

Deployment

Ensure settings.json, macro images, and .exe macro are included in your output folder

Can be published via Visual Studio, ClickOnce, or third-party packagers

Requirements

Windows 10/11

Steam installed

Python + PyAutoGUI (if using macro method)

Administrator permissions (for computer shutdown functionality)

Roadmap Ideas

In-app time zone preview

Multiple profile support

Encrypted settings or secure mode for community builds

Credits

Built with love by the Helios community

Designed for simplicity, automation, and flexible server management

Enjoy your hands-free seeding!

