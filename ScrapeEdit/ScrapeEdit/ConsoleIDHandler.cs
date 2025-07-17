using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ScrapeEdit
{
    internal class ConsoleIDHandler
    {
        public static Dictionary<string, string[]> consoleIds;

        public static string filePath = Path.Combine(
             Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
             "ScrapeEdit",
             "console_ids.xml"
         );


        public static Dictionary<string, string[]> LoadConsoleIds()
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Console ID file not found", filePath);

            var doc = XDocument.Load(filePath);

            return doc.Root.Elements("Console")
                .ToDictionary(
                    el => el.Attribute("key").Value,
                    el => new string[]
                    {
                    el.Attribute("id").Value,
                    el.Attribute("name").Value
                    });
        }

        public static void SaveConsoleIds(Dictionary<string, string[]> data)
        {
            var doc = new XDocument(
                new XElement("Consoles",
                    data.Select(kvp =>
                        new XElement("Console",
                            new XAttribute("key", kvp.Key),
                            new XAttribute("id", kvp.Value[0]),
                            new XAttribute("name", kvp.Value[1])
                        )
                    )
                )
            );

            doc.Save(filePath);
        }

        public static string GetConsoleID(string console)
        {
            if (!consoleIds.ContainsKey(console))
                return null;

            return consoleIds[console][0];
        }
        public static string GetConsoleNameFull(string console)
        {
            return consoleIds[console][1];
        }

        public static Dictionary<string, string[]> consoleIds_backup = new Dictionary<string, string[]>
        {
            { "megadrive",           new string[] { "1",   "SEGA Megadrive" } },
            { "genesis",             new string[] { "1",   "SEGA Genesis" } },
            { "mastersystem",        new string[] { "2",   "SEGA Master System" } },
            { "nes",                 new string[] { "3",   "Nintendo NES" } },
            { "snes",                new string[] { "4",   "Nintendo Super Nintendo" } },
            { "cps1",                new string[] { "6",   "Capcom Play System" } },
            { "cps2",                new string[] { "7",   "Capcom Play System 2" } },
            { "cps3",                new string[] { "8",   "Capcom Play System 3" } },
            { "gb",                  new string[] { "9",   "Nintendo Game Boy" } },
            { "gbc",                 new string[] { "10",  "Nintendo Game Boy Color" } },
            { "virtualboy",          new string[] { "11",  "Nintendo Virtual Boy" } },
            { "gba",                 new string[] { "12",  "Nintendo Game Boy Advance" } },
            { "gamecube",            new string[] { "13",  "Nintendo Gamecube" } },
            { "n64",                 new string[] { "14",  "Nintendo 64" } },
            { "nds",                 new string[] { "15",  "Nintendo DS" } },
            { "wii",                 new string[] { "16",  "Nintendo Wii" } },
            { "3ds",                 new string[] { "17",  "Nintendo 3DS" } },
            { "wiiu",                new string[] { "18",  "Nintendo Wii U" } },
            { "sega32x",                 new string[] { "19",  "SEGA Megadrive 32X" } },
            { "megacd",              new string[] { "20",  "SEGA Mega-CD" } },
            { "gamegear",            new string[] { "21",  "SEGA Game Gear" } },
            { "saturn",              new string[] { "22",  "SEGA Saturn" } },
            { "dreamcast",           new string[] { "23",  "SEGA Dreamcast" } },
            { "neogeoaes",           new string[] { "24",  "SNK Neo-Geo AES" } },
            { "ngp",                 new string[] { "25",  "SNK Neo-Geo Pocket" } },
            { "atari2600",           new string[] { "26",  "Atari 2600" } },
            { "jaguar",              new string[] { "27",  "Atari Jaguar" } },
            { "lynx",                new string[] { "28",  "Atari Lynx" } },
            { "3do",                 new string[] { "29",  "Panasonic 3DO" } },
            { "ngage",               new string[] { "30",  "Nokia N-Gage" } },
            { "pcengine",            new string[] { "31",  "PC Engine" } },
            { "xbox",                new string[] { "32",  "Microsoft Xbox" } },
            { "xbox360",             new string[] { "33",  "Microsoft Xbox 360" } },
            { "xboxone",             new string[] { "34",  "Microsoft Xbox One" } },
            { "arcadeemu",           new string[] { "35",  "Another Arcade Emulator" } },
            { "atom",                new string[] { "36",  "Atom" } },
            { "bbcmicro",            new string[] { "37",  "BBC Micro" } },
            { "atari800",            new string[] { "38",  "Atari 800" } },
            { "atari2600supercharger", new string[] { "39", "Atari 2600 Supercharger" } },
            { "atari5200",           new string[] { "40",  "Atari 5200" } },
            { "atari7800",           new string[] { "41",  "Atari 7800" } },
            { "atarist",             new string[] { "42",  "Atari ST" } },
            { "atari8bit",           new string[] { "43",  "Atari 8bit" } },
            { "astrocade",           new string[] { "44",  "Astrocade" } },
            { "wswan",               new string[] { "45",  "WonderSwan" } },
            { "wswanc",              new string[] { "46",  "WonderSwan Color" } },
            { "cave",                new string[] { "47",  "Cave" } },
            { "colecovision",        new string[] { "48",  "Colecovision" } },
            { "daphne",              new string[] { "49",  "Daphne" } },
            { "coregrafx",           new string[] { "50",  "CoreGrafX" } },
            { "coregrafx2",          new string[] { "51",  "CoreGrafX II" } },
            { "gameandwatch",        new string[] { "52",  "Nintendo Game & Watch" } },
            { "atomiswave",          new string[] { "53",  "Atomiswave" } },
            { "model2",              new string[] { "54",  "SEGA Model 2" } },
            { "model3",              new string[] { "55",  "SEGA Model 3" } },
            { "naomi",               new string[] { "56",  "SEGA Naomi" } },
            { "psx",                 new string[] { "57",  "Sony Playstation" } },
            { "ps2",                 new string[] { "58",  "Sony Playstation 2" } },
            { "ps3",                 new string[] { "59",  "Sony Playstation 3" } },
            { "ps4",                 new string[] { "60",  "Sony Playstation 4" } },
            { "psp",                 new string[] { "61",  "Sony PSP" } },
            { "psvita",              new string[] { "62",  "Sony PS Vita" } },
            { "android",             new string[] { "63",  "Android - Smartphone" } },
            { "amiga",               new string[] { "64",  "Commodore Amiga" } },
            { "amstradcpc",          new string[] { "65",  "Amstrad CPC" } },
            { "c64",                 new string[] { "66",  "Commodore 64" } },
            { "supercassettevision", new string[] { "67",  "Epoch Super Cassette Vision" } },
            { "neogeomvs",           new string[] { "68",  "SNK Neo-Geo MVS" } },
            { "st-v",                new string[] { "69",  "SEGA ST-V - Console & Arcade" } },
            { "neogeocd",            new string[] { "70",  "SNK Neo-Geo CD" } },
            { "fightcade",           new string[] { "71",  "Fightcade - Online" } },
            { "pcfx",                new string[] { "72",  "PC-FX" } },
            { "vic20",               new string[] { "73",  "Commodore Vic-20" } },
            { "pv1000",              new string[] { "74",  "Casio PV-1000" } },
            { "mame",                new string[] { "75",  "Mame" } },
            { "zxspectrum",          new string[] { "76",  "ZX Spectrum" } },
            { "zx81",                new string[] { "77",  "ZX81" } },
            { "adventurevision",     new string[] { "78",  "Entex Industries, Inc Adventure Vision" } },
            { "x68000",              new string[] { "79",  "Sharp X68000" } },
            { "channelf",            new string[] { "80",  "Channel F" } },
            { "actionmax",           new string[] { "81",  "Action Max" } },
            { "neogeopocketcolor",   new string[] { "82",  "SNK Neo-Geo Pocket Color" } },
            { "aamberpegasus",       new string[] { "83",  "Aamber Pegasus" } },
            { "archimedes",          new string[] { "84",  "Archimedes" } },
            { "electron",            new string[] { "85",  "Electron" } },
            { "apple2",              new string[] { "86",  "Apple II" } },
            { "gx4000",              new string[] { "87",  "Amstrad GX4000" } },
            { "camputerslynx",       new string[] { "88",  "Camputers Lynx" } },
            { "adam",                new string[] { "89",  "Adam" } },
            { "megaduck",            new string[] { "90",  "Mega Duck" } },
            { "dragon32_64",         new string[] { "91",  "Dragon 32/64" } },
            { "eg2000",              new string[] { "92",  "EG2000 Colour Genie" } },
            { "elektronikabk",       new string[] { "93",  "Elektronika BK" } },
            { "arcadia2001",         new string[] { "94",  "Arcadia 2001" } },
            { "epochgamepocket",     new string[] { "95",  "Epoch Game Pocket Computer" } },
            { "exl100",              new string[] { "96",  "EXL 100" } },
            { "fm7",                 new string[] { "97",  "Fujitsu FM-7" } },
            { "loopy",               new string[] { "98",  "Casio Loopy" } },
            { "cplus4",              new string[] { "99",  "Commodore Plus/4" } },
            { "superacan",           new string[] { "100", "Super A'can" } },
            { "gp32",                new string[] { "101", "GP32" } },
            { "vectrex",             new string[] { "102", "Vectrex" } },
            { "gamemaster",          new string[] { "103", "Game Master" } },
            { "o2em",                new string[] { "104", "Videopac G7000" } },
            { "supergrafx",          new string[] { "105", "PC Engine SuperGrafx" } },
            { "fds",                 new string[] { "106", "Nintendo Family Computer Disk System" } },
            { "satellaview",         new string[] { "107", "Nintendo Satellaview" } },
            { "sufamiturbo",         new string[] { "108", "Sufami Turbo" } },
            { "sg1000",              new string[] { "109", "SEGA SG-1000" } },
            { "nintendopower",       new string[] { "110", "Nintendo Power" } },
            { "amigaaga",            new string[] { "111", "Commodore Amiga (AGA)" } },
            { "taitotypx",           new string[] { "112", "Taito Type X" } },
            { "msx1",                new string[] { "113", "Microsoft MSX" } },
            { "pcenginecd",          new string[] { "114", "PC Engine CD-Rom" } },
            { "intellivision",       new string[] { "115", "Intellivision" } },
            { "msx2",                new string[] { "116", "Microsoft MSX2" } },
            { "msx2plus",            new string[] { "117", "Microsoft MSX2+" } },
            { "msxturbor",           new string[] { "118", "Microsoft MSX Turbo R" } },
            { "gbaereader",          new string[] { "119", "Nintendo GBA e-Reader" } },
            { "vsmile",              new string[] { "120", "VTech V.Smile" } },
            { "gamecom",             new string[] { "121", "Game.com" } },
            { "n64dd",               new string[] { "122", "Nintendo 64DD" } },
            { "scummvm",             new string[] { "123", "ScummVM" } },
            { "mikrosha",            new string[] { "124", "Mikrosha" } },
            { "pecom64",             new string[] { "125", "Pecom 64" } },
            { "jupiterace",          new string[] { "126", "Jupiter Ace" } },
            { "supergameboy",        new string[] { "127", "Nintendo Super Game Boy" } },
            { "supergameboy2",       new string[] { "128", "Nintendo Super Game Boy 2" } },
            { "amigacdtv",           new string[] { "129", "Commodore Amiga CDTV" } },
            { "amigacd32",           new string[] { "130", "Commodore Amiga CD32" } },
            { "oric1",               new string[] { "131", "Oric 1 / Atmos" } },
            { "cdi",                 new string[] { "133", "CD-i" } },
            { "amigacd",             new string[] { "134", "Commodore Amiga CD" } },
            { "dos",                 new string[] { "135", "Microsoft PC Dos" } },
            { "dxx-rebirth",         new string[] { "135", "Microsoft PC Dos" } },
            { "eduke32",             new string[] { "135", "Microsoft PC Dos" } },
            { "fallout1-ce",         new string[] { "135", "Microsoft PC Dos" } },
            { "fallout2-ce",         new string[] { "135", "Microsoft PC Dos" } },
            { "win3xx",              new string[] { "136", "Microsoft PC Win3.xx" } },
            { "win9x",               new string[] { "137", "Microsoft PC Win9X" } },
            { "windows",             new string[] { "138", "Microsoft PC Windows" } },
            { "doom3",               new string[] { "138", "Microsoft PC Windows" } },
            { "devilutionx",         new string[] { "138", "Microsoft PC Windows" } },
            { "quake3",              new string[] { "138", "Microsoft PC Windows" } },
            { "amigacd32hack",       new string[] { "139", "Commodore Amiga CD32 (hack)" } },
            { "mo5",                 new string[] { "140", "MO5" } },
            { "moto",                new string[] { "141", "Thomson MO/TO" } },
            { "neogeo",              new string[] { "142", "SNK Neo-Geo - Console & Arcade" } },
            { "zenpinballfx2",       new string[] { "143", "Zen Pinball FX2" } },
            { "trs80color",          new string[] { "144", "TRS-80 Color Computer" } },
            { "publiclinux",         new string[] { "145", "Public Domain Linux" } },
            { "macos",               new string[] { "146", "Apple Mac OS" } },
            { "segaclassics",        new string[] { "147", "SEGA Classics" } },
            { "iremclassics",        new string[] { "148", "Irem Classics" } },
            { "seta",                new string[] { "149", "Seta" } },
            { "midwayclassics",      new string[] { "150", "Midway Classics" } },
            { "capcomclassics",      new string[] { "151", "Capcom Classics" } },
            { "eightingraizing",     new string[] { "152", "Eighting / Raizing" } },
            { "tecmo",               new string[] { "153", "Tecmo" } },
            { "snkclassics",         new string[] { "154", "SNK Classics" } },
            { "namcoclassics",       new string[] { "155", "Namco Classics" } },
            { "namcosystem22",       new string[] { "156", "Namco System 22" } },
            { "taitoclassics",       new string[] { "157", "Taito Classics" } },
            { "konamiclassics",      new string[] { "158", "Konami Classics" } },
            { "jaleco",              new string[] { "159", "Jaleco" } },
            { "atariclassics",       new string[] { "160", "Atari Classics" } },
            { "nintendoclassics",    new string[] { "161", "Nintendo Classics" } },
            { "dataeastclassics",    new string[] { "162", "Data East Classics" } },
            { "nmk",                 new string[] { "163", "NMK" } },
            { "sammyclassics",       new string[] { "164", "Sammy Classics" } },
            { "exidy",               new string[] { "165", "Exidy" } },
            { "acclaim",             new string[] { "166", "Acclaim" } },
            { "psikyo",              new string[] { "167", "Psikyo" } },
            { "nonjeu",              new string[] { "168", "non Jeu" } },
            { "technos",             new string[] { "169", "Technos" } },
            { "americanlasergames",  new string[] { "170", "American Laser Games" } },
            { "atarijaguarcd",       new string[] { "171", "Atari Jaguar CD" } },
            { "psminis",             new string[] { "172", "Sony Playstation minis" } },
            { "dynax",               new string[] { "173", "Dynax" } },
            { "kaneko",              new string[] { "174", "Kaneko" } },
            { "videosystemco",       new string[] { "175", "Video System Co." } },
            { "igs",                 new string[] { "176", "IGS" } },
            { "comad",               new string[] { "177", "Comad" } },
            { "amcoe",               new string[] { "178", "Amcoe" } },
            { "centuryelectronics",  new string[] { "179", "Century Electronics" } },
            { "nichibutsu",          new string[] { "180", "Nichibutsu" } },
            { "visco",               new string[] { "181", "Visco" } },
            { "alphadenshi",         new string[] { "182", "Alpha Denshi Co." } },
            { "coleco",              new string[] { "183", "Coleco" } },
            { "playchoice",          new string[] { "184", "PlayChoice" } },
            { "atlus",               new string[] { "185", "Atlus" } },
            { "banpresto",           new string[] { "186", "Banpresto" } },
            { "semicom",             new string[] { "187", "SemiCom" } },
            { "universal",           new string[] { "188", "Universal" } },
            { "mitchell",            new string[] { "189", "Mitchell" } },
            { "seibukaihatsu",       new string[] { "190", "Seibu Kaihatsu" } },
            { "toaplan",             new string[] { "191", "Toaplan" } },
            { "cinematronics",       new string[] { "192", "Cinematronics" } },
            { "incredibletech",      new string[] { "193", "Incredible Technologies" } },
            { "gaelco",              new string[] { "194", "Gaelco" } },
            { "megatech",            new string[] { "195", "SEGA Mega-Tech" } },
            { "megaplay",            new string[] { "196", "SEGA Mega-Play" } },
            { "flipper",             new string[] { "197", "Flipper" } },
            { "visualpinball",       new string[] { "198", "Visual Pinball" } },
            { "futurepinball",       new string[] { "199", "Future Pinball" } },
            { "pinballarcade",       new string[] { "200", "the Pinball Arcade" } },
            { "zenpinballfx3",       new string[] { "201", "Zen Pinball FX3" } },
            { "sneshacks",           new string[] { "202", "Nintendo Snes - Super Mario World Hacks" } },
            { "megadrivehacks",      new string[] { "203", "SEGA Megadrive - Sonic The Hedgehog 2 Hacks" } },
            { "ti99",                new string[] { "205", "Texas Instruments TI-99/4A" } },
            { "lutro",               new string[] { "206", "Lutro" } },
            { "supervision",         new string[] { "207", "Watara Supervision" } },
            { "pc9801",              new string[] { "208", "NEC PC-9801" } },
            { "gottlieb",            new string[] { "209", "Gottlieb" } },
            { "msu1",                new string[] { "210", "Nintendo Super MSU-1" } },
            { "pokemini",            new string[] { "211", "Nintendo Pokémon mini" } },
            { "samcoupe",            new string[] { "213", "MGT SAM Coupé" } },
            { "openbor",             new string[] { "214", "OpenBOR" } },
            { "zmachine",            new string[] { "215", "Z-Machine" } },
            { "uzebox",              new string[] { "216", "Uzebox" } },
            { "iigs",                new string[] { "217", "Apple IIGS" } },
            { "spectravideo",        new string[] { "218", "Spectravideo" } },
            { "palmos",              new string[] { "219", "Palm OS" } },
            { "x1",                  new string[] { "220", "Sharp X1" } },
            { "pc8801",              new string[] { "221", "NEC PC-8801" } },
            { "tic80",               new string[] { "222", "TIC-80" } },
            { "solarus",             new string[] { "223", "Solarus" } },
            { "switch",              new string[] { "225", "Nintendo Switch" } },
            { "naomigdrom",          new string[] { "227", "SEGA Naomi GD-ROM" } },
            { "naomi2",              new string[] { "230", "SEGA Naomi 2" } },
            { "easyrpg",             new string[] { "231", "EasyRPG" } },
            { "pico8",               new string[] { "234", "Pico-8" } },
            { "pocketchallengev2",   new string[] { "237", "Pocket Challenge V2" } },
            { "pet",                 new string[] { "240", "Commodore PET" } },
            { "creativision",        new string[] { "241", "VTech CreatiVision" } },
            { "lowresnx",            new string[] { "244", "LowRes NX" } },
            { "trs80serie",          new string[] { "248", "TRS-80 Serie" } },
            { "segapico",            new string[] { "250", "SEGA Pico" } },
            { "fmtowns",             new string[] { "253", "Fujitsu FM Towns" } },
            { "hikaru",              new string[] { "258", "SEGA Hikaru" } },
            { "vg5000",              new string[] { "261", "Philips VG 5000" } },
            { "wasm4",               new string[] { "262", "WASM-4" } },
            { "arduboy",             new string[] { "263", "Arduboy" } },
            { "gamate",              new string[] { "266", "Gamate" } },
            { "teknoparrot",         new string[] { "269", "TeknoParrot" } },
            { "vircon32",            new string[] { "272", "Vircon32" } },
            { "voxatron",            new string[] { "275", "Voxatron" } },
            { "neshacks",            new string[] { "278", "Nintendo Nes - Super Mario Bros. Hacks" } },
            { "vc4000",              new string[] { "281", "VC 4000" } },
            { "ps5",                 new string[] { "284", "Sony Playstation 5" } },
            { "p2000t",              new string[] { "287", "Philips P2000T" } },
            { "prboom",              new string[] { "290", "PrBoom" } },
            { "flatpak",             new string[] {  "-1", "flatpak"} },
            { "mrboom",              new string[] {  "-1", "mrboom"} },
            { "odcommander",         new string[] {  "-1", "OD Commander"} },
            { "pygame",              new string[] {  "-1", "PyGame" } },
            { "sdlpop",              new string[] {  "-1", "Prince of Persia" } },
            { "steam",               new string[] {  "-1", "Steam" } },


        };
    
    }
}
