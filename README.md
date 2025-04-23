This is a work created by HAZAH7419, forked and modified for GBI uses.
Thank you for the wonderful work that you did.

https://github.com/HAZAH7419

AutoSeeder

AutoSeeder is a Windows desktop application built with C# WinForms that automates the process of joining and seeding Hell Let Loose servers at scheduled times. It includes time-based launching, macro-based server joining, and dynamic settings for multiple regions (Aliance and Australian modes).

🖐 If You See a Security Warning
This is expected with new software that isn’t code-signed yet.

To run it anyway:

Right-click the .zip → Properties → ✅ Check “Unblock” if shown

Extract the zip

Right-click the .exe → Properties → ✅ Check “Unblock” if shown

Run the app

Features

🕛 Scheduled Seeding

Automatically launches Hell Let Loose at specific times:

Weekdays: e.g., 5:00 PM (ET)

Weekends: e.g., 11:00 AM (ET)

Override mode to start immediately

🌍 Regional Modes

Aliance Mode (Default): Standard NA/EU settings

Australian Mode:

Different logo, color theme, and server name

Alternate time zone (AEST)

⚖️ Macro-Based Server Joiner

Waits 80 seconds for the game to fully load

Automatically:

Presses ESC to skip reconnect popups

Opens the server browser

Searches and joins the configured server

📊 Player Monitoring

Periodically checks server population via public API

Exits the game and optionally shuts down the PC when a threshold is reached (e.g., 80 players)

⚙️ Settings Interface

Set different seed times for weekdays and weekends

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

Administrator permissions (for computer shutdown functionality)

In-app time zone preview

Encrypted settings or secure mode for community builds

Credits

Built with love by the Helios community

Designed for simplicity, automation, and flexible server management

Enjoy your hands-free seeding!

