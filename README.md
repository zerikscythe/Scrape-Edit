# ScrapeEdit

**ScrapeEdit** is a powerful Windows desktop application for managing, scraping, and editing metadata and media for game ROM libraries. Designed for **Batocera**, **EmulationStation**, and other frontends that use `gamelist.xml` files, ScrapeEdit combines intuitive UI with deep customization, caching, and batch processing features.

WARNING! This program is still very much a work in progress. Many things may change/break and with that I am not responsible for you corrupting or loosing your roms or gamelists!
While i have not had issues with that in my testing... I'm just saying...

This is a passion project so updates or features may come in bursts with possible gaps in replys/updates. But should you use this software and find it of value or of promise, please feel free to throw me some suggestions. I'll flush out a "roadmap" of hopefull adds and edits later.

Thanks, ZerikScythe
---

## ✨ Key Features

### 🗂️ ROM Folder Tree View
- Automatically detects and displays ROMs organized by console folders
- Highlights:
  - **Gray**: Games missing metadata
  - **Black**: Fully scraped entries
- Supports nested folder structures and M3U playlist files

### 📝 Gamelist Management
- Parses and edits `gamelist.xml` per system folder
- Enables full editing of metadata:
  - Title, Description, Genre, Release Date, Players, Developer, etc.
- Saves new gamelists only after confirmed changes or batch operations

### 🖼️ Media Viewer & Assignment
- Preview and assign:
  - Screenshot
  - Thumbnail
  - Marquee
  - Video
  - Manual
- Context menu support for drag-and-drop image assignment
- Optionally rename and relocate media for local use

### 🌐 ScreenScraper API Integration
- Scrapes metadata using:
  - CRC32, MD5, or SHA1 hash
  - ROM filename (fallback mode)
- Verifies hash match before trusting API data
- Caches XML and media per-console
- Automatically injects missing `<rom>` tags in API response for compatibility
- Built-in re-scrape delay system (30 days) (Optional)

### ⚙️ Batch Scraping & Concurrency
- Scrape entire folders, individual games, or custom filters
- Up to **5 concurrent scraping threads**
- Dynamic UI panels display scrape progress in real time
- Progress panel includes error summaries and game list writing queue

### 💾 Media Cache & File Handling
- Central cache folder (`ScrapeEdit Cache`) stores master media
- Automatically rebuilds lost files from cache
- Supports alternate download types and fallback media rules

### 🔧 Advanced User Settings
- Persistent configuration for:
  - File paths (ROM root, cache)
  - ScreenScraper credentials
  - Language and region preferences
  - Image naming formats
- Includes full setting UI via `SettingsControlPanel` (replaces previous forms)

### 📁 M3U Playlist Tool
- Generate `.m3u` files for games with multiple discs or parts
- Options for:
  - Subdirectory creation
  - Hiding original ROMs
  - Copying scraped metadata

---

## 🚀 Setup Instructions

1. **Launch ScrapeEdit**
2. Set your:
   - ROM root directory (e.g. `batocera\roms`)
   - (Optional override) ScrapeEdit cache folder is stored in the `%appdata%/ScrapeEdit/Cache` by default.
3. Enter your ScreenScraper **username/password**
4. (Optional) Provide **Dev ID & Key** if you are looking to work with the source code
5. Right-click on a game or folder and choose **Scrape**
6. Edit metadata/media and click **Save Changes**

---

## ⚠️ Dev Credentials Required

> 🔐 If compiling this software yourself, you **must provide your own** ScreenScraper Dev ID and Key.  
> These are stored securely and never committed to source control.

---

## 📝 Notes
- Supported file extensions come from `_info.txt` or built-in defaults
- ROMs, metadata, and media can be renamed or deleted via context menu
- (Optional setting) Scraped data is matched against your hashes to ensure accuracy

---

## 👤 Author

**Developed by Zerikscythe**

For feedback, issues, or contributions, contact the developer directly or open an issue in the GitHub repository (if public).
